namespace Nik.Logging;

internal static class LoggingProviderFactory
{
    public static ILoggingBuilder AddProvider(this ILoggingBuilder loggingBuilder, IServiceCollection services, string providerName)
    {
        if (providerName == "NewRelic")
        {
            return loggingBuilder.AddNewRelic(services);
        }

        if (providerName == "Console")
        {
            return loggingBuilder.AddConsole();
        }

        return loggingBuilder;
    }
}