using System.Text.Json;
using WSIMS_ERP.Shared.HttpClients;

namespace ERP.Warehouse.App.Api;

public class WarehouseApiService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClientService _httpClientService;

    public WarehouseApiService(IHttpContextAccessor httpContextAccessor,
        HttpClientService httpClientService)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClientService = httpClientService;
    }
}
