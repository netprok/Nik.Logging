namespace Nik.Logging;

public static class ServicesExtensions
{
    private const string DefaultLoggingLevel = "Information";

    public static IServiceCollection AddNikLogging(this IServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            configure.ClearProviders();

            var minimumLevel = Context.Configuration.GetValue("Logging:LogLevel:Default", DefaultLoggingLevel) ?? DefaultLoggingLevel;
            configure.SetMinimumLevel(Enum.Parse<LogLevel>(minimumLevel));

            var providers = Context.Configuration.GetSection("Logging:Providers").Get<List<string>>();
            if (providers?.Count > 0)
            {
                foreach (var provider in providers)
                {
                    configure.AddProvider(services, provider);
                }
            }
        });

        return services;
    }
}