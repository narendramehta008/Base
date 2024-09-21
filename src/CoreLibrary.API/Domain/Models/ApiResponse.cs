using CoreLibrary.API.Domain.Constants;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace CoreLibrary.API.Domain.Models;

[ExcludeFromCodeCoverage]
public class ApiResponse
{
    public string Request { get; set; } = string.Empty;
    public string Response { get; private set; }
    public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
    public string? MediaType { get; set; } = GlobalConstants.MEDIATYPEJSON;
    public bool IsSuccess => (int)HttpStatusCode >= 200 && (int)HttpStatusCode <= 299;
    public MemoryStream? ResultStream { get; set; }
    public bool OffsetResponseSuccessStatusForRetry { get; set; } = false;
    public Exception? ExceptionObject { get; set; }

    public ApiResponse(HttpStatusCode httpStatusCode, Stream stream, string? mediaType = GlobalConstants.MEDIATYPEJSON)
    {
        Response = string.Empty;
        HttpStatusCode = httpStatusCode;
        ResultStream = new MemoryStream();
        stream.CopyTo(ResultStream);
        MediaType = mediaType;
    }
    public ApiResponse(HttpStatusCode httpStatusCode, string request, Stream stream, string? mediaType = GlobalConstants.MEDIATYPEJSON)
    {
        Request = request;
        Response = string.Empty;
        HttpStatusCode = httpStatusCode;
        ResultStream = new MemoryStream();
        stream.CopyTo(ResultStream);
        MediaType = mediaType;
    }

    public ApiResponse(HttpStatusCode httpStatusCode, string response, string? mediaType = GlobalConstants.MEDIATYPEJSON)
    {
        HttpStatusCode = httpStatusCode;
        Response = response;
        MediaType = mediaType;
    }

    public ApiResponse(HttpStatusCode httpStatusCode, string request, string response, string? mediaType = GlobalConstants.MEDIATYPEJSON, Exception? exceptionObject = null)
    {
        HttpStatusCode = httpStatusCode;
        Request = request;
        Response = response;
        MediaType = mediaType;
        ExceptionObject = exceptionObject;
    }

    public ApiResponse(string response)
    {
        Response = response;
    }

    public static ApiResponse BadRequest(string? message = null) =>
        new(HttpStatusCode.BadRequest, message ??= GlobalConstants.UNABLETOPROCESSREQUEST);
}