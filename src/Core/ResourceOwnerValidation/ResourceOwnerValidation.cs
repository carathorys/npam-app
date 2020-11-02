namespace FuryTechs.LinuxAdmin.Core.Validation
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using System;
  using IdentityServer4.Validation;
  using Microsoft.Extensions.Logging;
  using Npam.Interop;
  using Npam;
  using IdentityModel;
  using System.Security.Claims;

  public class PamResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
  {
    private readonly ILogger Logger;

    public PamResourceOwnerPasswordValidator(ILogger<PamResourceOwnerPasswordValidator> logger)
    {
      Logger = logger;
    }

    public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
      var username = context.UserName;
      var password = context.Password;
      Logger.LogInformation("Check user's credentials: {user}", username);
      using var session = new NpamSession("sudo", username.ToLower(), ConversionHandler(password), IntPtr.Zero);
      var retval = session.Start();

      if (retval != PamStatus.PAM_SUCCESS)
      {
        // TODO: PAM initialization issue?
        context.Result = new GrantValidationResult
        {
          IsError = true,
          Error = "Invalid username or password!"
      };
      }
      else
      {
        retval = session.Authenticate(0);

        if (retval != PamStatus.PAM_SUCCESS)
        {
          // TODO: PAM authentication issue?
          context.Result = new GrantValidationResult
          {
            IsError = true,
            Error = "Invalid username or password!"
          };

        }
        else
        {
          retval = session.AccountManagement(0);

          if (retval != PamStatus.PAM_SUCCESS)
          {
            // TODO: PAM account issue
            context.Result = new GrantValidationResult
            {
              IsError = true,
              Error = "Invalid username or password!"

            };
          }
          else
          {
            // TODO: User signed in successfully!

            context.Result = new GrantValidationResult
            {
              IsError = false,
              Subject = new System.Security.Claims.ClaimsPrincipal(Principal.Create("PAM", new System.Security.Claims.Claim[] {
                new Claim(IdentityModel.JwtClaimTypes.Subject, context.UserName),
                new Claim(IdentityModel.JwtClaimTypes.AuthenticationTime, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(IdentityModel.JwtClaimTypes.IdentityProvider, "PAM")
              }))
            };
          }
        }
      }

      return Task.CompletedTask;
    }

    private NpamSession.ConvCallbackDelegate ConversionHandler(string password)
    {
      return (messages, appData) =>
      {
        var responses = new List<PamResponse>();
        foreach (var message in messages)
        {
          switch (message.MsgStyle)
          {
            case MessageStyle.PAM_PROMPT_ECHO_ON:
              Logger.LogInformation("PAM_PROMPT_ECHO_ON: {msg}", message.Message);
              responses.Add(new PamResponse("\n"));
              break;
            case MessageStyle.PAM_PROMPT_ECHO_OFF:
              Logger.LogInformation("PAM_PROMPT_ECHO_OFF: {msg}", message.Message);
              responses.Add(new PamResponse(password));
              break;
            case MessageStyle.PAM_ERROR_MSG:
            case MessageStyle.PAM_TEXT_INFO:
            case MessageStyle.PAM_RADIO_TYPE:
            case MessageStyle.PAM_BINARY_PROMPT:
            default:
              Logger.LogWarning("Unsupported message style: '{PAM_MSG_STYLE}', Message: '{msg}'",
                message.MsgStyle.ToString(),
                message.Message);
              break;
          }
        }
        return responses;
      };
    }

  }
}
