# Apache NiFi API Test Automation Framework

A comprehensive BDD (Behavior-Driven Development) API test automation framework for Apache NiFi built with C# and .NET 8.0, using Reqnroll (Gherkin), Serilog, and industry-standard tools.

## Overview

This framework provides robust test automation for Apache NiFi with the following features:

- **BDD with Reqnroll**: Gherkin feature files for readable test scenarios
- **Apache NiFi Specific**: Models and clients tailored for NiFi API  
- **Serilog Logging**: Comprehensive logging for test execution
- **Configuration Management**: appsettings.json and .env file support
- **Generic API Client**: Reusable base client for all HTTP operations
- **Fluent Assertions**: Readable and maintainable test assertions

## Tech Stack

- **.NET 8.0**: Target framework
- **NUnit**: Testing framework
- **Reqnroll 2.2.0**: BDD framework (Gherkin/SpecFlow alternative)
- **RestSharp 113.0.0**: HTTP client library
- **FluentAssertions 8.8.0**: Fluent assertion library
- **Serilog 4.1.0**: Structured logging
- **Newtonsoft.Json 13.0.4**: JSON serialization/deserialization
- **DotNetEnv 3.1.1**: .env file support

## Project Structure

```
ApiTestAutomation.Tests/
├── Config/
│   └── NiFiConfig.cs                 # Configuration management (appsettings + .env)
├── Infrastructure/
│   ├── BaseApiClient.cs              # Base HTTP client
│   └── NiFiApiClient.cs              # NiFi-specific API client
├── Helpers/
│   ├── ResponseAssertions.cs         # Custom assertion helpers
│   ├── TestDataHelper.cs             # Test data generation utilities
│   └── LoggerHelper.cs               # Serilog logging helper
├── Models/
│   ├── Processor.cs                  # NiFi Processor model
│   ├── ProcessorGroup.cs             # NiFi ProcessorGroup model
│   └── Connection.cs                 # NiFi Connection (Queue) model
├── Features/                         # Gherkin feature files
│   ├── ProcessGroups.feature         # ProcessorGroup scenarios
│   ├── Processors.feature            # Processor management scenarios
│   └── QueueManagement.feature       # Queue/Connection scenarios
├── StepDefinitions/                  # Step definition implementations
│   ├── ProcessGroupSteps.cs
│   ├── ProcessorSteps.cs
│   └── QueueManagementSteps.cs
├── appsettings.json                  # Main configuration
├── appsettings.dev.json              # Development overrides
└── .env.example                      # Example environment variables
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Apache NiFi instance (running locally or accessible)
- Visual Studio 2022, VS Code, or any .NET-compatible IDE

### Installation

1. Clone the repository:
```bash
git clone https://github.com/MihaiBalica/dotnet_automation_demo.git
cd dotnet_automation_demo
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Configure your NiFi connection:

Create a `.env` file in the `ApiTestAutomation.Tests` directory:
```bash
NIFI_USERNAME=your_username
NIFI_PASSWORD=your_password
```

Or update `appsettings.json`:
```json
{
  "NiFi": {
    "BaseUrl": "http://your-nifi-host:8080/nifi-api",
    "Username": "admin",
    "Password": "your_password"
  }
}
```

4. Build the solution:
```bash
dotnet build
```

### Running Tests

Run all tests:
```bash
dotnet test
```

Run tests with detailed output:
```bash
dotnet test --verbosity detailed
```

Run specific feature:
```bash
dotnet test --filter "FullyQualifiedName~ProcessGroups"
```

Run specific scenario:
```bash
dotnet test --filter "DisplayName~List all process groups"
```

## Configuration

The framework supports multiple configuration sources with the following priority (highest to lowest):

1. **Environment Variables** (from .env file or system)
2. **appsettings.dev.json** (for development)
3. **appsettings.json** (base configuration)

### Configuration Options

**appsettings.json:**
```json
{
  "NiFi": {
    "BaseUrl": "http://localhost:8080/nifi-api",
    "DefaultTimeout": 30000,
    "MaxResponseTime": 5000
  },
  "Logging": {
    "LogLevel": "Information",
    "LogFilePath": "logs/test-execution-.log"
  }
}
```

**.env file:**
```bash
NIFI_USERNAME=admin
NIFI_PASSWORD=your_secure_password
```

## Example Feature Files

### Process Groups Feature

```gherkin
Feature: NiFi Process Groups Management
    As a NiFi administrator
    I want to manage process groups
    So that I can organize and monitor my data flows

Background:
    Given I am authenticated with NiFi

Scenario: List all process groups in root
    When I list all process groups in "root"
    Then I should receive a successful response
    And the process groups list should not be empty
```

### Processors Feature

```gherkin
Feature: NiFi Processors Management
    As a NiFi administrator
    I want to manage processors
    So that I can control data processing flows

Scenario: Start a processor
    Given I have a processor in "root" process group
    When I start the processor
    Then the processor should be in "RUNNING" state

Scenario: Stop a processor
    Given I have a running processor in "root" process group
    When I stop the processor
    Then the processor should be in "STOPPED" state
```

### Queue Management Feature

```gherkin
Feature: NiFi Queue Management
    As a NiFi administrator
    I want to monitor queues
    So that I can track flow file processing

Scenario: Count flow files in a queue
    Given I have a connection in "root" process group
    When I get the queue count for the connection
    Then the queue count should be a valid number
    And the queue count should be greater than or equal to 0
```

## NiFi API Client Usage

### Authentication

```csharp
var nifiClient = new NiFiApiClient();
await nifiClient.AuthenticateAsync(); // Uses credentials from .env or appsettings
```

### List Process Groups

```csharp
var processGroups = await nifiClient.ListProcessGroupsAsync("root");
foreach (var pg in processGroups)
{
    LoggerHelper.LogInfo("Process Group: {Name}", pg.Component?.Name);
}
```

### List Processors

```csharp
var processors = await nifiClient.ListProcessorsAsync("root");
foreach (var processor in processors)
{
    LoggerHelper.LogInfo("Processor: {Name}, State: {State}", 
        processor.Component?.Name, 
        processor.Component?.State);
}
```

### Start/Stop Processor

```csharp
// Start processor
var processor = await nifiClient.GetProcessorAsync(processorId);
var version = processor?.Revision?.Version ?? 0;
await nifiClient.StartProcessorAsync(processorId, version);

// Stop processor
await nifiClient.StopProcessorAsync(processorId, version);
```

### Get Queue Count

```csharp
var connections = await nifiClient.ListConnectionsAsync("root");
foreach (var connection in connections)
{
    var count = await nifiClient.GetQueueCountAsync(connection.Id);
    LoggerHelper.LogInfo("Connection: {Name}, Queue Count: {Count}", 
        connection.Component?.Name, count);
}
```

## Logging

The framework uses Serilog for comprehensive logging. Logs are written to both console and file.

**Console Output:**
```
[2024-12-03 10:30:45 INF] Authenticating with NiFi
[2024-12-03 10:30:46 INF] Authentication successful
[2024-12-03 10:30:47 INF] Listing processors for process group: root
[2024-12-03 10:30:48 INF] Found 5 processors
```

**Log Files:**
- Location: `logs/test-execution-{date}.log`
- Rolling: Daily
- Format: Structured logging with timestamp, level, and message

### Using Logger in Tests

```csharp
LoggerHelper.LogInfo("Starting test scenario");
LoggerHelper.LogDebug("Detailed debug information: {Details}", details);
LoggerHelper.LogWarning("Warning message");
LoggerHelper.LogError(exception, "Error occurred: {Message}", message);
```

## Extending the Framework

### Adding New Models

Create model classes in the `Models` folder for additional NiFi objects:

```csharp
public class ControllerService
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
}
```

### Adding New Feature Files

Create `.feature` files in the `Features` folder:

```gherkin
Feature: Controller Services Management
    As a NiFi administrator
    I want to manage controller services
    So that I can configure shared services

Scenario: List controller services
    Given I am authenticated with NiFi
    When I list all controller services
    Then I should have at least 0 controller services
```

### Adding Step Definitions

Create corresponding step definition classes in `StepDefinitions`:

```csharp
[Binding]
public class ControllerServiceSteps
{
    private readonly NiFiApiClient _nifiClient;
    
    public ControllerServiceSteps()
    {
        _nifiClient = new NiFiApiClient();
    }
    
    [When(@"I list all controller services")]
    public async Task WhenIListAllControllerServices()
    {
        // Implementation
    }
}
```

### Extending NiFi API Client

Add new methods to `NiFiApiClient` for additional operations:

```csharp
public async Task<List<ControllerService>> ListControllerServicesAsync(string processGroupId = "root")
{
    LoggerHelper.LogInfo("Listing controller services for: {ProcessGroupId}", processGroupId);
    var response = await GetAsync($"/flow/process-groups/{processGroupId}/controller-services", GetAuthHeaders());
    // Parse and return results
}
```

## Best Practices

1. **BDD Scenarios**: Write scenarios in plain English that stakeholders can understand
2. **Given-When-Then**: Structure scenarios with clear preconditions, actions, and assertions
3. **Logging**: Use LoggerHelper for comprehensive test logging
4. **Configuration**: Keep sensitive data in .env file, general config in appsettings.json
5. **Independent Tests**: Each scenario should be independent and not rely on others
6. **Clean Data**: Clean up test data after scenarios if needed
7. **Meaningful Names**: Use descriptive scenario and step names
8. **Error Handling**: Use try-catch in step definitions for better error messages

## CI/CD Integration

### GitHub Actions Example

```yaml
name: NiFi API Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    permissions:
      contents: read
    services:
      nifi:
        image: apache/nifi:latest
        ports:
          - 8080:8080
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
        env:
          NIFI_USERNAME: ${{ secrets.NIFI_USERNAME }}
          NIFI_PASSWORD: ${{ secrets.NIFI_PASSWORD }}
      - name: Upload logs
        if: always()
        uses: actions/upload-artifact@v4
        with:
          name: test-logs
          path: logs/
```

## Troubleshooting

### Network Issues

If tests fail with connection errors:
- Check if the NiFi instance is running
- Verify the BaseUrl in appsettings.json
- Ensure network/firewall settings allow connections
- Check if NiFi is secured and requires authentication

### Authentication Issues

If authentication fails:
- Verify username and password in .env file
- Check NiFi authentication configuration
- Ensure NiFi is configured for username/password auth
- Check NiFi logs for authentication errors

### Test Failures

If specific tests fail:
- Check test logs in `logs/` directory
- Review console output for detailed error messages
- Verify NiFi has the expected processors/connections
- Ensure NiFi instance has required permissions

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please open an issue on the GitHub repository.

## Acknowledgments

- Built for [Apache NiFi](https://nifi.apache.org/)
- BDD framework: [Reqnroll](https://reqnroll.net/)
- HTTP client: [RestSharp](https://restsharp.dev/)
- Testing with [NUnit](https://nunit.org/)
- Assertions with [FluentAssertions](https://fluentassertions.com/)
- Logging with [Serilog](https://serilog.net/)
