namespace Nik.Logging;

public static class ServicesExtensions
{
    private const string DefaultLoggingLevel = "Information";

    public static IServiceCollection AddNikLogging(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            var minimumLevel = configuration.GetValue("Logging:LogLevel:Default", DefaultLoggingLevel) ?? DefaultLoggingLevel;
            configure.SetMinimumLevel(Enum.Parse<LogLevel>(minimumLevel));

            configure.UseNewRelicLogging(services, configuration);
        });

        return services;
    }
}