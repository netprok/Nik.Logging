#nullable disable

namespace Nik.Logging.NewRelic;

public sealed class NewRelicApiLogger(
        IJsonSerializer jsonSerializer,
        NewRelicOptions newRelicConfig,
        string categoryName,
        IEnvironmentHelper environmentHelper,
        Func<NewRelicOptions> getCurrentConfig) : ILogger
{
    private LogContent GenerateLogContent(string message, LogLevel level, string eventID, Exception exception)
    {
        var environmentName = environmentHelper.GetEnvironmentName();

        return new()
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
            Environment = environmentName,
            LogType = newRelicConfig.LogType,
            Channel = newRelicConfig.Channel,
        };
    }

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
            HttpClient httpClient = new()
            {
                BaseAddress = new Uri("https://log-api.eu.newrelic.com"),
            };

            httpClient.DefaultRequestHeaders.Add("Api-Key", newRelicConfig.NewRelicLicenseKey);
            var content = GenerateLogContent(formatter(state, exception), logLevel, eventId.ToString(), exception);
            var json = jsonSerializer.Serialize(content, false);
            var response = httpClient.PostAsync("/log/v1", new StringContent(json, Encoding.UTF8, "application/json")).Result;
            response.Content.ReadAsStringAsync();
        }
        catch (Exception)
        {
        }
        finally
        {
        }
    }

    public bool IsEnabled(LogLevel logLevel) => getCurrentConfig().ActiveLogLevels.Contains(logLevel);

    public IDisposable BeginScope<TState>(TState state) => default!;
}