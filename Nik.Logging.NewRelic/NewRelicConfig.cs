namespace Nik.Logging.NewRelic;

public sealed class NewRelicConfig
{
    public string NewRelicLicenseKey { get; set; } = string.Empty;
    public List<LogLevel> ActiveLogLevels { get; set; } = new();
}
