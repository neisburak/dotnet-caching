using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DistributedCaching.Extensions;

public static class JsonExtensions
{
    public static byte[] Serialize<T>(this T item) => Serialize<T>(item, null);

    public static byte[] Serialize<T>(this T item, JsonSerializerOptions? options = null)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(item, options ?? GetOptions()));
    }

    public static T? Deserialize<T>(this byte[] val)
    {
        return Deserialize<T>(val, null);
    }

    public static T? Deserialize<T>(this byte[] val, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<T>(val, options ?? GetOptions());
    }

    private static JsonSerializerOptions GetOptions() => new JsonSerializerOptions
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}