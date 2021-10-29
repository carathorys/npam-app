using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FuryTechs.LinuxAdmin.IdentityProvider.Extensions;
using FuryTechs.LinuxAdmin.IdentityProvider.Models.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Models
{
  [ModelBinder(BinderType = typeof(EncryptedModelBinder<LoginModel>))]
  public class LoginModel
  {
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }

    public bool RememberLogin { get; set; }

    public string ReturnUrl { get; set; }
  }
}
