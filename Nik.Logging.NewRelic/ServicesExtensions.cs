namespace Nik.Logging.NewRelic;

public static class ServicesExtensions
{
    public static ILoggingBuilder AddNewRelic(this ILoggingBuilder loggingBuilder, IServiceCollection services)
    {
        var newRelicLevels = Context.Configuration.GetSection("Logging:NewRelic:ActiveLogLevels").Get<List<string>>();
        var newRelicKey = Context.Configuration.GetValue(typeof(string), "Logging:NewRelic:NewRelicLicenseKey") as string;
        var channel = Context.Configuration.GetValue(typeof(string), "Logging:NewRelic:Channel") as string;
        var logType = Context.Configuration.GetValue(typeof(string), "Logging:NewRelic:LogType") as string;
        if (!string.IsNullOrWhiteSpace(newRelicKey))
        {
            IEnumerable<LogLevel> levels = Array.Empty<LogLevel>();

            if (newRelicLevels == default)
            {
                levels = new LogLevel[] { LogLevel.Information, LogLevel.Critical, LogLevel.Warning, LogLevel.Error };
            }
            else
            {
                levels = newRelicLevels.Select(level => Enum.Parse<LogLevel>(level));
            }

            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, NewRelicLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<NewRelicOptions, NewRelicLoggerProvider>(services);

            services.Configure<NewRelicOptions>(configuration =>
            {
                configuration.ActiveLogLevels.AddRange(levels);
                configuration.NewRelicLicenseKey = newRelicKey;
                configuration.Channel = channel ?? "Nik";
                configuration.LogType = logType ?? "windows_application";
            });
        }

        return loggingBuilder;
    }
}