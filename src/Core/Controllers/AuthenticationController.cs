using System;
using System.Threading.Tasks;
using FuryTechs.LinuxAdmin.Identity;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FuryTechs.LinuxAdmin.Core.Controllers {

    public class LoginModel {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    [Route ("api/authentication")]
    public class AuthenticationController : ControllerBase {
        private LoginModel LoginModel { get; set; }

        [HttpGet]
        public IActionResult Generate ([FromServices] IAntiforgery antiforgery) {
            var tokens = antiforgery.GetAndStoreTokens (this.HttpContext);
            return Ok (new { tokens.HeaderName, tokens.RequestToken });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (
            [FromBody] LoginModel login, [FromServices] SignInManager<User> signInManager
        ) {
            await Task.CompletedTask;
            LoginModel = login;

            if (!ModelState.IsValid)
                return BadRequest ();

            var result = await signInManager.PasswordSignInAsync (login.UserName, login.Password, false, true);
            if (result.Succeeded) {
                return Ok (new { Success = true });
            }

            return Unauthorized (new { Success = false, Message = "Invalid username, or password!" });
        }

    }
}
