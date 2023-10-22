namespace Nik.Logging.NewRelic;

public static class NewRelicLoggerExtensions
{
    public static void AddNewRelicLogger(this IServiceCollection Services)
    {
        Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NewRelicLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions<NewRelicConfig, NewRelicLoggerProvider>(Services);
    }

    public static void AddNewRelicLogger(this IServiceCollection Services, Action<NewRelicConfig> configure)
    {
        Services.AddNewRelicLogger();
        Services.Configure(configure);
    }
}