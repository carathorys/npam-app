using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
  using System;
  using System.Collections.Generic;
  using Npam;
  using Npam.Interop;

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

      using var session = new NpamSession("sudo", login.UserName, ConvHandler, IntPtr.Zero);
      var retval = session.Start();
      if (retval == PamStatus.PAM_SUCCESS)
      {
        retval = session.Authenticate(0);
        if (retval == PamStatus.PAM_SUCCESS)
        {
          retval = session.AccountManagement(0);
          if (retval == PamStatus.PAM_SUCCESS)
          {
            return Ok(new { Success = true });
          }
          return BadRequest(new { Success = false, Message = "Access failure" });
        }
        return BadRequest(new { Success = false, Message = "Authentication failure" });
      }
      return BadRequest(new { Success = false, Message = "Start failure" });
    }

    [HttpGet]
    public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string passwd)
    {
      await Task.CompletedTask;
      LoginModel = new LoginModel
      {
        UserName = username,
        Password = passwd
      };

      using var session = new NpamSession("passwd", LoginModel.UserName, ConvHandler, IntPtr.Zero);
      var retval = session.Start();
      if (retval == PamStatus.PAM_SUCCESS)
      {
        retval = session.Authenticate(0);
        if (retval == PamStatus.PAM_SUCCESS)
        {
          retval = session.AccountManagement(0);
          if (retval == PamStatus.PAM_SUCCESS)
          {
            return Ok(new { Success = true });
          }
          return BadRequest(new { Success = false, Message = "Access failure" });
        }
        return BadRequest(new { Success = false, Message = "Authentication failure" });
      }
      return BadRequest(new { Success = false, Message = "Start failure" });
    }

    private IEnumerable<PamResponse> ConvHandler(IEnumerable<PamMessage> messages, IntPtr appData)
    {
      foreach (PamMessage message in messages)
      {
        string response = "";

        switch (message.MsgStyle)
        {
          case MessageStyle.PAM_PROMPT_ECHO_ON:
            Console.Write(message.Message);
            response = "\n";
            break;
          case MessageStyle.PAM_PROMPT_ECHO_OFF:
            Console.Write(message.Message);
            response = LoginModel.Password;
            break;
          case MessageStyle.PAM_ERROR_MSG:
            break;
          default:
            Console.WriteLine(message.Message);
            Console.WriteLine("Unsupported PAM message style \"{0}\".", message.MsgStyle);
            break;
        }

        yield return new PamResponse(response);
      }
    }

    private string AcceptInputNoEcho()
    {
      string password = "";
      while (true)
      {
        ConsoleKeyInfo i = Console.ReadKey(true);
        if (i.Key == ConsoleKey.Enter)
        {
          Console.WriteLine();
          break;
        }
        else if (i.Key == ConsoleKey.Backspace)
        {
          if (password.Length > 0)
          {
            password = password.Substring(0, password.Length - 1);
          }
        }
        else
        {
          password += (i.KeyChar);
        }
      }
      return password;
    }
  }
}
