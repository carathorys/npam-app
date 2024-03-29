using System;
using System.Threading.Tasks;
using FuryTechs.LinuxAdmin.Identity;
using FuryTechs.LinuxAdmin.IdentityProvider.Models;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Linq;
using FuryTechs.LinuxAdmin.IdentityProvider.Models.Cryptography;
using System.Text.Unicode;
using FuryTechs.LinuxAdmin.IdentityProvider.Extensions;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Controllers
{

  [AllowAnonymous]
  public class AccountController : ControllerBase
  {
    [HttpGet]
    public IActionResult Generate([FromServices] IAntiforgery antiforgery)
    {
      var tokens = antiforgery.GetAndStoreTokens(this.HttpContext);
      var (rsa, padding) = GetEncryptionKey();
      try
      {
        return Ok(new
        {
          tokens.HeaderName,
          tokens.RequestToken,
          pub = rsa.ToJwk(
            padding: RSAEncryptionPadding.OaepSHA256,
            includePrivateKey: false
          )
        });
      }
      finally
      {
        rsa.Dispose();
      }
    }

    private (RSA Rsa, RSAEncryptionPadding Padding) GetEncryptionKey()
    {
      if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString("Encryption")))
      {
        var rsa = RSA.Create(2048);
        HttpContext.Session.SetString("Encryption", Newtonsoft.Json.JsonConvert.SerializeObject(rsa.ToJwk(RSAEncryptionPadding.OaepSHA256, true)));
        return (rsa, RSAEncryptionPadding.OaepSHA256);
      }

      var jwk = Newtonsoft.Json.JsonConvert.DeserializeObject<RsaJwk>(HttpContext.Session.GetString("Encryption"));
      return jwk.ToRSA();
    }

    [HttpGet]
    public async Task<IActionResult> Login() => Redirect("/authentication" + Request.QueryString);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
      [FromBody] LoginModel model,
      [FromServices] UserManager<User> userManager,
      [FromServices] SignInManager<User> signInManager,
      [FromServices] IIdentityServerInteractionService interaction,
      [FromServices] IClientStore clientStore,
      [FromServices] IAuthenticationSchemeProvider schemeProvider,
      [FromServices] IEventService events
    )
    {
      if (!ModelState.IsValid)
        return BadRequest();

      var context = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);

      var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberLogin, true);

      if (result.Succeeded)
      {
        var user = await userManager.FindByNameAsync(model.UserName);
        await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

        return Ok();

      }

      return Unauthorized();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
      await HttpContext.SignOutAsync();
      return Ok();
    }
  }
}
