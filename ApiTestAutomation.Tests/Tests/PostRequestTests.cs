using ApiTestAutomation.Tests.Config;
using ApiTestAutomation.Tests.Helpers;
using ApiTestAutomation.Tests.Infrastructure;
using ApiTestAutomation.Tests.Models;
using FluentAssertions;
using System.Net;

namespace ApiTestAutomation.Tests.Tests;

/// <summary>
/// Sample API tests demonstrating POST operations
/// </summary>
public class PostRequestTests : IDisposable
{
    private readonly BaseApiClient _apiClient;

    public PostRequestTests()
    {
        _apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    }

    [Fact]
    public async Task CreatePost_ShouldReturnCreatedStatusCode()
    {
        // Arrange
        var newPost = new Post
        {
            UserId = 1,
            Title = TestDataHelper.GenerateRandomString(20),
            Body = TestDataHelper.GenerateRandomString(50)
        };

        // Act
        var response = await _apiClient.PostAsync("/posts", newPost);

        // Assert
        response.AssertStatusCode(HttpStatusCode.Created);
        response.AssertSuccess();
        response.AssertContentNotEmpty();
    }

    [Fact]
    public async Task CreatePost_ShouldReturnCreatedPost()
    {
        // Arrange
        var newPost = new Post
        {
            UserId = 1,
            Title = "Test Post Title",
            Body = "Test Post Body"
        };

        // Act
        var createdPost = await _apiClient.PostAsync<Post>("/posts", newPost);

        // Assert
        createdPost.Should().NotBeNull();
        createdPost!.Id.Should().BeGreaterThan(0);
        createdPost.UserId.Should().Be(newPost.UserId);
        createdPost.Title.Should().Be(newPost.Title);
        createdPost.Body.Should().Be(newPost.Body);
    }

    [Fact]
    public async Task CreatePost_WithRandomData_ShouldSucceed()
    {
        // Arrange
        var newPost = new Post
        {
            UserId = TestDataHelper.GenerateRandomInt(1, 10),
            Title = $"Automated Test - {TestDataHelper.GenerateRandomString(15)}",
            Body = $"This is a test post created at {DateTime.UtcNow}"
        };

        // Act
        var response = await _apiClient.PostAsync("/posts", newPost);

        // Assert
        response.AssertSuccess();
        response.AssertContentType("application/json");
    }

    [Fact]
    public async Task CreatePost_WithMinimalData_ShouldSucceed()
    {
        // Arrange
        var newPost = new Post
        {
            UserId = 1,
            Title = "Minimal",
            Body = "Body"
        };

        // Act
        var createdPost = await _apiClient.PostAsync<Post>("/posts", newPost);

        // Assert
        createdPost.Should().NotBeNull();
        createdPost!.Title.Should().Be(newPost.Title);
        createdPost.Body.Should().Be(newPost.Body);
    }

    [Fact]
    public async Task CreateMultiplePosts_ShouldAllSucceed()
    {
        // Arrange
        var postsToCreate = new List<Post>
        {
            new Post { UserId = 1, Title = "First Post", Body = "First Body" },
            new Post { UserId = 2, Title = "Second Post", Body = "Second Body" },
            new Post { UserId = 3, Title = "Third Post", Body = "Third Body" }
        };

        // Act & Assert
        foreach (var post in postsToCreate)
        {
            var response = await _apiClient.PostAsync("/posts", post);
            response.AssertSuccess();
        }
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}
