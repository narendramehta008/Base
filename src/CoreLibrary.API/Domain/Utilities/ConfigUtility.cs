using System.Diagnostics.CodeAnalysis;

namespace CoreLibrary.API.Domain.Utilities;

public static class ConfigUtility
{

    [ExcludeFromCodeCoverage]
    private static string RetrieveEnvironmentVariable()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
    }

    public static IConfigurationBuilder SetConfigurationBuilder()
    {
        var envName = RetrieveEnvironmentVariable();
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings." + envName + ".json", optional: true, reloadOnChange: true);
        configurationBuilder.AddEnvironmentVariables();
        return configurationBuilder;
    }

    [ExcludeFromCodeCoverage]
    public static bool ParseSsl(string ssl)
    {
        if (!string.IsNullOrEmpty(ssl) && ssl.ToLower() != "null")
        {
            return bool.Parse(ssl);
        }

        return false;
    }
}
