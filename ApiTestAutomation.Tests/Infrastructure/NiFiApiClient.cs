using ApiTestAutomation.Tests.Config;
using ApiTestAutomation.Tests.Helpers;
using ApiTestAutomation.Tests.Models;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace ApiTestAutomation.Tests.Infrastructure;

/// <summary>
/// Apache NiFi specific API client
/// </summary>
public class NiFiApiClient : BaseApiClient
{
    private string? _authToken;

    public NiFiApiClient() : base(NiFiConfig.BaseUrl)
    {
        LoggerHelper.LogInfo("NiFi API Client initialized with base URL: {BaseUrl}", NiFiConfig.BaseUrl);
    }

    /// <summary>
    /// Authenticate with NiFi using username and password
    /// </summary>
    public async Task<bool> AuthenticateAsync()
    {
        if (!string.IsNullOrEmpty(NiFiConfig.Username) && !string.IsNullOrEmpty(NiFiConfig.Password))
        {
            try
            {
                var authPayload = new
                {
                    username = NiFiConfig.Username,
                    password = NiFiConfig.Password
                };

                LoggerHelper.LogInfo("Attempting to authenticate with NiFi as user: {Username}", NiFiConfig.Username);
                var response = await PostAsync("/access/token", authPayload);

                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    _authToken = response.Content;
                    LoggerHelper.LogInfo("Authentication successful");
                    return true;
                }

                LoggerHelper.LogWarning("Authentication failed with status: {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex, "Error during authentication");
                return false;
            }
        }

        LoggerHelper.LogInfo("No credentials provided, skipping authentication");
        return true; // Return true if no auth needed
    }

    /// <summary>
    /// Get authorization headers
    /// </summary>
    private Dictionary<string, string>? GetAuthHeaders()
    {
        if (string.IsNullOrEmpty(_authToken))
            return null;

        return new Dictionary<string, string>
        {
            { "Authorization", $"Bearer {_authToken}" }
        };
    }

    /// <summary>
    /// List all process groups
    /// </summary>
    public async Task<List<ProcessorGroup>> ListProcessGroupsAsync(string processGroupId = "root")
    {
        LoggerHelper.LogInfo("Listing process groups for parent: {ProcessGroupId}", processGroupId);
        
        var response = await GetAsync($"/process-groups/{processGroupId}/process-groups", GetAuthHeaders());
        
        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            LoggerHelper.LogWarning("Failed to list process groups: {StatusCode}", response.StatusCode);
            return new List<ProcessorGroup>();
        }

        var jsonObject = JObject.Parse(response.Content);
        var processGroups = jsonObject["processGroups"]?.ToObject<List<ProcessorGroup>>() ?? new List<ProcessorGroup>();
        
        LoggerHelper.LogInfo("Found {Count} process groups", processGroups.Count);
        return processGroups;
    }

    /// <summary>
    /// List all processors in a process group
    /// </summary>
    public async Task<List<Processor>> ListProcessorsAsync(string processGroupId = "root")
    {
        LoggerHelper.LogInfo("Listing processors for process group: {ProcessGroupId}", processGroupId);
        
        var response = await GetAsync($"/process-groups/{processGroupId}/processors", GetAuthHeaders());
        
        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            LoggerHelper.LogWarning("Failed to list processors: {StatusCode}", response.StatusCode);
            return new List<Processor>();
        }

        var jsonObject = JObject.Parse(response.Content);
        var processors = jsonObject["processors"]?.ToObject<List<Processor>>() ?? new List<Processor>();
        
        LoggerHelper.LogInfo("Found {Count} processors", processors.Count);
        return processors;
    }

    /// <summary>
    /// Get processor by ID
    /// </summary>
    public async Task<Processor?> GetProcessorAsync(string processorId)
    {
        LoggerHelper.LogInfo("Getting processor: {ProcessorId}", processorId);
        return await GetAsync<Processor>($"/processors/{processorId}", GetAuthHeaders());
    }

    /// <summary>
    /// Start a processor
    /// </summary>
    public async Task<bool> StartProcessorAsync(string processorId, long version = 0)
    {
        LoggerHelper.LogInfo("Starting processor: {ProcessorId}", processorId);
        
        var updatePayload = new
        {
            revision = new { version = version },
            component = new { id = processorId, state = "RUNNING" }
        };

        var response = await PutAsync($"/processors/{processorId}", updatePayload, GetAuthHeaders());
        var success = response.IsSuccessful;
        
        LoggerHelper.LogInfo("Processor start {Result}: {StatusCode}", success ? "succeeded" : "failed", response.StatusCode);
        return success;
    }

    /// <summary>
    /// Stop a processor
    /// </summary>
    public async Task<bool> StopProcessorAsync(string processorId, long version = 0)
    {
        LoggerHelper.LogInfo("Stopping processor: {ProcessorId}", processorId);
        
        var updatePayload = new
        {
            revision = new { version = version },
            component = new { id = processorId, state = "STOPPED" }
        };

        var response = await PutAsync($"/processors/{processorId}", updatePayload, GetAuthHeaders());
        var success = response.IsSuccessful;
        
        LoggerHelper.LogInfo("Processor stop {Result}: {StatusCode}", success ? "succeeded" : "failed", response.StatusCode);
        return success;
    }

    /// <summary>
    /// Get connections (queues) in a process group
    /// </summary>
    public async Task<List<Connection>> ListConnectionsAsync(string processGroupId = "root")
    {
        LoggerHelper.LogInfo("Listing connections for process group: {ProcessGroupId}", processGroupId);
        
        var response = await GetAsync($"/process-groups/{processGroupId}/connections", GetAuthHeaders());
        
        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            LoggerHelper.LogWarning("Failed to list connections: {StatusCode}", response.StatusCode);
            return new List<Connection>();
        }

        var jsonObject = JObject.Parse(response.Content);
        var connections = jsonObject["connections"]?.ToObject<List<Connection>>() ?? new List<Connection>();
        
        LoggerHelper.LogInfo("Found {Count} connections", connections.Count);
        return connections;
    }

    /// <summary>
    /// Get flow files count in a connection (queue)
    /// </summary>
    public async Task<int> GetQueueCountAsync(string connectionId)
    {
        LoggerHelper.LogInfo("Getting queue count for connection: {ConnectionId}", connectionId);
        
        var connection = await GetAsync<Connection>($"/connections/{connectionId}", GetAuthHeaders());
        
        if (connection?.Status?.AggregateSnapshot != null)
        {
            var count = connection.Status.AggregateSnapshot.FlowFilesQueued;
            LoggerHelper.LogInfo("Queue has {Count} flow files", count);
            return count;
        }

        LoggerHelper.LogWarning("Could not get queue count");
        return 0;
    }
}
