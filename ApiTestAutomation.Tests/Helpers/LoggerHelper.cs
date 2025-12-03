using Serilog;
using Serilog.Events;
using ApiTestAutomation.Tests.Config;

namespace ApiTestAutomation.Tests.Helpers;

/// <summary>
/// Logging helper using Serilog
/// </summary>
public static class LoggerHelper
{
    private static ILogger? _logger;

    /// <summary>
    /// Initialize the logger
    /// </summary>
    public static void InitializeLogger()
    {
        if (_logger != null) return;

        var logLevel = Enum.TryParse<LogEventLevel>(NiFiConfig.LogLevel, out var level) 
            ? level 
            : LogEventLevel.Information;

        _logger = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                NiFiConfig.LogFilePath,
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        _logger.Information("Logger initialized with level: {LogLevel}", logLevel);
    }

    /// <summary>
    /// Get the logger instance
    /// </summary>
    public static ILogger Logger
    {
        get
        {
            if (_logger == null)
            {
                InitializeLogger();
            }
            return _logger!;
        }
    }

    /// <summary>
    /// Log information message
    /// </summary>
    public static void LogInfo(string message, params object[] args)
    {
        Logger.Information(message, args);
    }

    /// <summary>
    /// Log debug message
    /// </summary>
    public static void LogDebug(string message, params object[] args)
    {
        Logger.Debug(message, args);
    }

    /// <summary>
    /// Log warning message
    /// </summary>
    public static void LogWarning(string message, params object[] args)
    {
        Logger.Warning(message, args);
    }

    /// <summary>
    /// Log error message
    /// </summary>
    public static void LogError(Exception ex, string message, params object[] args)
    {
        Logger.Error(ex, message, args);
    }

    /// <summary>
    /// Log error message without exception
    /// </summary>
    public static void LogError(string message, params object[] args)
    {
        Logger.Error(message, args);
    }
}
