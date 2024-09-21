using Newtonsoft.Json.Linq;

namespace CoreLibrary.API.Domain.Extensions;

public static class TokenExtension
{
    public static T? GetValue<T>(this JToken? token, params string[] paths)
    {
        for (int index = 0; index < paths.Length; index++)
        {
            string? current = paths[index];
            if (token == null || !token.HasValues) return default;
            token = token[key: current];
        }
        return token is null ? default : token.ToObject<T>();
    }

    public static T? GetValue<T>(this JToken? token, string path)
    {
        if (token == null) return default;
        token = token[path];
        return token is null ? default : token.ToObject<T>();
    }

    public static T? SelectTokenValue<T>(this JToken token, string path)
    {
        var jToken = token.SelectToken(path, false);
        return jToken is null ? default : jToken.ToObject<T>();
    }
}
