using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DistributedCaching.Extensions;

public static class JsonExtensions
{
    public static byte[] Serialize<T>(this T item) => Serialize<T>(item);

    public static byte[] Serialize<T>(this T item, JsonSerializerOptions? options)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(item, options ?? GetOptions()));
    }

    public static T Deserialize<T>(this byte[] val) => Deserialize<T>(val);

    public static T? Deserialize<T>(this byte[] val, JsonSerializerOptions? options)
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