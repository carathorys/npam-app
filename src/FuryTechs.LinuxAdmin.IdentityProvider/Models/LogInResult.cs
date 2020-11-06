using System;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Models
{
  public class LoginResult
  {
    public bool Succeeded { get; set; }
    public string ErrorMessage { get; set; }
  }
}
