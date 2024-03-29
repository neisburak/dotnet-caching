using System.Net;
using System.Text.Json;

namespace Shared.Extensions;

public static class HttpClientExtensions
{
    private static HttpClient GetClient(this string url)
    {
        return new HttpClient { BaseAddress = new Uri(url) };
    }

    public static async Task<T?> GetAsync<T>(this string url, string? parameters = default)
    {
        try
        {
            using var client = url.GetClient();
            var response = await client.GetAsync(parameters);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return default;
        }
        catch
        {
            return default;
        }
    }
}