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
  public class EncryptedModelBinder<T> : IModelBinder
   where T : class, new()
  {
    private (RSA Rsa, RSAEncryptionPadding Padding) GetEncryptionKey(HttpContext ctx)
    {
      if (String.IsNullOrWhiteSpace(ctx.Session.GetString("Encryption")))
      {
        var rsa = RSA.Create(2048);
        ctx.Session.SetString("Encryption", Newtonsoft.Json.JsonConvert.SerializeObject(rsa.ToJwk(RSAEncryptionPadding.OaepSHA256, true)));
        return (rsa, RSAEncryptionPadding.OaepSHA256);
      }

      var jwk = Newtonsoft.Json.JsonConvert.DeserializeObject<RsaJwk>(ctx.Session.GetString("Encryption"));
      return jwk.ToRSA();
    }

    private T DecryptPayload(HttpContext ctx, string encryptedPayload)
    {
      var key = GetEncryptionKey(ctx);
      try
      {
        var decryptedValue = key.Rsa.Decrypt(Convert.FromBase64String(encryptedPayload), key.Padding);
        var str = System.Text.Encoding.UTF8.GetString(decryptedValue);
        return System.Text.Json.JsonSerializer.Deserialize<T>(str);
      }
      catch (Exception ex)
      {
        return null;
      }
      finally
      {
        key.Rsa.Dispose();
      }
    }
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
      if (bindingContext == null)
      {
        throw new ArgumentNullException(nameof(bindingContext));
      }
      var modelName = bindingContext.ModelName;
      var str = await bindingContext.HttpContext.Request.GetRawBodyStringAsync();
      var model = DecryptPayload(bindingContext.HttpContext, str);
      bindingContext.Result = ModelBindingResult.Success(model);
      return;
      // Try to fetch the value of the argument by name
      // var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

      // if (valueProviderResult == ValueProviderResult.None)
      // {
      //   return;
      // }

      // bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

      // var value = valueProviderResult.FirstValue;

      // // Check if the argument value is null or empty
      // if (string.IsNullOrEmpty(value))
      // {
      //   return;
      // }

      // if (!int.TryParse(value, out var id))
      // {
      //   // Non-integer arguments result in model state errors
      //   bindingContext.ModelState.TryAddModelError(
      //       modelName, "Author Id must be an integer.");

      //   return;
      // }

      // // Model will be null if not found, including for
      // // out of range id values (0, -3, etc.)
      // // bindingContext.Result = ModelBindingResult.Success(model);
      return;
    }
  }

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
