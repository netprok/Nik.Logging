namespace Nik.Logging.NewRelic;

[ProviderAlias("NewRelic")]
public sealed class NewRelicLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? onChangeToken;
    private NewRelicConfig currentConfig;
    private readonly ConcurrentDictionary<string, NewRelicApiLogger> loggers = new(StringComparer.OrdinalIgnoreCase);
    private readonly IEnvironmentHelper environmentHelper;
    private readonly IJsonSerializer jsonSerializer;

    public NewRelicLoggerProvider(
        IOptionsMonitor<NewRelicConfig> config,
        IEnvironmentHelper environmentHelper,
        IJsonSerializer jsonSerializer)
    {
        currentConfig = config.CurrentValue;
        onChangeToken = config.OnChange(updatedConfig => currentConfig = updatedConfig);
        this.environmentHelper = environmentHelper;
        this.jsonSerializer = jsonSerializer;
    }

    public ILogger CreateLogger(string categoryName) =>
        loggers.GetOrAdd(categoryName, categoryName => new NewRelicApiLogger(jsonSerializer, currentConfig, categoryName, environmentHelper, GetCurrentConfig));

    private NewRelicConfig GetCurrentConfig() => currentConfig;

    public void Dispose()
    {
        loggers.Clear();
        onChangeToken?.Dispose();
    }
}