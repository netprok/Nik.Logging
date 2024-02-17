namespace Nik.Logging;

public static class ServicesExtensions
{
    private const string DefaultLoggingLevel = "Information";

    public static IServiceCollection AddNikLogging(this IServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            configure.ClearProviders();
            configure.AddConsole();
            configure.AddNewRelic(services);

            var minimumLevel = Context.Configuration.GetValue("Logging:LogLevel:Default", DefaultLoggingLevel) ?? DefaultLoggingLevel;
            configure.SetMinimumLevel(Enum.Parse<LogLevel>(minimumLevel));
        });

        return services;
    }
}