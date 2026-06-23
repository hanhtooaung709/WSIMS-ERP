using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using WSIMS_ERP.Shared.Models;

namespace WSIMS_ERP.Shared.HttpClients;

public class HttpClientService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;
    private readonly System.Text.Json.JsonSerializerOptions _jsonOptions;

    public HttpClientService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClient;
        _jsonOptions = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<Result<TResponse>> ExecuteAsync<TRequest, TResponse>(string endpoint, TRequest reqModel)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var jsonStr = System.Text.Json.JsonSerializer.Serialize(reqModel);
            var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(responseString))
                {
                    var root = doc.RootElement;

                    int respType = 0;
                    if (root.TryGetProperty("respType", out JsonElement respTypeElement) ||
                        root.TryGetProperty("RespType", out respTypeElement))
                    {
                        respType = respTypeElement.GetInt32();
                    }

                    string respDesp = "";
                    if (root.TryGetProperty("respDesp", out JsonElement respDespElement) ||
                        root.TryGetProperty("RespDesp", out respDespElement))
                    {
                        respDesp = respDespElement.GetString() ?? "";
                    }

                    if (respType == 1)
                    {
                        return Result<TResponse>.Error(respDesp);
                    }

                    var data = System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseString, _jsonOptions);
                    return Result<TResponse>.Success(data, respDesp);
                }
            }

            return Result<TResponse>.Error($"API Error: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return Result<TResponse>.Error($"Internal Exception: {ex.Message}");
        }
    }
}