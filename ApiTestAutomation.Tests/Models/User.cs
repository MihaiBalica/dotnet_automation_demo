using Newtonsoft.Json;

namespace ApiTestAutomation.Tests.Models;

/// <summary>
/// Model representing a User object (for JSONPlaceholder API example)
/// </summary>
public class User
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("address")]
    public Address? Address { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonProperty("website")]
    public string Website { get; set; } = string.Empty;

    [JsonProperty("company")]
    public Company? Company { get; set; }
}

public class Address
{
    [JsonProperty("street")]
    public string Street { get; set; } = string.Empty;

    [JsonProperty("suite")]
    public string Suite { get; set; } = string.Empty;

    [JsonProperty("city")]
    public string City { get; set; } = string.Empty;

    [JsonProperty("zipcode")]
    public string Zipcode { get; set; } = string.Empty;

    [JsonProperty("geo")]
    public Geo? Geo { get; set; }
}

public class Geo
{
    [JsonProperty("lat")]
    public string Lat { get; set; } = string.Empty;

    [JsonProperty("lng")]
    public string Lng { get; set; } = string.Empty;
}

public class Company
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("catchPhrase")]
    public string CatchPhrase { get; set; } = string.Empty;

    [JsonProperty("bs")]
    public string Bs { get; set; } = string.Empty;
}
