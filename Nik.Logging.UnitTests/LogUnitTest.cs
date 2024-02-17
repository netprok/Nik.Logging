using System.Threading;

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
        var logger = host.Services.GetService(typeof(ILogger<LogUnitTest>)) as ILogger<LogUnitTest>;
        logger.Should().NotBeNull();
        logger.LogError("TEST");
        Thread.Sleep(10000);
    }
}