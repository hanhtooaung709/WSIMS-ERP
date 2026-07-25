using ERP.Warehouse.App.Api.Endpoints.ApprovePackage;
using ERP.Warehouse.App.Api.Endpoints.ApproveProduct;
using ERP.Warehouse.App.Api.Endpoints.Box;
using ERP.Warehouse.App.Api.Endpoints.Branch;
using ERP.Warehouse.App.Api.Endpoints.Currency;
using ERP.Warehouse.App.Api.Endpoints.Package;
using ERP.Warehouse.App.Api.Endpoints.Product;
using ERP.Warehouse.App.Api.Endpoints.SignIn;
using ERP.Warehouse.App.Api.Endpoints.WarehouseUser;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.Branch;
using ERP.Warehouse.Models.Models.Currency;
using ERP.Warehouse.Models.Models.Package.PackageList;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using ERP.Warehouse.Models.Models.Signin;
using ERP.Warehouse.Models.Models.Signin.Signin;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUserChanges;
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

    #region ReqWarehouseUserChanges

    public async Task<Result<ReqWarehouseUserChangesRepModel>> Get(ReqWarehouseUserChangesReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserChangesReqModel, ReqWarehouseUserChangesRepModel>
        (ReqWarehouseUserChangesEndpoints.Get, reqModel);

    public async Task<Result<ReqWarehouseUserChangesModel>> Edit(ReqWarehouseUserChangesEditModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserChangesEditModel, ReqWarehouseUserChangesModel>
        (ReqWarehouseUserChangesEndpoints.Edit, reqModel);

    public async Task<Result<ReqWarehouseUserChangesModel>> Update(ReqWarehouseUserChangesReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserChangesReqModel, ReqWarehouseUserChangesModel>
        (ReqWarehouseUserChangesEndpoints.Update, reqModel);

    public async Task<Result<ReqWarehouseUserChangesModel>> Delete(ReqWarehouseUserChangesEditModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserChangesEditModel, ReqWarehouseUserChangesModel>
        (ReqWarehouseUserChangesEndpoints.Delete, reqModel);

    public async Task<Result<ReqWarehouseUserChangesDetailsModel>> Details(ReqWarehouseUserChangesEditModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqWarehouseUserChangesEditModel, ReqWarehouseUserChangesDetailsModel>
        (ReqWarehouseUserChangesEndpoints.Details, reqModel);

    #endregion

    #region Branch

    public async Task<Result<BranchRepModel>> Get()
        => await _httpClientService.ExecuteAsync<object, BranchRepModel>
        (BranchEndpoints.Get, null!);

    #endregion

    #region Box

    public async Task<Result<BoxRepModel>> Get(BoxReqModel reqModel)
        => await _httpClientService.ExecuteAsync<BoxReqModel, BoxRepModel>
        (BoxEndpoints.Get, reqModel);

    public async Task<Result<BoxModel>> Create(BoxReqModel reqModel)
        => await _httpClientService.ExecuteAsync<BoxReqModel, BoxModel>
        (BoxEndpoints.Create, reqModel);

    public async Task<Result<BoxModel>> Edit(BoxEditModel reqModel)
       => await _httpClientService.ExecuteAsync<BoxEditModel, BoxModel>
       (BoxEndpoints.Edit, reqModel);

    public async Task<Result<BoxModel>> Update(BoxReqModel reqModel)
       => await _httpClientService.ExecuteAsync<BoxReqModel, BoxModel>
       (BoxEndpoints.Update, reqModel);

    public async Task<Result<BoxModel>> Delete(BoxEditModel reqModel)
       => await _httpClientService.ExecuteAsync<BoxEditModel, BoxModel>
       (BoxEndpoints.Delete, reqModel);

    public async Task<Result<BoxDetailModel>> Details(BoxEditModel reqModel)
       => await _httpClientService.ExecuteAsync<BoxEditModel, BoxDetailModel>
       (BoxEndpoints.Details, reqModel);

    #endregion

    #region Currency

    public async Task<Result<CurrencyRepModel>> Get(CurrencyReqModel reqModel)
        => await _httpClientService.ExecuteAsync<CurrencyReqModel, CurrencyRepModel>
        (CurrencyEndpoints.Get, reqModel);

    public async Task<Result<CurrencyModel>> Create(CurrencyReqModel reqModel)
        => await _httpClientService.ExecuteAsync<CurrencyReqModel, CurrencyModel>
        (CurrencyEndpoints.Create, reqModel);

    public async Task<Result<CurrencyModel>> Edit(CurrencyEditModel reqModel)
       => await _httpClientService.ExecuteAsync<CurrencyEditModel, CurrencyModel>
       (CurrencyEndpoints.Edit, reqModel);

    public async Task<Result<CurrencyModel>> Update(CurrencyReqModel reqModel)
       => await _httpClientService.ExecuteAsync<CurrencyReqModel, CurrencyModel>
       (CurrencyEndpoints.Update, reqModel);

    public async Task<Result<CurrencyModel>> Delete(CurrencyEditModel reqModel)
       => await _httpClientService.ExecuteAsync<CurrencyEditModel, CurrencyModel>
       (CurrencyEndpoints.Delete, reqModel);

    public async Task<Result<CurrencyDetailModel>> Details(CurrencyEditModel reqModel)
       => await _httpClientService.ExecuteAsync<CurrencyEditModel, CurrencyDetailModel>
       (CurrencyEndpoints.Details, reqModel);

    #endregion

    #region Product

    public async Task<Result<ProductRepModel>> Get(ProductReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ProductReqModel, ProductRepModel>
        (ProductEndpoints.Get, reqModel);

    public async Task<Result<ProductModel>> Create(ProductReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ProductReqModel, ProductModel>
        (ProductEndpoints.Create, reqModel);

    public async Task<Result<ProductModel>> Edit(ProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ProductEditModel, ProductModel>
       (ProductEndpoints.Edit, reqModel);

    public async Task<Result<ProductModel>> Update(ProductReqModel reqModel)
       => await _httpClientService.ExecuteAsync<ProductReqModel, ProductModel>
       (ProductEndpoints.Update, reqModel);

    public async Task<Result<ProductModel>> Delete(ProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ProductEditModel, ProductModel>
       (ProductEndpoints.Delete, reqModel);

    public async Task<Result<ProductDetailModel>> Details(ProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ProductEditModel, ProductDetailModel>
       (ProductEndpoints.Details, reqModel);

    #endregion

    #region ReqProduct

    public async Task<Result<ReqProductRepModel>> Get(ReqProductReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqProductReqModel, ReqProductRepModel>
        (ReqProductEndpoints.Get, reqModel);

    public async Task<Result<ReqProductModel>> Edit(ReqProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductEditModel, ReqProductModel>
       (ReqProductEndpoints.Edit, reqModel);

    public async Task<Result<ReqProductModel>> Update(ReqProductReqModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductReqModel, ReqProductModel>
       (ReqProductEndpoints.Update, reqModel);

    public async Task<Result<ReqProductModel>> Delete(ReqProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductEditModel, ReqProductModel>
       (ReqProductEndpoints.Delete, reqModel);

    public async Task<Result<ReqProductDetailModel>> Details(ReqProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductEditModel, ReqProductDetailModel>
       (ReqProductEndpoints.Details, reqModel);

    #endregion

    #region ReqProductChanges

    public async Task<Result<ReqProductChangesRepModel>> Get(ReqProductChangesReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqProductChangesReqModel, ReqProductChangesRepModel>
        (ReqProductChangesEndpoints.Get, reqModel);

    public async Task<Result<ReqProductChangesModel>> Edit(ReqProductChangesEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductChangesEditModel, ReqProductChangesModel>
       (ReqProductChangesEndpoints.Edit, reqModel);

    public async Task<Result<ReqProductChangesModel>> Update(ReqProductChangesReqModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductChangesReqModel, ReqProductChangesModel>
       (ReqProductChangesEndpoints.Update, reqModel);

    public async Task<Result<ReqProductChangesModel>> Delete(ReqProductChangesEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductChangesEditModel, ReqProductChangesModel>
       (ReqProductChangesEndpoints.Delete, reqModel);

    public async Task<Result<ReqProductChangesDetailModel>> Details(ReqProductChangesEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductChangesEditModel, ReqProductChangesDetailModel>
       (ReqProductChangesEndpoints.Details, reqModel);

    #endregion

    #region ApproveReqProduct

    public async Task<Result<ReqProductRepModel>> GetApproveReqProduct(ReqProductReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqProductReqModel, ReqProductRepModel>
        (ApproveReqProductEndpoints.Get, reqModel);

    public async Task<Result<ReqProductModel>> Approve(ReqProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductEditModel, ReqProductModel>
       (ApproveReqProductEndpoints.Approve, reqModel);

    public async Task<Result<ReqProductModel>> Reject(ReqProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductEditModel, ReqProductModel>
       (ApproveReqProductEndpoints.Reject, reqModel);

    public async Task<Result<ReqProductDetailModel>> ApproveReqProductDetails(ReqProductEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductEditModel, ReqProductDetailModel>
       (ApproveReqProductEndpoints.Details, reqModel);

    #endregion

    #region ApproveReqProductChanges

    public async Task<Result<ReqProductChangesRepModel>> GetApproveReqProduct(ReqProductChangesReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqProductChangesReqModel, ReqProductChangesRepModel>
        (ApproveReqProductChangesEndpoints.Get, reqModel);

    public async Task<Result<ReqProductChangesModel>> Approve(ReqProductChangesEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductChangesEditModel, ReqProductChangesModel>
       (ApproveReqProductChangesEndpoints.Approve, reqModel);

    public async Task<Result<ReqProductChangesModel>> Reject(ReqProductChangesEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductChangesEditModel, ReqProductChangesModel>
       (ApproveReqProductChangesEndpoints.Reject, reqModel);

    public async Task<Result<ReqProductChangesDetailModel>> ApproveReqProductDetails(ReqProductChangesEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqProductChangesEditModel, ReqProductChangesDetailModel>
       (ApproveReqProductChangesEndpoints.Details, reqModel);

    #endregion

    #region Package

    public async Task<Result<PackageRepModel>> Get(PackageReqModel reqModel)
        => await _httpClientService.ExecuteAsync<PackageReqModel, PackageRepModel>
        (PackageEndpoints.Get, reqModel);

    public async Task<Result<PackageModel>> Create(PackageReqModel reqModel)
       => await _httpClientService.ExecuteAsync<PackageReqModel, PackageModel>
       (PackageEndpoints.Create, reqModel);

    public async Task<Result<PackageModel>> Edit(PackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<PackageEditModel, PackageModel>
       (PackageEndpoints.Edit, reqModel);

    public async Task<Result<PackageModel>> Update(PackageReqModel reqModel)
       => await _httpClientService.ExecuteAsync<PackageReqModel, PackageModel>
       (PackageEndpoints.Update, reqModel);

    public async Task<Result<PackageModel>> Delete(PackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<PackageEditModel, PackageModel>
       (PackageEndpoints.Delete, reqModel);

    public async Task<Result<PackageDetailModel>> Details(PackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<PackageEditModel, PackageDetailModel>
       (PackageEndpoints.Details, reqModel);

    public async Task<Result<List<BranchResponseModel>>> GetOtherBranch()
       => await _httpClientService.ExecuteAsync<object, List<BranchResponseModel>>
       (PackageEndpoints.GetBranch, null!);

    #endregion

    #region ReqPackage

    public async Task<Result<ReqPackageRepModel>> Get(ReqPackageReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqPackageReqModel, ReqPackageRepModel>
        (ReqPackageEndpoints.Get, reqModel);

    public async Task<Result<ReqPackageModel>> Edit(ReqPackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageEditModel, ReqPackageModel>
       (ReqPackageEndpoints.Edit, reqModel);

    public async Task<Result<ReqPackageModel>> Update(ReqPackageReqModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageReqModel, ReqPackageModel>
       (ReqPackageEndpoints.Update, reqModel);

    public async Task<Result<ReqPackageModel>> Delete(ReqPackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageEditModel, ReqPackageModel>
       (ReqPackageEndpoints.Delete, reqModel);

    public async Task<Result<ReqPackageDetailModel>> Details(ReqPackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageEditModel, ReqPackageDetailModel>
       (ReqPackageEndpoints.Details, reqModel);

    public async Task<Result<List<ProductResponseModel>>> GetProduct()
       => await _httpClientService.ExecuteAsync<object, List<ProductResponseModel>>
       (ReqPackageEndpoints.GetProduct, null!);

    public async Task<Result<List<CurrencyResponseModel>>> GetCurrency()
       => await _httpClientService.ExecuteAsync<object, List<CurrencyResponseModel>>
       (ReqPackageEndpoints.GetCurrency, null!);

    public async Task<Result<List<BoxResponseModel>>> GetBox()
       => await _httpClientService.ExecuteAsync<object, List<BoxResponseModel>>
       (ReqPackageEndpoints.GetBox, null!);

    #endregion

    #region ReqPackageChange

    public async Task<Result<ReqPackageChangeRepModel>> Get(ReqPackageChangeReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqPackageChangeReqModel, ReqPackageChangeRepModel>
        (ReqPackageChangeEndpoints.Get, reqModel);

    public async Task<Result<ReqPackageChangeModel>> Edit(ReqPackageChangeEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageChangeEditModel, ReqPackageChangeModel>
       (ReqPackageChangeEndpoints.Edit, reqModel);

    public async Task<Result<ReqPackageChangeModel>> Update(ReqPackageChangeReqModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageChangeReqModel, ReqPackageChangeModel>
       (ReqPackageChangeEndpoints.Update, reqModel);

    public async Task<Result<ReqPackageChangeModel>> Delete(ReqPackageChangeEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageChangeEditModel, ReqPackageChangeModel>
       (ReqPackageChangeEndpoints.Delete, reqModel);

    public async Task<Result<ReqPackageChangeDetailModel>> Details(ReqPackageChangeEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageChangeEditModel, ReqPackageChangeDetailModel>
       (ReqPackageChangeEndpoints.Details, reqModel);

    #endregion

    #region ApproveReqPackage

    public async Task<Result<ReqPackageRepModel>> GetApproveReqPackage(ReqPackageReqModel reqModel)
        => await _httpClientService.ExecuteAsync<ReqPackageReqModel, ReqPackageRepModel>
        (ApproveReqPackageEndpoints.Get, reqModel);

    public async Task<Result<ReqPackageModel>> Approve(ReqPackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageEditModel, ReqPackageModel>
       (ApproveReqPackageEndpoints.Approve, reqModel);

    public async Task<Result<ReqPackageModel>> Reject(ReqPackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageEditModel, ReqPackageModel>
       (ApproveReqPackageEndpoints.Reject, reqModel);

    public async Task<Result<ReqPackageDetailModel>> ApproveReqPackageDetails(ReqPackageEditModel reqModel)
       => await _httpClientService.ExecuteAsync<ReqPackageEditModel, ReqPackageDetailModel>
       (ApproveReqPackageEndpoints.Details, reqModel);

    #endregion
}
