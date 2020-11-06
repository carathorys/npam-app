using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Models.Cryptography
{

  public sealed class RsaJwk
  {
    // See RFC7517
    [System.Text.Json.Serialization.JsonPropertyName("kty")]
    [Newtonsoft.Json.JsonProperty("kty")]
    public string KeyType { get; set; }

    // See RFC7517
    [System.Text.Json.Serialization.JsonPropertyName("alg")]
    [Newtonsoft.Json.JsonProperty("alg")]
    public string Algorithm { get; set; }

    // See RFC7518, 6.3
    [System.Text.Json.Serialization.JsonPropertyName("n"), System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonBase64UrlConverter))]
    [Newtonsoft.Json.JsonProperty("n"), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] Modulus { get; set; }

    // See RFC7518, 6.3
    [JsonPropertyName("e"), System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonBase64UrlConverter))]
    [Newtonsoft.Json.JsonProperty("e"), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] Exponent { get; set; }

    // See RFC7518, 6.3
    [System.Text.Json.Serialization.JsonPropertyName("d"), System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonBase64UrlConverter))]
    [Newtonsoft.Json.JsonProperty("d", NullValueHandling = NullValueHandling.Ignore), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] PrivateExponent { get; set; }

    // See RFC7518, 6.3
    [System.Text.Json.Serialization.JsonPropertyName("p"), System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonBase64UrlConverter))]
    [Newtonsoft.Json.JsonProperty("p", NullValueHandling = NullValueHandling.Ignore), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] FirstPrimeFactor { get; set; }

    // See RFC7518, 6.3
    [System.Text.Json.Serialization.JsonPropertyName("q"), System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonBase64UrlConverter))]
    [Newtonsoft.Json.JsonProperty("q", NullValueHandling = NullValueHandling.Ignore), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] SecondPrimeFactor { get; set; }

    // See RFC7518, 6.3
    [System.Text.Json.Serialization.JsonPropertyName("dp"), System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonBase64UrlConverter))]
    [Newtonsoft.Json.JsonProperty("dp", NullValueHandling = NullValueHandling.Ignore), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] FirstFactorCrtExponent { get; set; }

    // See RFC7518, 6.3
    [System.Text.Json.Serialization.JsonPropertyName("dq"), System.Text.Json.Serialization.JsonConverter(typeof(SystemTextJsonBase64UrlConverter))]
    [Newtonsoft.Json.JsonProperty("dq", NullValueHandling = NullValueHandling.Ignore), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] SecondFactorCrtExponent { get; set; }

    // See RFC7518, 6.3
    [System.Text.Json.Serialization.JsonPropertyName("qi")]
    [Newtonsoft.Json.JsonProperty("qi", NullValueHandling = NullValueHandling.Ignore), Newtonsoft.Json.JsonConverter(typeof(NewtonsoftBase64UrlConverter))]
    public byte[] FirstCrtCoefficient { get; set; }
  }
}
