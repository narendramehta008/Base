using CoreLibrary.API.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Constants;

[ExcludeFromCodeCoverage]
public static class GlobalConstants
{
    public static RetryModel RetryModel { get; set; } = new();
    public const int INITIALDELAY = 1;
    public const string INVALIDURL = "Invalid url";
    public const int HEALTHCHECKINITIALDELAY = 5;
    public const int CYCLETHRESHOLDLIMIT = 18;
    public const int CYCLEINITIALDELAY = 30;
    public const int CYCLERETRYINCREMENT = 3;
    public const int RETRYCYCLES = 6;
    public const int THRESHOLDLIMIT = 3;
    public const string MEDIATYPEJSON = "application/json";
    public const string MEDIATYPEMULTIFORM = "multipart/form-data";
    public const string DEFAULTAPIVERSION = "1.0";
    public const int RETRYCOUNT = 1;
    public const string RETRYAPIMODELINITIAILZE = "Please initialize RetryApiModel _apiModel";
    public const string HEALTHCHECKPATH = "health/live";
    public const string UNABLETOPROCESSREQUEST = "Unable to process request.";

    public const string ApiKeyHeaderName = "x-api-key";
    public const string AuthHeaderName = "Authorization";
    public const string ApiKeyName = "ApiKey";
    public const string Accept = "Accept";
    public const string BASICAUTH = "Basic";

    public const string Admin = "Admin";
    public const string User = "User";
    public const string TemporaryUser = "TemporaryUser";
    public const string DefaultCors = "DefaultCors";
    public static string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    public static string SeedDataPath = BasePath + @"./Data/{0}.json";
}
