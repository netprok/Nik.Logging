namespace Nik.Logging.NewRelic;

public static class ServicesExtensions
{
    public static ILoggingBuilder AddNewRelic(this ILoggingBuilder loggingBuilder, IServiceCollection services)
    {
        var newRelicKey = Context.Configuration.GetValue(typeof(string), "Logging:NewRelic:NewRelicLicenseKey") as string;

        if (!string.IsNullOrWhiteSpace(newRelicKey))
        {
            var newRelicLevels = Context.Configuration.GetSection("Logging:NewRelic:ActiveLogLevels").Get<List<string>>();
            var channel = Context.Configuration.GetValue(typeof(string), "Logging:NewRelic:Channel") as string;
            var logType = Context.Configuration.GetValue(typeof(string), "Logging:NewRelic:LogType") as string;

            var levels = newRelicLevels?.Count > 0 ?
                newRelicLevels.Select(level => Enum.Parse<LogLevel>(level)) :
                [LogLevel.Information, LogLevel.Critical, LogLevel.Warning, LogLevel.Error];

            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NewRelicLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<NewRelicOptions, NewRelicLoggerProvider>(services);

            services.Configure<NewRelicOptions>(configuration =>
            {
                configuration.ActiveLogLevels.AddRange(levels);
                configuration.NewRelicLicenseKey = newRelicKey;
                configuration.Channel = channel ?? "Nik";
                configuration.LogType = logType ?? "app";
            });
        }

        return loggingBuilder;
    }
}