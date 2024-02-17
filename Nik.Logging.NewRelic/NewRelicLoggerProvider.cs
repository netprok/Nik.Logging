﻿namespace Nik.Logging.NewRelic;

[ProviderAlias("NewRelic")]
public sealed class NewRelicLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? onChangeToken;
    private NewRelicOptions currentOptions;
    private readonly ConcurrentDictionary<string, NewRelicApiLogger> loggers = new(StringComparer.OrdinalIgnoreCase);
    private readonly IEnvironmentHelper environmentHelper;
    private readonly IJsonSerializer jsonSerializer;

    public NewRelicLoggerProvider(
        IOptionsMonitor<NewRelicOptions> config,
        IEnvironmentHelper environmentHelper,
        IJsonSerializer jsonSerializer)
    {
        currentOptions = config.CurrentValue;
        onChangeToken = config.OnChange(updatedOptions => currentOptions = updatedOptions);
        this.environmentHelper = environmentHelper;
        this.jsonSerializer = jsonSerializer;
    }

    public ILogger CreateLogger(string categoryName) =>
        loggers.GetOrAdd(categoryName, categoryName => new NewRelicApiLogger(jsonSerializer, currentOptions, categoryName, environmentHelper, GetCurrentConfig));

    private NewRelicOptions GetCurrentConfig() => currentOptions;

    public void Dispose()
    {
        loggers.Clear();
        onChangeToken?.Dispose();
    }
}