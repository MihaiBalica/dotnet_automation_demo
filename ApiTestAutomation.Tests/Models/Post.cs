using Newtonsoft.Json;

namespace ApiTestAutomation.Tests.Models;

/// <summary>
/// Model representing a Post object (for JSONPlaceholder API example)
/// </summary>
public class Post
{
    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [JsonProperty("body")]
    public string Body { get; set; } = string.Empty;
}
