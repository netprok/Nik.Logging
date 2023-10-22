#nullable disable

namespace Nik.Logging.NewRelic;

public sealed class NewRelicApiLogger : ILogger
{
    private readonly string name;
    private readonly IEnvironmentHelper environmentHelper;
    private readonly Func<NewRelicConfig> getCurrentConfig;
    private readonly IJsonSerializer jsonSerializer;
    private readonly NewRelicConfig newRelicConfig;

    public NewRelicApiLogger(
        IJsonSerializer jsonSerializer,
        NewRelicConfig newRelicConfig,
        string name,
        IEnvironmentHelper environmentHelper,
        Func<NewRelicConfig> getCurrentConfig)
    {
        this.jsonSerializer = jsonSerializer;
        this.newRelicConfig = newRelicConfig;
        this.name = name;
        this.environmentHelper = environmentHelper;
        this.getCurrentConfig = getCurrentConfig;
    }

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
            LogType = "windows_application",
            TimeCreated = DateTime.Now.ToString("s"),
            Source = "api.logs",
            ClassName = name,
            EventID = eventID,
            ErrorDetails = exception?.ToString(),
            Environment = environmentName,
            Channel = "Worker"
        };
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

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
            response.Content.ReadAsStringAsync().Wait();
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