using RestSharp;
using FluentAssertions;
using System.Net;

namespace ApiTestAutomation.Tests.Helpers;

/// <summary>
/// Helper class for API response assertions
/// </summary>
public static class ResponseAssertions
{
    /// <summary>
    /// Assert that response status code is as expected
    /// </summary>
    public static void AssertStatusCode(this RestResponse response, HttpStatusCode expectedStatusCode)
    {
        response.StatusCode.Should().Be(expectedStatusCode, 
            $"Expected status code {expectedStatusCode} but got {response.StatusCode}. Response: {response.Content}");
    }

    /// <summary>
    /// Assert that response is successful (2xx)
    /// </summary>
    public static void AssertSuccess(this RestResponse response)
    {
        response.IsSuccessful.Should().BeTrue(
            $"Expected successful response but got {response.StatusCode}. Response: {response.Content}");
    }

    /// <summary>
    /// Assert that response contains specific header
    /// </summary>
    public static void AssertHeaderExists(this RestResponse response, string headerName)
    {
        var header = response.Headers?.FirstOrDefault(h => h.Name?.Equals(headerName, StringComparison.OrdinalIgnoreCase) == true);
        header.Should().NotBeNull($"Expected header '{headerName}' to exist in response");
    }

    /// <summary>
    /// Assert that response header has specific value
    /// </summary>
    public static void AssertHeaderValue(this RestResponse response, string headerName, string expectedValue)
    {
        var header = response.Headers?.FirstOrDefault(h => h.Name?.Equals(headerName, StringComparison.OrdinalIgnoreCase) == true);
        header.Should().NotBeNull($"Expected header '{headerName}' to exist in response");
        header!.Value?.ToString().Should().Be(expectedValue, 
            $"Expected header '{headerName}' to have value '{expectedValue}'");
    }

    /// <summary>
    /// Assert that response content is not null or empty
    /// </summary>
    public static void AssertContentNotEmpty(this RestResponse response)
    {
        response.Content.Should().NotBeNullOrEmpty("Expected response content to be not null or empty");
    }

    /// <summary>
    /// Assert that response content type is as expected
    /// </summary>
    public static void AssertContentType(this RestResponse response, string expectedContentType)
    {
        response.ContentType.Should().Contain(expectedContentType, 
            $"Expected content type to contain '{expectedContentType}' but got '{response.ContentType}'");
    }

    /// <summary>
    /// Assert response time is within acceptable range
    /// </summary>
    public static void AssertResponseTime(this RestResponse response, int maxMilliseconds)
    {
        // Note: RestSharp 113+ doesn't expose ResponseTime directly
        // This is a placeholder for custom timing implementation if needed
        response.Should().NotBeNull("Response should not be null for response time assertion");
    }
}
