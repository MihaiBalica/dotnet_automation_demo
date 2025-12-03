using Newtonsoft.Json;

namespace ApiTestAutomation.Tests.Models;

/// <summary>
/// Model representing a NiFi Processor
/// </summary>
public class Processor
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
    public ProcessorComponent? Component { get; set; }

    [JsonProperty("status")]
    public ProcessorStatus? Status { get; set; }

    [JsonProperty("revision")]
    public Revision? Revision { get; set; }
}

public class ProcessorComponent
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("state")]
    public string State { get; set; } = string.Empty;

    [JsonProperty("config")]
    public ProcessorConfig? Config { get; set; }
}

public class ProcessorConfig
{
    [JsonProperty("schedulingPeriod")]
    public string SchedulingPeriod { get; set; } = string.Empty;

    [JsonProperty("schedulingStrategy")]
    public string SchedulingStrategy { get; set; } = string.Empty;

    [JsonProperty("concurrentlySchedulableTaskCount")]
    public int ConcurrentlySchedulableTaskCount { get; set; }

    [JsonProperty("properties")]
    public Dictionary<string, string>? Properties { get; set; }
}

public class ProcessorStatus
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("runStatus")]
    public string RunStatus { get; set; } = string.Empty;

    [JsonProperty("statsLastRefreshed")]
    public string StatsLastRefreshed { get; set; } = string.Empty;

    [JsonProperty("aggregateSnapshot")]
    public ProcessorSnapshot? AggregateSnapshot { get; set; }
}

public class ProcessorSnapshot
{
    [JsonProperty("bytesIn")]
    public long BytesIn { get; set; }

    [JsonProperty("bytesOut")]
    public long BytesOut { get; set; }

    [JsonProperty("flowFilesIn")]
    public int FlowFilesIn { get; set; }

    [JsonProperty("flowFilesOut")]
    public int FlowFilesOut { get; set; }

    [JsonProperty("activeThreadCount")]
    public int ActiveThreadCount { get; set; }

    [JsonProperty("taskCount")]
    public int TaskCount { get; set; }
}

public class Revision
{
    [JsonProperty("version")]
    public long Version { get; set; }

    [JsonProperty("clientId")]
    public string ClientId { get; set; } = string.Empty;
}
