using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers; // MediaTypeHeaderValue အတွက် ထည့်ပေးရပါမည်
using System.Text;
using System.Threading.Tasks;
using WSIMS_ERP.Shared.Models;

namespace WSIMS_ERP.Shared.HttpClients;

public class HttpClientService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;
    private readonly System.Text.Json.JsonSerializerOptions _jsonOptions; // အတိအကျ သတ်မှတ်ပေးခြင်း

    // HttpClient ကို တိုက်ရိုက် Inject လုပ်ယူပါသည်
    public HttpClientService(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClient;
        _jsonOptions = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    private async Task<Result<TResponse>> ExecuteAsync<TRequest, TResponse>(string endpoint, TRequest reqModel)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            var jsonStr = System.Text.Json.JsonSerializer.Serialize(reqModel);

            var content = new StringContent(jsonStr, Encoding.UTF8);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();

                var data = System.Text.Json.JsonSerializer.Deserialize<TResponse>(responseString, _jsonOptions);
                return Result<TResponse>.Success(data);
            }

            return Result<TResponse>.Error($"API Error: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return Result<TResponse>.Error($"Internal Exception: {ex.Message}");
        }
    }
}