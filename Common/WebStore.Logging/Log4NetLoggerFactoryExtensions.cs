using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WebStore.Logging;

public static class Log4NetLoggerFactoryExtensions
{
    private static string CheckFilePath(string FilePath)
    {
        if(FilePath is not { Length: > 0 })
            throw new ArgumentException("Не указан путь к файлу конфигурации", nameof(FilePath));

        if(Path.IsPathRooted(FilePath))
            return FilePath;

        var assemly = Assembly.GetEntryAssembly();
        var dir = Path.GetDirectoryName(assemly!.Location)!;
        return Path.Combine(dir, FilePath);
    }

    public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string ConfigurationFile = "log4net.config")
    {
        builder.AddProvider(new Log4NetLoggerProvider(CheckFilePath(ConfigurationFile)));

        return builder;
    }
}
