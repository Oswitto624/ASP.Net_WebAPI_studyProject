using Microsoft.Extensions.Logging;

namespace WebStore.Logging;

public static class Log4NetLoggerFactoryExtensions
{
    public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string ConfigurationFile = "log4net.config")
    {


        return builder;
    }
}

public class Log4NetLoggerProvider
{

}

public class Log4NetLogger
{

}
