namespace Nik.Logging.NewRelic;

public sealed class NewRelicConfig
{
    public string NewRelicLicenseKey { get; set; } = string.Empty;

    public List<LogLevel> ActiveLogLevels { get; set; } = [];

    public string Channel { get; set; } = string.Empty;

    public string LogType { get; set; } = string.Empty;
}