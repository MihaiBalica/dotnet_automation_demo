using ApiTestAutomation.Tests.Config;
using ApiTestAutomation.Tests.Helpers;
using ApiTestAutomation.Tests.Infrastructure;
using System.Net;

namespace ApiTestAutomation.Tests.Tests;

/// <summary>
/// Sample API tests demonstrating DELETE operations
/// </summary>
public class DeleteRequestTests : IDisposable
{
    private readonly BaseApiClient _apiClient;

    public DeleteRequestTests()
    {
        _apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    }

    [Fact]
    public async Task DeletePost_ShouldReturnSuccessStatusCode()
    {
        // Arrange
        var postId = 1;

        // Act
        var response = await _apiClient.DeleteAsync($"/posts/{postId}");

        // Assert
        response.AssertStatusCode(HttpStatusCode.OK);
        response.AssertSuccess();
    }

    [Fact]
    public async Task DeleteNonExistentPost_ShouldReturn404()
    {
        // Arrange
        var invalidPostId = 99999;

        // Act
        var response = await _apiClient.DeleteAsync($"/posts/{invalidPostId}");

        // Assert
        response.AssertStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteMultiplePosts_ShouldAllSucceed()
    {
        // Arrange
        var postIds = new[] { 1, 2, 3, 4, 5 };

        // Act & Assert
        foreach (var postId in postIds)
        {
            var response = await _apiClient.DeleteAsync($"/posts/{postId}");
            response.AssertSuccess();
        }
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}
