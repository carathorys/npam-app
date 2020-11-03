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

namespace FuryTechs.LinuxAdmin.IdentityProvider.Controllers
{

  [AllowAnonymous]
  public class AccountController : ControllerBase
  {

    [HttpGet]
    public IActionResult Generate([FromServices] IAntiforgery antiforgery)
    {
      var tokens = antiforgery.GetAndStoreTokens(this.HttpContext);
      return Ok(new
      {
        tokens.HeaderName,
        tokens.RequestToken
      });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
      LoginModel model,
      [FromServices] UserManager<User> userManager,
      [FromServices] IIdentityServerInteractionService interaction,
      [FromServices] IClientStore clientStore,
      [FromServices] IAuthenticationSchemeProvider schemeProvider,
      [FromServices] IEventService events
    )
    {
      if (!ModelState.IsValid)
        return BadRequest();

      var context = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);

      var user = await userManager.FindByNameAsync(model.UserName);
      if (user != null)
      {
        var pwResult = await userManager.CheckPasswordAsync(user, model.Password);
        if (pwResult)
        {
          await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

          // only set explicit expiration here if user chooses "remember me". 
          // otherwise we rely upon expiration configured in cookie middleware.
          AuthenticationProperties props = null;
          if (model.RememberLogin)
          {
            props = new AuthenticationProperties
            {
              IsPersistent = true,
              ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(31))
            };
          };

          var issuer = new IdentityServerUser(user.Id)
          {
            DisplayName = user.UserName
          };
          var princ = issuer.CreatePrincipal();
          await HttpContext.SignInAsync(issuer.CreatePrincipal(), props);

        }
      }
      // var result = await .PasswordSignInAsync(login.UserName, login.Password, false, true);
      // if (result.Succeeded)
      // {
      //   return Ok(new LoginResult { Success = true });
      // }

      return Ok(new LoginResult { Success = false, ErrorMessage = "Invalid username or password!" });
    }
  }
}
