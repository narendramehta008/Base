using CoreLibrary.API.Domain.Enums;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Models.Authorization;

[ExcludeFromCodeCoverage]
public class IdpConfigOptions
{
    public string ApiKey { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool IsAuthEnabled { get; set; } = true;
    public Authentications Authentication { get; set; } = Authentications.Bearer;
    public ApiRequest? ApiRequest { get; set; }
    /// <summary>
    /// As we know token response wouldn't be same for all the the services, here we can customize it
    /// </summary>
    public Func<JToken, IdpAuthResponse?>? TokenDeserializer { get; set; }

    public string UniqueId =>
        Url + ClientId;
}
