using Microsoft.AspNetCore.Mvc;

namespace CoreLibrary.API.Services;

public interface IFileServices
{
    Task<FileResult> DownloadFile(string url, string fileName);
}

public class FileServices : IFileServices
{
    private HttpClient _httpClient;
    public FileServices(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    public async Task<FileResult> DownloadFile(string url, string fileName)
    {
        if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("http"))
            throw new Exception("Invalid Url");
        string ext;
        if (url.LastIndexOf('?') > 0)
        {
            var trimmedToken = url.Substring(0, url.LastIndexOf('?'));
            ext = trimmedToken.Substring(trimmedToken.LastIndexOf('.') + 1);
            fileName ??= $"{trimmedToken.Substring(trimmedToken.LastIndexOf('/') + 1)}";
        }
        else
        {
            ext = url.Substring(url.LastIndexOf('.') + 1);
        }
        fileName = string.IsNullOrWhiteSpace(fileName) ? url.Substring(url.LastIndexOf('/') + 1) : $"{fileName}.{ext}";

        var response = await _httpClient.GetAsync(new Uri(url));
        var result = new FileContentResult(await response.Content.ReadAsByteArrayAsync(), "application/xlsx")
        {
            FileDownloadName = $"{fileName}"
        };

        return result;
    }
}
