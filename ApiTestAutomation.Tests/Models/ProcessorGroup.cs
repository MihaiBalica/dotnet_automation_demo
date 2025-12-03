using Newtonsoft.Json;

namespace ApiTestAutomation.Tests.Models;

/// <summary>
/// Model representing a NiFi ProcessorGroup
/// </summary>
public class ProcessorGroup
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("uri")]
    public string Uri { get; set; } = string.Empty;

    [JsonProperty("parentGroupId")]
    public string ParentGroupId { get; set; } = string.Empty;

    [JsonProperty("position")]
    public Position? Position { get; set; }

    [JsonProperty("component")]
    public ProcessorGroupComponent? Component { get; set; }

    [JsonProperty("status")]
    public ProcessorGroupStatus? Status { get; set; }
}

public class ProcessorGroupComponent
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("comments")]
    public string Comments { get; set; } = string.Empty;

    [JsonProperty("runningCount")]
    public int RunningCount { get; set; }

    [JsonProperty("stoppedCount")]
    public int StoppedCount { get; set; }

    [JsonProperty("invalidCount")]
    public int InvalidCount { get; set; }

    [JsonProperty("disabledCount")]
    public int DisabledCount { get; set; }
}

public class ProcessorGroupStatus
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("statsLastRefreshed")]
    public string StatsLastRefreshed { get; set; } = string.Empty;

    [JsonProperty("aggregateSnapshot")]
    public AggregateSnapshot? AggregateSnapshot { get; set; }
}

public class AggregateSnapshot
{
    [JsonProperty("bytesIn")]
    public long BytesIn { get; set; }

    [JsonProperty("bytesOut")]
    public long BytesOut { get; set; }

    [JsonProperty("flowFilesIn")]
    public int FlowFilesIn { get; set; }

    [JsonProperty("flowFilesOut")]
    public int FlowFilesOut { get; set; }

    [JsonProperty("flowFilesQueued")]
    public int FlowFilesQueued { get; set; }

    [JsonProperty("bytesQueued")]
    public long BytesQueued { get; set; }
}

public class Position
{
    [JsonProperty("x")]
    public double X { get; set; }

    [JsonProperty("y")]
    public double Y { get; set; }
}
