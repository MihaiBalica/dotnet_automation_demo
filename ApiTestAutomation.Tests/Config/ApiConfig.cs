namespace ApiTestAutomation.Tests.Config;

/// <summary>
/// Configuration settings for API tests
/// </summary>
public class ApiConfig
{
    /// <summary>
    /// Base URL for the API under test
    /// </summary>
    public static string BaseUrl => GetEnvironmentVariable("API_BASE_URL", "https://jsonplaceholder.typicode.com");

    /// <summary>
    /// API timeout in milliseconds
    /// </summary>
    public static int Timeout => int.Parse(GetEnvironmentVariable("API_TIMEOUT", "30000"));

    /// <summary>
    /// Maximum acceptable response time in milliseconds
    /// </summary>
    public static int MaxResponseTime => int.Parse(GetEnvironmentVariable("MAX_RESPONSE_TIME", "5000"));

    /// <summary>
    /// API authentication token (if required)
    /// </summary>
    public static string? AuthToken => GetEnvironmentVariable("API_AUTH_TOKEN", null);

    /// <summary>
    /// API key (if required)
    /// </summary>
    public static string? ApiKey => GetEnvironmentVariable("API_KEY", null);

    /// <summary>
    /// Get environment variable with fallback to default value
    /// </summary>
    private static string GetEnvironmentVariable(string key, string? defaultValue)
    {
        return Environment.GetEnvironmentVariable(key) ?? defaultValue ?? string.Empty;
    }
}
