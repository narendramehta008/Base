using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Models.Authorization;

[ExcludeFromCodeCoverage]
public class IdpAuthResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = null!;

    [JsonProperty("id_token")]
    public string IdToken { get; set; } = null!;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = null!;

    [JsonProperty("expires_in")]
    public double ExpiresIn { get; set; }
}