using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.HttpService;

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient();
    }

    public async Task<TResponse> GetAsync<TResponse>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<TResponse>(jsonString, Options);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var response = await _httpClient.PostAsync(url, CreateJsonRequestContent(data)).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<TResponse>(jsonString, Options);
    }

    public async Task PutAsync<TRequest>(string url, TRequest data)
    {
        var response = await _httpClient.PutAsync(url, CreateJsonRequestContent(data)).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    private static HttpContent CreateJsonRequestContent<T>(T content) =>
        new StringContent(JsonSerializer.Serialize(content, Options), Encoding.UTF8, "application/json");

    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,  
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
        }
    };
}


