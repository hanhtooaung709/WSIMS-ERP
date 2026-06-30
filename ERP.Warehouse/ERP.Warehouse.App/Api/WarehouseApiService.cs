using ERP.Warehouse.App.Api.Endpoints;
using ERP.Warehouse.Models.Models.Signin;
using ERP.Warehouse.Models.Models.Signin.Signin;
using ERP.Warehouse.Models.Models.WarehouseUserList;
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

    public async Task<Result<WarehouseUserInfoModel>> GetUserData()
        => await _httpClientService.ExecuteAsync<object, WarehouseUserInfoModel>
        (SignInEndpoints.GetUserData, null!);

    public async Task<Result<bool>> Logout(LogoutReqModel reqModel)
        => await _httpClientService.ExecuteAsync<LogoutReqModel, bool>
        (SignInEndpoints.Logout, reqModel);

    #endregion

    #region WarehouseUserList

    public async Task<Result<WarehouseUserRepModel>> Get(WarehouseUserReqModel reqModel)
        => await _httpClientService.ExecuteAsync<WarehouseUserReqModel, WarehouseUserRepModel>
        (WarehouseUserListEndpoints.Get, reqModel);

    public async Task<Result<WarehouseUserModel>> Edit(WarehouseUserEditModel reqModel)
       => await _httpClientService.ExecuteAsync<WarehouseUserEditModel, WarehouseUserModel>
       (WarehouseUserListEndpoints.Edit, reqModel);

    #endregion
}
