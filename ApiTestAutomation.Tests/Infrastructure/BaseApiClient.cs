using RestSharp;
using Newtonsoft.Json;

namespace ApiTestAutomation.Tests.Infrastructure;

/// <summary>
/// Base API client for making HTTP requests
/// </summary>
public class BaseApiClient
{
    protected readonly RestClient Client;
    protected readonly string BaseUrl;

    public BaseApiClient(string baseUrl)
    {
        BaseUrl = baseUrl;
        var options = new RestClientOptions(baseUrl)
        {
            ThrowOnAnyError = false
        };
        Client = new RestClient(options);
    }

    /// <summary>
    /// Execute GET request
    /// </summary>
    public async Task<RestResponse> GetAsync(string endpoint, Dictionary<string, string>? headers = null)
    {
        var request = new RestRequest(endpoint, Method.Get);
        AddHeaders(request, headers);
        return await Client.ExecuteAsync(request);
    }

    /// <summary>
    /// Execute GET request and deserialize response
    /// </summary>
    public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? headers = null)
    {
        var response = await GetAsync(endpoint, headers);
        return DeserializeResponse<T>(response);
    }

    /// <summary>
    /// Execute POST request
    /// </summary>
    public async Task<RestResponse> PostAsync(string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        var request = new RestRequest(endpoint, Method.Post);
        AddHeaders(request, headers);
        if (body != null)
        {
            request.AddJsonBody(body);
        }
        return await Client.ExecuteAsync(request);
    }

    /// <summary>
    /// Execute POST request and deserialize response
    /// </summary>
    public async Task<T?> PostAsync<T>(string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        var response = await PostAsync(endpoint, body, headers);
        return DeserializeResponse<T>(response);
    }

    /// <summary>
    /// Execute PUT request
    /// </summary>
    public async Task<RestResponse> PutAsync(string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        var request = new RestRequest(endpoint, Method.Put);
        AddHeaders(request, headers);
        if (body != null)
        {
            request.AddJsonBody(body);
        }
        return await Client.ExecuteAsync(request);
    }

    /// <summary>
    /// Execute PUT request and deserialize response
    /// </summary>
    public async Task<T?> PutAsync<T>(string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        var response = await PutAsync(endpoint, body, headers);
        return DeserializeResponse<T>(response);
    }

    /// <summary>
    /// Execute DELETE request
    /// </summary>
    public async Task<RestResponse> DeleteAsync(string endpoint, Dictionary<string, string>? headers = null)
    {
        var request = new RestRequest(endpoint, Method.Delete);
        AddHeaders(request, headers);
        return await Client.ExecuteAsync(request);
    }

    /// <summary>
    /// Execute PATCH request
    /// </summary>
    public async Task<RestResponse> PatchAsync(string endpoint, object? body = null, Dictionary<string, string>? headers = null)
    {
        var request = new RestRequest(endpoint, Method.Patch);
        AddHeaders(request, headers);
        if (body != null)
        {
            request.AddJsonBody(body);
        }
        return await Client.ExecuteAsync(request);
    }

    /// <summary>
    /// Add headers to request
    /// </summary>
    private void AddHeaders(RestRequest request, Dictionary<string, string>? headers)
    {
        if (headers == null) return;
        
        foreach (var header in headers)
        {
            request.AddHeader(header.Key, header.Value);
        }
    }

    /// <summary>
    /// Deserialize response content to specified type
    /// </summary>
    private T? DeserializeResponse<T>(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
            return default;

        return JsonConvert.DeserializeObject<T>(response.Content);
    }
}
