namespace CoreLibrary.API.Domain.Extensions;

public static class StringExtensions
{
    public static string BaseUrl(this Uri uri) =>
        $"{uri.Scheme}://{uri.Host}/";
    public static string UrlAppendPath(this Uri uri, string path) =>
    $"{BaseUrl(uri)}{path.Trim('/')}";
}
