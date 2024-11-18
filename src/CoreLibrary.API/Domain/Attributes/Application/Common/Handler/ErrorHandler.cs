namespace CoreLibrary.API.Application.Common.Handler;

public static class ErrorHandler
{
    public static T? Handle<T>(this Func<T> func, ILogger logger, Action<Exception>? action = null)
    {
        try
        {
            return func();
        }
        catch (Exception ex)
        {
            action?.Invoke(ex);
            logger.LogWarning("Error: {ex}", ex);
            return default;
        }
    }

    public static void Handle(this Action action, ILogger logger, Action<Exception>? act = null)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            act?.Invoke(ex);
            logger.LogWarning("Error: {ex}", ex);
        }
    }
}
