﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FuryTechs.LinuxAdmin.Identity
{
  public class SignInManager : SignInManager<User>
  {
    public SignInManager(
      UserManager<User> userManager,
      IHttpContextAccessor contextAccessor,
      IUserClaimsPrincipalFactory<User> claimsFactory,
      IOptions<IdentityOptions> optionsAccessor,
      ILogger<SignInManager<User>> logger,
      IAuthenticationSchemeProvider schemes,
      IUserConfirmation<User> userConfirmation) : base(
      userManager,
      contextAccessor,
      claimsFactory,
      optionsAccessor,
      logger,
      schemes,
      userConfirmation)
    {

    }
    public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent,
      bool lockoutOnFailure)
    {
      return base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
    }

    public override Task<SignInResult> PasswordSignInAsync(User user, string password, bool isPersistent,
      bool lockoutOnFailure)
    {
      return base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
    }
  }
}
