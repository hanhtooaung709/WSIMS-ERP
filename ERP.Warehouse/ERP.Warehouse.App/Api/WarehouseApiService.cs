using ERP.Warehouse.App.Api.Endpoints.SignIn;
using ERP.Warehouse.Models.Models.Signin;
using ERP.Warehouse.Models.Models.Signin.Signin;
using WSIMS_ERP.Shared.HttpClients;
using WSIMS_ERP.Shared.Models;

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

    #region SignIn

    public async Task<Result<SigninResModel>> SignIn(SigninReqModel reqModel)
        => await _httpClientService.ExecuteAsync<SigninReqModel, SigninResModel>
        (SignInEndpoints.SignIn, reqModel);

    public async Task<Result<WarehouseUserInfoListModel>> GetUserData()
        => await _httpClientService.ExecuteAsync<object, WarehouseUserInfoListModel>
        (SignInEndpoints.GetUserData, null!);

    #endregion
}
