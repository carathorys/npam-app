using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FuryTechs.LinuxAdmin.IdentityProvider.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Models.Cryptography
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
    }
  }
}
