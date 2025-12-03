using ApiTestAutomation.Tests.Config;
using ApiTestAutomation.Tests.Helpers;
using ApiTestAutomation.Tests.Infrastructure;
using ApiTestAutomation.Tests.Models;
using FluentAssertions;
using System.Net;

namespace ApiTestAutomation.Tests.Tests;

/// <summary>
/// Sample API tests demonstrating GET operations
/// </summary>
public class GetRequestTests : IDisposable
{
    private readonly BaseApiClient _apiClient;

    public GetRequestTests()
    {
        _apiClient = new BaseApiClient(ApiConfig.BaseUrl);
    }

    [Fact]
    public async Task GetAllPosts_ShouldReturnSuccessStatusCode()
    {
        // Act
        var response = await _apiClient.GetAsync("/posts");

        // Assert
        response.AssertStatusCode(HttpStatusCode.OK);
        response.AssertSuccess();
        response.AssertContentNotEmpty();
        response.AssertContentType("application/json");
    }

    [Fact]
    public async Task GetAllPosts_ShouldReturnListOfPosts()
    {
        // Act
        var posts = await _apiClient.GetAsync<List<Post>>("/posts");

        // Assert
        posts.Should().NotBeNull();
        posts.Should().NotBeEmpty();
        posts!.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetPostById_ShouldReturnCorrectPost()
    {
        // Arrange
        var postId = 1;

        // Act
        var post = await _apiClient.GetAsync<Post>($"/posts/{postId}");

        // Assert
        post.Should().NotBeNull();
        post!.Id.Should().Be(postId);
        post.Title.Should().NotBeNullOrEmpty();
        post.Body.Should().NotBeNullOrEmpty();
        post.UserId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetNonExistentPost_ShouldReturn404()
    {
        // Arrange
        var invalidPostId = 99999;

        // Act
        var response = await _apiClient.GetAsync($"/posts/{invalidPostId}");

        // Assert
        response.AssertStatusCode(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnListOfUsers()
    {
        // Act
        var users = await _apiClient.GetAsync<List<User>>("/users");

        // Assert
        users.Should().NotBeNull();
        users.Should().NotBeEmpty();
        users!.Count.Should().BeGreaterThan(0);
        
        // Validate first user has required fields
        var firstUser = users.First();
        firstUser.Id.Should().BeGreaterThan(0);
        firstUser.Name.Should().NotBeNullOrEmpty();
        firstUser.Email.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUserWithAllFields()
    {
        // Arrange
        var userId = 1;

        // Act
        var user = await _apiClient.GetAsync<User>($"/users/{userId}");

        // Assert
        user.Should().NotBeNull();
        user!.Id.Should().Be(userId);
        user.Name.Should().NotBeNullOrEmpty();
        user.Username.Should().NotBeNullOrEmpty();
        user.Email.Should().NotBeNullOrEmpty();
        user.Address.Should().NotBeNull();
        user.Company.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRequest_ResponseTimeShouldBeAcceptable()
    {
        // Act
        var response = await _apiClient.GetAsync("/posts/1");

        // Assert
        response.AssertSuccess();
        response.AssertResponseTime(ApiConfig.MaxResponseTime);
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}
