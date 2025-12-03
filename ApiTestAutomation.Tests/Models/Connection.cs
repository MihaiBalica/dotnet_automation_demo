using Newtonsoft.Json;

namespace ApiTestAutomation.Tests.Models;

/// <summary>
/// Model representing a NiFi Connection (Queue)
/// </summary>
public class Connection
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("uri")]
    public string Uri { get; set; } = string.Empty;

    [JsonProperty("parentGroupId")]
    public string ParentGroupId { get; set; } = string.Empty;

    [JsonProperty("component")]
    public ConnectionComponent? Component { get; set; }

    [JsonProperty("status")]
    public ConnectionStatus? Status { get; set; }
}

public class ConnectionComponent
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("source")]
    public ConnectableComponent? Source { get; set; }

    [JsonProperty("destination")]
    public ConnectableComponent? Destination { get; set; }

    [JsonProperty("selectedRelationships")]
    public List<string>? SelectedRelationships { get; set; }

    [JsonProperty("backPressureDataSizeThreshold")]
    public string BackPressureDataSizeThreshold { get; set; } = string.Empty;

    [JsonProperty("backPressureObjectThreshold")]
    public long BackPressureObjectThreshold { get; set; }
}

public class ConnectableComponent
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("groupId")]
    public string GroupId { get; set; } = string.Empty;
}

public class ConnectionStatus
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("aggregateSnapshot")]
    public ConnectionSnapshot? AggregateSnapshot { get; set; }
}

public class ConnectionSnapshot
{
    [JsonProperty("flowFilesIn")]
    public int FlowFilesIn { get; set; }

    [JsonProperty("flowFilesOut")]
    public int FlowFilesOut { get; set; }

    [JsonProperty("flowFilesQueued")]
    public int FlowFilesQueued { get; set; }

    [JsonProperty("bytesIn")]
    public long BytesIn { get; set; }

    [JsonProperty("bytesOut")]
    public long BytesOut { get; set; }

    [JsonProperty("bytesQueued")]
    public long BytesQueued { get; set; }

    [JsonProperty("queuedSize")]
    public string QueuedSize { get; set; } = string.Empty;

    [JsonProperty("queuedCount")]
    public string QueuedCount { get; set; } = string.Empty;
}
