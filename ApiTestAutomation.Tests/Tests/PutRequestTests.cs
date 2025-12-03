using ApiTestAutomation.Tests.Config;
using ApiTestAutomation.Tests.Helpers;
using ApiTestAutomation.Tests.Infrastructure;
using ApiTestAutomation.Tests.Models;
using FluentAssertions;
using System.Net;

namespace ApiTestAutomation.Tests.Tests;

/// <summary>
/// Sample API tests demonstrating PUT operations
/// </summary>
public class PutRequestTests : IDisposable
{
    private readonly BaseApiClient _apiClient;

    public PutRequestTests()
    {
        _apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    }

    [Fact]
    public async Task UpdatePost_ShouldReturnSuccessStatusCode()
    {
        // Arrange
        var postId = 1;
        var updatedPost = new Post
        {
            Id = postId,
            UserId = 1,
            Title = "Updated Title",
            Body = "Updated Body"
        };

        // Act
        var response = await _apiClient.PutAsync($"/posts/{postId}", updatedPost);

        // Assert
        response.AssertStatusCode(HttpStatusCode.OK);
        response.AssertSuccess();
    }

    [Fact]
    public async Task UpdatePost_ShouldReturnUpdatedPost()
    {
        // Arrange
        var postId = 1;
        var updatedPost = new Post
        {
            Id = postId,
            UserId = 1,
            Title = TestDataHelper.GenerateRandomString(25),
            Body = TestDataHelper.GenerateRandomString(60)
        };

        // Act
        var result = await _apiClient.PutAsync<Post>($"/posts/{postId}", updatedPost);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(postId);
        result.Title.Should().Be(updatedPost.Title);
        result.Body.Should().Be(updatedPost.Body);
    }

    [Fact]
    public async Task UpdateNonExistentPost_ShouldReturn404()
    {
        // Arrange
        var invalidPostId = 99999;
        var updatedPost = new Post
        {
            Id = invalidPostId,
            UserId = 1,
            Title = "Test",
            Body = "Test"
        };

        // Act
        var response = await _apiClient.PutAsync($"/posts/{invalidPostId}", updatedPost);

        // Assert
        response.AssertStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdatePost_WithPartialData_ShouldSucceed()
    {
        // Arrange
        var postId = 1;
        var partialUpdate = new
        {
            title = "Partially Updated Title"
        };

        // Act
        var response = await _apiClient.PutAsync($"/posts/{postId}", partialUpdate);

        // Assert
        response.AssertSuccess();
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}
