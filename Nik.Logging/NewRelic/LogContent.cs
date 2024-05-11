namespace Nik.Logging.NewRelic;

internal class LogContent
{
    [Newtonsoft.Json.JsonProperty(propertyName: "message")]
    public string Message { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "Level")]
    public int Level { get; internal set; } = 0;

    [Newtonsoft.Json.JsonProperty(propertyName: "fb.input")]
    public string? Project { get; internal set; }

    [Newtonsoft.Json.JsonProperty(propertyName: "ClassName")]
    public string? ClassName { get; internal set; }

    [Newtonsoft.Json.JsonProperty(propertyName: "Version")]
    public string? Version { get; internal set; }

    [Newtonsoft.Json.JsonProperty(propertyName: "UserId")]
    public string UserId { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "EventID")]
    public string EventID { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "hostname")]
    public string HostName { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "logtype")]
    public string LogType { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "TimeCreated")]
    public string TimeCreated { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "newrelic.source")]
    public string Source { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "ErrorDetails")]
    public string? ErrorDetails { get; internal set; }

    [Newtonsoft.Json.JsonProperty(propertyName: "Channel")]
    public string Channel { get; internal set; } = string.Empty;

    [Newtonsoft.Json.JsonProperty(propertyName: "Environment")]
    public string Environment { get; internal set; } = string.Empty;
}
