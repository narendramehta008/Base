using System.Diagnostics.CodeAnalysis;
using CoreLibrary.API.Domain.Enums;
using CoreLibrary.API.Domain.Models.Authorization;

namespace CoreLibrary.API.Domain.Models;

[ExcludeFromCodeCoverage]
public class ApiRequest
{
    public HttpMethod Method { get; set; } = HttpMethod.Get;

    /// <summary>
    /// Key is header name and Value is its value
    /// </summary>
    public ICollection<KeyValuePair<string, string>>? Headers { get; set; }

    /// <summary>
    /// Key is Parameter name and Value is its value
    /// </summary>
    public ICollection<KeyValuePair<string, string>>? Parameters { get; set; }

    /// <summary>
    /// Sample Examples
    /// HttpContent content = new StreamContent(file.InputStream);
    /// HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "CompanyNumber", "1993" } });
    ///  using var form = new MultipartFormDataContent();
    ///  using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
    ///  form.Add(fileContent, "Files", Path.GetFileName(filePath));
    ///   form.Add(new StringContent("de.json"), "FileInfo")
    /// </summary>
    public HttpContent? Content { get; set; }

    public HttpRequestMessage? RequestMessage { get; set; }

    /// <summary>
    /// Bearer token config like Dpaas
    /// </summary>
    public IdpConfigOptions? IdpConfig { get; set; }

    /// <summary>
    /// To do health check before making request
    /// </summary>
    public HealthCheck HealthCheck { get; set; } = HealthCheck.None;

    public string? HealthCheckPath { get; set; }

    /// <summary>
    /// If failed what action like to perform in Retry
    /// since mostly API's action is different but timespan is same. therefore, keeping it here.
    /// </summary>
    public Action<ApiResponse, int>? Action { get; set; }

    public Action<ApiResponse, int>? HealthCheckAction { get; set; }

    /// <summary>
    /// To add new result predicate like ApiResponse is Unauthorized, Service not available then it will fall back and not go for next attempt in synchrnous consecutive hits
    /// </summary>
    public Func<ApiResponse, bool>? ResultPredicate { get; set; }

    public Func<ApiResponse, bool>? HealthCheckResultPredicate { get; set; }
    public bool AcceptJsonHeader { get; set; } = true;
    public bool IsContentStream { get; set; }
    public string? Query { get; set; }
    public TimeSpan? Timeout { get; set; }
    public bool OffsetResponseSuccessStatusForRetry { get; set; } = false;
    public HttpClientManager? HttpClientManager { get; set; }
}

public class HttpClientManager(HttpMessageHandler handler, bool disposeHandler = true)
{
    public HttpMessageHandler? HttpMessageHandler { get; set; } = handler;
    public bool DisposeHandler { get; set; } = disposeHandler;
}