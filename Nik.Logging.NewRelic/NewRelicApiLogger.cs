#nullable disable

namespace Nik.Logging.NewRelic;

public sealed class NewRelicApiLogger(
        IJsonSerializer jsonSerializer,
        NewRelicOptions newRelicConfig,
        string categoryName,
        Func<NewRelicOptions> getCurrentConfig) : ILogger
{
    private LogContent GenerateLogContent(string message, LogLevel level, string eventID, Exception exception) => new()
    {
        Message = message,
        Level = (int)level,
        Project = Assembly.GetEntryAssembly()?.GetName()?.Name,
        Version = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString(),
        UserId = Environment.UserName,
        HostName = Environment.MachineName,
        TimeCreated = DateTime.Now.ToString("O"),
        Source = "api.logs",
        ClassName = categoryName,
        EventID = eventID,
        ErrorDetails = exception?.ToString(),
        Environment = Context.Environment,
        LogType = newRelicConfig.LogType,
        Channel = newRelicConfig.Channel,
    };

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        Task.Run(() => LogNonBlocking(logLevel, eventId, state, exception, formatter)).ConfigureAwait(false);
    }

    private void LogNonBlocking<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        try
        {
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Api-Key", newRelicConfig.NewRelicLicenseKey);
            var content = GenerateLogContent(formatter(state, exception), logLevel, eventId.ToString(), exception);
            var json = jsonSerializer.Serialize(content, false);
            httpClient.PostAsync("https://log-api.eu.newrelic.com/log/v1", new StringContent(json, Encoding.UTF8, "application/json"));
        }
        catch (Exception)
        {
        }
    }

    public bool IsEnabled(LogLevel logLevel) => getCurrentConfig().ActiveLogLevels.Contains(logLevel);

    public IDisposable BeginScope<TState>(TState state) => default!;
}