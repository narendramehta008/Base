using CoreLibrary.API.Domain.Enums;
using CoreLibrary.API.Domain.Extensions;
using CoreLibrary.API.Domain.Models.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;

namespace CoreLibrary.API.Domain.Models.Apis;

[ExcludeFromCodeCoverage]
public class ApiRequestConfig : ApiRequest
{
    public new HttpMethod? Method { get; set; }
    public required string Category { get; set; }
    public required string[] Types { get; set; }
    public string? UrlFormat { get; set; }
    public new Dictionary<string, string>? Headers { get; set; }
    public Dictionary<string, string>? Contents { get; set; }
    public string? Proxy { get; set; }
    public Authentications? AuthType { get; set; }
    public string? AuthKey { get; set; }
    public HttpStatusCode[]? NotResultPredicate { get; set; }

    public string RequestUrl(string url) =>
        string.IsNullOrWhiteSpace(UrlFormat) ? url : string.Format(UrlFormat, url);

    public ApiRequestConfig Initialize()
    {
        base.Headers = Headers ?? [];
        if (!string.IsNullOrWhiteSpace(Proxy))
            HttpClientManager = new Uri(Proxy).WebProxySetting();
        if (NotResultPredicate != null)
            ResultPredicate = (apiResponse) => NotResultPredicate.Any(result => result != apiResponse.HttpStatusCode);
        if (AuthType != null && !string.IsNullOrWhiteSpace(AuthKey))
            base.Headers.AddThese(((Authentications)AuthType).AuthorizationHeader(AuthKey));
        if (Method != null)
            base.Method = Method;
        if (Contents != null)
            Content = new FormUrlEncodedContent(Contents);

        return this;
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        Initialize();
    }
}

public class IdpConfig : IdpConfigOptions
{
    public new ApiRequestConfig? ApiRequest { get; set; }

    public IdpConfig Initialize()
    {
        base.ApiRequest = ApiRequest?.Initialize();
        return this;
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        Initialize();
    }
}