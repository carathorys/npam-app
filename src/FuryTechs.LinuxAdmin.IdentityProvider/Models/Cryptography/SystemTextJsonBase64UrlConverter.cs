using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Models.Cryptography
{
  public sealed class SystemTextJsonBase64UrlConverter : JsonConverter<byte[]>
  {
    // From RFC7515
    private static string Base64UrlEncode(byte[] arg)
    {
      string s = Convert.ToBase64String(arg); // Regular base64 encoder
      s = s.Split('=')[0]; // Remove any trailing '='s
      s = s.Replace('+', '-'); // 62nd char of encoding
      s = s.Replace('/', '_'); // 63rd char of encoding
      return s;
    }

    // From RFC7515
    private static byte[] Base64UrlDecode(string arg)
    {
      string s = arg;
      s = s.Replace('-', '+'); // 62nd char of encoding
      s = s.Replace('_', '/'); // 63rd char of encoding
      switch (s.Length % 4) // Pad with trailing '='s
      {
        case 0: break; // No pad chars in this case
        case 2: s += "=="; break; // Two pad chars
        case 3: s += "="; break; // One pad char
        default:
          throw new InvalidDataException("Illegal base64url string.");
      }
      return Convert.FromBase64String(s); // Standard base64 decoder
    }

    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      var encodedValue = (string)reader.GetString();
      return Base64UrlDecode(encodedValue);
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
      var encodedValue = Base64UrlEncode(value);
      writer.WriteStringValue(encodedValue);
    }
  }
}
