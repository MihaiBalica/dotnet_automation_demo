using Microsoft.Extensions.Configuration;
using DotNetEnv;

namespace ApiTestAutomation.Tests.Config;

/// <summary>
/// Configuration settings for Apache NiFi API tests
/// </summary>
public class NiFiConfig
{
    private static IConfiguration? _configuration;
    private static bool _isInitialized = false;

    static NiFiConfig()
    {
        Initialize();
    }

    /// <summary>
    /// Initialize configuration from appsettings.json and .env files
    /// </summary>
    public static void Initialize()
    {
        if (_isInitialized) return;

        // Load .env file if exists
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (File.Exists(envPath))
        {
            Env.Load(envPath);
        }

        // Build configuration from appsettings.json files
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        _isInitialized = true;
    }

    /// <summary>
    /// Base URL for the NiFi API
    /// </summary>
    public static string BaseUrl => _configuration?["NiFi:BaseUrl"] ?? "http://localhost:8080/nifi-api";

    /// <summary>
    /// NiFi username - from .env or appsettings
    /// </summary>
    public static string Username
    {
        get
        {
            var username = Environment.GetEnvironmentVariable("NIFI_USERNAME");
            return username ?? _configuration?["NiFi:Username"] ?? string.Empty;
        }
    }

    /// <summary>
    /// NiFi password - from .env or appsettings
    /// </summary>
    public static string Password
    {
        get
        {
            var password = Environment.GetEnvironmentVariable("NIFI_PASSWORD");
            return password ?? _configuration?["NiFi:Password"] ?? string.Empty;
        }
    }

    /// <summary>
    /// API timeout in milliseconds
    /// </summary>
    public static int Timeout
    {
        get
        {
            var timeout = _configuration?["NiFi:DefaultTimeout"];
            return int.TryParse(timeout, out var result) ? result : 30000;
        }
    }

    /// <summary>
    /// Maximum acceptable response time in milliseconds
    /// </summary>
    public static int MaxResponseTime
    {
        get
        {
            var maxTime = _configuration?["NiFi:MaxResponseTime"];
            return int.TryParse(maxTime, out var result) ? result : 5000;
        }
    }

    /// <summary>
    /// Logging level
    /// </summary>
    public static string LogLevel => _configuration?["Logging:LogLevel"] ?? "Information";

    /// <summary>
    /// Log file path
    /// </summary>
    public static string LogFilePath => _configuration?["Logging:LogFilePath"] ?? "logs/test-execution-.log";
}
