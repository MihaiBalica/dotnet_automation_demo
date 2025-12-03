using Newtonsoft.Json;

namespace ApiTestAutomation.Tests.Helpers;

/// <summary>
/// Helper class for managing test data
/// </summary>
public static class TestDataHelper
{
    /// <summary>
    /// Generate random string
    /// </summary>
    public static string GenerateRandomString(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Generate random email
    /// </summary>
    public static string GenerateRandomEmail()
    {
        return $"{GenerateRandomString(8)}@test.com";
    }

    /// <summary>
    /// Generate random integer within range
    /// </summary>
    public static int GenerateRandomInt(int min = 1, int max = 1000)
    {
        var random = new Random();
        return random.Next(min, max);
    }

    /// <summary>
    /// Serialize object to JSON
    /// </summary>
    public static string SerializeToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    /// <summary>
    /// Deserialize JSON to object
    /// </summary>
    public static T? DeserializeFromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>
    /// Read JSON from file
    /// </summary>
    public static T? ReadJsonFromFile<T>(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return DeserializeFromJson<T>(json);
    }

    /// <summary>
    /// Write JSON to file
    /// </summary>
    public static void WriteJsonToFile(string filePath, object obj)
    {
        var json = SerializeToJson(obj);
        File.WriteAllText(filePath, json);
    }
}
