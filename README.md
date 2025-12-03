# API Test Automation Framework

A comprehensive, generic API test automation framework built with C# and .NET, using industry-standard tools and best practices.

## Overview

This framework provides a robust foundation for API testing with the following features:

- **Generic API Client**: Reusable base client for all HTTP operations (GET, POST, PUT, DELETE, PATCH)
- **Fluent Assertions**: Readable and maintainable test assertions
- **Test Data Management**: Helpers for generating and managing test data
- **Configuration Management**: Environment-based configuration support
- **Comprehensive Examples**: Sample tests demonstrating various API operations

## Tech Stack

- **.NET 10.0**: Latest .NET framework
- **xUnit**: Testing framework
- **RestSharp 113.0.0**: HTTP client library
- **FluentAssertions 8.8.0**: Fluent assertion library
- **Newtonsoft.Json 13.0.4**: JSON serialization/deserialization

## Project Structure

```
ApiTestAutomation.Tests/
├── Config/
│   └── ApiConfig.cs                 # Configuration settings
├── Infrastructure/
│   └── BaseApiClient.cs             # Base API client for HTTP operations
├── Helpers/
│   ├── ResponseAssertions.cs        # Custom assertion helpers
│   └── TestDataHelper.cs            # Test data generation utilities
├── Models/
│   ├── Post.cs                      # Sample data model for Posts
│   └── User.cs                      # Sample data model for Users
└── Tests/
    ├── GetRequestTests.cs           # GET request examples
    ├── PostRequestTests.cs          # POST request examples
    ├── PutRequestTests.cs           # PUT request examples
    └── DeleteRequestTests.cs        # DELETE request examples
```

## Getting Started

### Prerequisites

- .NET 10.0 SDK or later
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

3. Build the solution:
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

Run specific test class:
```bash
dotnet test --filter "FullyQualifiedName~GetRequestTests"
```

Run specific test method:
```bash
dotnet test --filter "FullyQualifiedName~GetAllPosts_ShouldReturnSuccessStatusCode"
```

## Configuration

The framework supports environment-based configuration through the `ApiConfig` class. You can set the following environment variables:

- `API_BASE_URL`: Base URL of the API (default: `https://jsonplaceholder.typicode.com`)
- `API_TIMEOUT`: Request timeout in milliseconds (default: `30000`)
- `MAX_RESPONSE_TIME`: Maximum acceptable response time (default: `5000`)
- `API_AUTH_TOKEN`: Authentication token (if required)
- `API_KEY`: API key (if required)

### Setting Environment Variables

**Windows (PowerShell):**
```powershell
$env:API_BASE_URL="https://api.example.com"
```

**Linux/macOS:**
```bash
export API_BASE_URL="https://api.example.com"
```

## Usage Examples

### Basic GET Request

```csharp
[Fact]
public async Task GetAllPosts_ShouldReturnSuccessStatusCode()
{
    // Arrange
    var apiClient = new BaseApiClient(ApiConfig.BaseUrl);

    // Act
    var response = await apiClient.GetAsync("/posts");

    // Assert
    response.AssertStatusCode(HttpStatusCode.OK);
    response.AssertContentNotEmpty();
}
```

### GET Request with Deserialization

```csharp
[Fact]
public async Task GetPostById_ShouldReturnCorrectPost()
{
    var apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    var post = await apiClient.GetAsync<Post>("/posts/1");
    
    post.Should().NotBeNull();
    post!.Id.Should().Be(1);
    post.Title.Should().NotBeNullOrEmpty();
}
```

### POST Request

```csharp
[Fact]
public async Task CreatePost_ShouldReturnCreatedPost()
{
    var apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    var newPost = new Post
    {
        UserId = 1,
        Title = "Test Post",
        Body = "Test Body"
    };

    var createdPost = await apiClient.PostAsync<Post>("/posts", newPost);
    
    createdPost.Should().NotBeNull();
    createdPost!.Title.Should().Be(newPost.Title);
}
```

### PUT Request

```csharp
[Fact]
public async Task UpdatePost_ShouldReturnUpdatedPost()
{
    var apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    var updatedPost = new Post
    {
        Id = 1,
        UserId = 1,
        Title = "Updated Title",
        Body = "Updated Body"
    };

    var result = await apiClient.PutAsync<Post>("/posts/1", updatedPost);
    
    result.Should().NotBeNull();
    result!.Title.Should().Be(updatedPost.Title);
}
```

### DELETE Request

```csharp
[Fact]
public async Task DeletePost_ShouldReturnSuccessStatusCode()
{
    var apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    var response = await apiClient.DeleteAsync("/posts/1");
    
    response.AssertSuccess();
}
```

## Custom Assertions

The framework provides custom assertion helpers for common API testing scenarios:

```csharp
// Status code assertion
response.AssertStatusCode(HttpStatusCode.OK);

// Success assertion (any 2xx status)
response.AssertSuccess();

// Content assertions
response.AssertContentNotEmpty();
response.AssertContentType("application/json");

// Header assertions
response.AssertHeaderExists("Content-Type");
response.AssertHeaderValue("Content-Type", "application/json");

// Response time assertion
response.AssertResponseTime(5000); // Max 5 seconds
```

## Test Data Helpers

Generate test data easily:

```csharp
// Random string
var randomString = TestDataHelper.GenerateRandomString(20);

// Random email
var email = TestDataHelper.GenerateRandomEmail();

// Random integer
var randomId = TestDataHelper.GenerateRandomInt(1, 100);

// JSON serialization
var json = TestDataHelper.SerializeToJson(myObject);

// JSON deserialization
var obj = TestDataHelper.DeserializeFromJson<MyType>(jsonString);
```

## Adding Custom Headers

```csharp
var headers = new Dictionary<string, string>
{
    { "Authorization", "Bearer your-token" },
    { "Custom-Header", "custom-value" }
};

var response = await apiClient.GetAsync("/protected-endpoint", headers);
```

## Extending the Framework

### Adding New Models

Create model classes in the `Models` folder:

```csharp
public class YourModel
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
}
```

### Adding Custom API Clients

Extend `BaseApiClient` for specific APIs:

```csharp
public class MyApiClient : BaseApiClient
{
    public MyApiClient() : base(ApiConfig.BaseUrl)
    {
    }

    public async Task<MyModel> GetMyResource(int id)
    {
        return await GetAsync<MyModel>($"/myresource/{id}");
    }
}
```

### Adding Custom Assertions

Add extension methods to `ResponseAssertions`:

```csharp
public static void AssertCustomCondition(this RestResponse response)
{
    // Your custom assertion logic
}
```

## Best Practices

1. **Use Descriptive Test Names**: Follow the pattern `MethodName_StateUnderTest_ExpectedBehavior`
2. **Arrange-Act-Assert**: Structure tests clearly with AAA pattern
3. **Independent Tests**: Each test should be independent and not rely on others
4. **Clean Up**: Implement `IDisposable` for test classes that need cleanup
5. **Use Configuration**: Don't hardcode URLs or credentials
6. **Test Data Generation**: Use helpers for generating test data
7. **Meaningful Assertions**: Use fluent assertions for better readability
8. **Handle Errors**: Test both success and error scenarios

## CI/CD Integration

### GitHub Actions Example

```yaml
name: API Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '10.0.x'
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
        env:
          API_BASE_URL: ${{ secrets.API_BASE_URL }}
```

## Troubleshooting

### Network Issues

If tests fail with connection errors:
- Check if the API endpoint is accessible
- Verify network/firewall settings
- Ensure SSL certificates are valid
- Check if a proxy is required

### SSL Certificate Issues

For development environments with self-signed certificates:
```csharp
var options = new RestClientOptions(baseUrl)
{
    RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
};
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new features
5. Ensure all tests pass
6. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For issues, questions, or contributions, please open an issue on the GitHub repository.

## Acknowledgments

- Built with [RestSharp](https://restsharp.dev/)
- Testing with [xUnit](https://xunit.net/)
- Assertions with [FluentAssertions](https://fluentassertions.com/)
- Sample API: [JSONPlaceholder](https://jsonplaceholder.typicode.com/)