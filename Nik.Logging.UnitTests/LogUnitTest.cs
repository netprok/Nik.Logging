namespace Nik.Logging.UnitTests;

public class LogUnitTest
{
    private IHost Prepare()
    {
        return Host.CreateDefaultBuilder()
         .ConfigureServices((services) =>
         {
             services.InitContext()
                .AddNikSerialization()
                .AddNikLogging();
         })
         .Build();
    }

    [Fact]
    public void Test1()
    {
        var host = Prepare();
        var logger = host.Services.GetService<ILogger<LogUnitTest>>();
        logger.Should().NotBeNull();
        logger!.LogError("TEST");
        Thread.Sleep(10000);
    }
}