using System.ComponentModel.DataAnnotations;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Models {
  public class LoginModel {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberLogin { get; set; }

    public string ReturnUrl { get; set; }
  }
}
