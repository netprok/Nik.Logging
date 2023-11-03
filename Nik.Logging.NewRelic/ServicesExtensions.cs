﻿namespace Nik.Logging.NewRelic;

public static class ServicesExtensions
{
    private const string DefaultLoggingLevel = "Information";

    public static IServiceCollection UseLogging(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddLogging(configure =>
        {
            var minimumLevel = configuration.GetValue("Logging:LogLevel:Default", DefaultLoggingLevel) ?? DefaultLoggingLevel;
            configure.SetMinimumLevel(Enum.Parse<LogLevel>(minimumLevel));
        });

        return services;
    }

    public static IServiceCollection UseNewRelicLogging(this IServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("_config/newrelic.json")
              .Build();

            var newRelicLevels = configuration.GetSection("ActiveLogLevels").Get<List<string>>();
            var newRelicKey = configuration.GetValue(typeof(string), "NewRelicLicenseKey") as string;
            if (!string.IsNullOrWhiteSpace(newRelicKey))
            {
                IEnumerable<LogLevel> levels = new LogLevel[] { };

                if (newRelicLevels == default)
                {
                    levels = new LogLevel[] { LogLevel.Information, LogLevel.Critical, LogLevel.Warning, LogLevel.Error };
                }
                else
                {
                    levels = newRelicLevels.Select(level => Enum.Parse<LogLevel>(level));
                }

                services.AddNewRelicLogger(configuration =>
                {
                    configuration.ActiveLogLevels.AddRange(levels);
                    configuration.NewRelicLicenseKey = newRelicKey;
                });
            }
        });

        return services;
    }
}