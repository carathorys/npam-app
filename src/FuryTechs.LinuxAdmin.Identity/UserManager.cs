using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npam;
using Npam.Interop;

namespace FuryTechs.LinuxAdmin.Identity
{
  public class UserManager : AspNetUserManager<User>
  {
    public UserManager(IUserStore<User> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<User>> logger) : base(
        store,
        optionsAccessor,
        passwordHasher,
        userValidators,
        passwordValidators,
        keyNormalizer,
        errors,
        services,
        logger)
    {
    }

    public override async Task<bool> CheckPasswordAsync(User user, string password)
    {
      await Task.CompletedTask;
      try
      {
        Logger.LogInformation("Check user's credentials: {user}", user.UserName);
        using var session = new NpamSession("login", user.UserName.ToLower(), ConversionHandler(password), IntPtr.Zero);
        var retval = session.Start();

        if (retval != PamStatus.PAM_SUCCESS) return false;
        retval = session.Authenticate(0);

        if (retval != PamStatus.PAM_SUCCESS) return false;
        retval = session.AccountManagement(0);

        return retval == PamStatus.PAM_SUCCESS;
      }
      catch
      {
        return false;
      }
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

    // private IEnumerable<PamResponse> ConvHandler(IEnumerable<PamMessage> messages, IntPtr appData)
    // {
    //     
    //     foreach (var message in messages)
    //     {
    //         var response = "";
    //
    //         switch (message.MsgStyle)
    //         {
    //             case MessageStyle.PAM_PROMPT_ECHO_ON:
    //                 Console.Write(message.Message);
    //                 response = "\n";
    //                 break;
    //             case MessageStyle.PAM_PROMPT_ECHO_OFF:
    //                 Console.Write(message.Message);
    //                 response = LoginModel.Password;
    //                 break;
    //             case MessageStyle.PAM_ERROR_MSG:
    //                 break;
    //             case MessageStyle.PAM_TEXT_INFO:
    //                 break;
    //             case MessageStyle.PAM_RADIO_TYPE:
    //                 break;
    //             case MessageStyle.PAM_BINARY_PROMPT:
    //                 break;
    //             default:
    //                 Console.WriteLine(message.Message);
    //                 Console.WriteLine("Unsupported PAM message style \"{0}\".", message.MsgStyle);
    //                 break;
    //         }
    //
    //         yield return new PamResponse(response);
    //     }
    // }
  }
}
