using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Extensions;

[ExcludeFromCodeCoverage]
public static class DataExtensions
{
    public static string Serialize<T>(this T model, params JsonConverter[] converters)
    {
        return JsonConvert.SerializeObject(model, converters);
    }
    public static string SerializedCamelCaseIdented<T>(this T model, params JsonConverter[] converters)
    {
        var setting = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },

            Formatting = Newtonsoft.Json.Formatting.Indented
        };
        if (converters != null && converters.Length > 0) setting.Converters = converters;
        return JsonConvert.SerializeObject(model, setting);
    }

    public static T? Deserialize<T>(this string value, params JsonConverter[] converters)
    {
        return JsonConvert.DeserializeObject<T>(value, converters);
    }

    public static bool TryDeserialize<T>(this string value, [MaybeNullWhen(false)] out T data)
    {
        data = Deserialize<T>(value)!;
        return data != null;
    }

    public static T? GetValue<T>(this string json, string path)
    {
        if (json.Parse(out JToken? jToken))
            return jToken.GetValue<T>(path);
        return default;
    }

    public static bool Parse(this string json, [MaybeNullWhen(false)] out JToken jToken)
    {
        try
        {
            jToken = JObject.Parse(json);
            return true;
        }
        catch (Exception ex)
        {
            jToken = default;
            Log.Warning("Parsing error {ex}", ex);
        }
        return false;
    }
}
