using ERP.Warehouse.App.Api.Endpoints;
using ERP.Warehouse.Models.Models.Signin;
using ERP.Warehouse.Models.Models.Signin.Signin;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
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

    public async Task<Result<WarehouseUserModel>> Create(WarehouseUserReqModel reqModel)
        => await _httpClientService.ExecuteAsync<WarehouseUserReqModel, WarehouseUserModel>
        (WarehouseUserListEndpoints.Create, reqModel);

    public async Task<Result<WarehouseUserModel>> Edit(WarehouseUserEditModel reqModel)
       => await _httpClientService.ExecuteAsync<WarehouseUserEditModel, WarehouseUserModel>
       (WarehouseUserListEndpoints.Edit, reqModel);

    public async Task<Result<WarehouseUserModel>> Update(WarehouseUserReqModel reqModel)
       => await _httpClientService.ExecuteAsync<WarehouseUserReqModel, WarehouseUserModel>
       (WarehouseUserListEndpoints.Update, reqModel);

    public async Task<Result<WarehouseUserModel>> Delete(WarehouseUserEditModel reqModel)
       => await _httpClientService.ExecuteAsync<WarehouseUserEditModel, WarehouseUserModel>
       (WarehouseUserListEndpoints.Delete, reqModel);

    public async Task<Result<WarehouseUserDetailsModel>> Details(WarehouseUserEditModel reqModel)
       => await _httpClientService.ExecuteAsync<WarehouseUserEditModel, WarehouseUserDetailsModel>
       (WarehouseUserListEndpoints.Details, reqModel);

    public async Task<Result<List<RoleResponseModel>>> GetRole()
       => await _httpClientService.ExecuteAsync<object, List<RoleResponseModel>>
       (WarehouseUserListEndpoints.GetRole, null!);

    public async Task<Result<List<BranchResponseModel>>> GetBranch()
       => await _httpClientService.ExecuteAsync<object, List<BranchResponseModel>>
       (WarehouseUserListEndpoints.GetBranch, null!);

    #endregion

    #region ReqWarehouseUser

    public async Task<Result<ReqWarehouseUserRepModel>> Get(ReqWarehouseUserReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserReqModel, ReqWarehouseUserRepModel>
        (ReqWarehouseUserEndpoints.Get, reqModel);

    public async Task<Result<ReqWarehouseUserModel>> Edit(ReqWarehouseUserEditModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserEditModel, ReqWarehouseUserModel>
        (ReqWarehouseUserEndpoints.Edit, reqModel);

    public async Task<Result<ReqWarehouseUserModel>> Update(ReqWarehouseUserReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserReqModel, ReqWarehouseUserModel>
        (ReqWarehouseUserEndpoints.Update, reqModel);

    public async Task<Result<ReqWarehouseUserModel>> Delete(ReqWarehouseUserEditModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserEditModel, ReqWarehouseUserModel>
        (ReqWarehouseUserEndpoints.Delete, reqModel);

    public async Task<Result<ReqWarehouseUserDetailsModel>> Details(ReqWarehouseUserEditModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserEditModel, ReqWarehouseUserDetailsModel>
        (ReqWarehouseUserEndpoints.Details, reqModel);

    #endregion
}
