using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

//using Npam;
//using Npam.Interop;

namespace FuryTechs.LinuxAdmin.Core.Controllers
{

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        public LoginModel LoginModel { get; set; }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            await Task.CompletedTask;
            LoginModel = login;

            if (!this.ModelState.IsValid)
                return BadRequest();

            //using var session = new NpamSession("sudo", login.UserName, ConvHandler, IntPtr.Zero);
            //var retval = session.Start();
            //if (retval == PamStatus.PAM_SUCCESS)
            //{
            //  retval = session.Authenticate(0);
            //  if (retval == PamStatus.PAM_SUCCESS)
            //  {
            //    retval = session.AccountManagement(0);
            //    if (retval == PamStatus.PAM_SUCCESS)
            //    {
            //      return Ok(new { Success = true });
            //    }
            //    return Unauthorized(new { Success = false, Message = "Invalid username, or password!" });
            //  }
            //  return Unauthorized(new { Success = false, Message = "Invalid username, or password!" });
            //}
            return Unauthorized(new { Success = false, Message = "Invalid username, or password!" });
        }

        //private IEnumerable<PamResponse> ConvHandler(IEnumerable<PamMessage> messages, IntPtr appData)
        //{
        //    foreach (PamMessage message in messages)
        //    {
        //        string response = "";

        //        switch (message.MsgStyle)
        //        {
        //            case MessageStyle.PAM_PROMPT_ECHO_ON:
        //                Console.Write(message.Message);
        //                response = "\n";
        //                break;
        //            case MessageStyle.PAM_PROMPT_ECHO_OFF:
        //                Console.Write(message.Message);
        //                response = LoginModel.Password;
        //                break;
        //            case MessageStyle.PAM_ERROR_MSG:
        //                break;
        //            default:
        //                Console.WriteLine(message.Message);
        //                Console.WriteLine("Unsupported PAM message style \"{0}\".", message.MsgStyle);
        //                break;
        //        }

        //        yield return new PamResponse(response);
        //    }
        //}
    }
}
