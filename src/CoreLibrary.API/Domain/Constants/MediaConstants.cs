using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Constants;

[ExcludeFromCodeCoverage]
public class MediaConstants
{
    public static Dictionary<FileType, IEnumerable<string>> SupportedFormats = new Dictionary<FileType, IEnumerable<string>>()
        {
            {FileType.Image, new List<string> { ".jpg", ".jpeg", ".png" } },
            {FileType.Video, new List<string> { ".mp4", ".mpeg"} },
            {FileType.File, MimeTypeMap.GetAllExtension() },
        };
}

public enum FileType
{
    Image,
    Video,
    File
}