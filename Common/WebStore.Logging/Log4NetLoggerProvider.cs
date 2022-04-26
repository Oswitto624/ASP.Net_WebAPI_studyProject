using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Xml;

namespace WebStore.Logging;

public class Log4NetLoggerProvider : ILoggerProvider
{
    private readonly string _СonfigurationFile;
    private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new();

    public Log4NetLoggerProvider(string ConfigurationFile) => _СonfigurationFile = ConfigurationFile;

    public ILogger CreateLogger(string Category) =>
        _Loggers.GetOrAdd(Category, static (category, configuration_file) =>
        {
            var xml = new XmlDocument();
            xml.Load(configuration_file);
            return new Log4NetLogger(category, xml["log4net"]!);
        }, _СonfigurationFile);

    public void Dispose() => _Loggers.Clear();
}
