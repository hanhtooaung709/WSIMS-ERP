using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Stock;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models.DynamicModel;
using System.Data;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.ConfigModel;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using WSIMS_ERP.Shared;

namespace ERP.Warehouse.Api.Features.ApproveStock;

public class ApproveStockService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;
    private readonly CustomSettingModel _setting;

    public ApproveStockService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
        _setting = setting;
    }

    #region Get/Approve/Reject/Details

    public async Task<Result<StockRepModel>> Get(StockReqModel reqModel)
    {
        StockRepModel model = new();
        try
        {
            #region Check User

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == AuthorizedUserId);
            if (user is null)
            {
                return Result<StockRepModel>.Error(JsonResource.WHE001);
            }

            #endregion

            var parameters = new
            {
                CurrentUserId = user.WarehouseUserId,
                PackageName = reqModel.PackageName,
                ProductName = reqModel.ProductCode,
                Box = reqModel.BoxCode,
                Branch = user.BranchCode,
                Status = reqModel.Status
            };
            var result = await _dapperService.QueryStoredProcedureAsync<StockModel>
                (SqlQueries.Sp_GetApproveStockList, parameters);
            model.list = result;
            return Result<StockRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<StockRepModel>.Error(ex);
        }
    }

    public async Task<Result<StockDetailModel>> Details(StockEditModel reqModel)
    {
        StockDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<StockDetailInfoModel>(
                SqlQueries.Sp_GetStockDetail, new
                {
                    ReqPackageId = reqModel.ReqPackageId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> packageInfo = new List<DynamicReportModel>();
            packageInfo.Add("Package Name", detail.PackageName!);
            packageInfo.Add("Product Name", detail.ProductName!);
            packageInfo.Add("PackageInfo Code", detail.PackageInfoCode!);
            packageInfo.Add("Branch Name", detail.BranchName!);
            packageInfo.Add("Quantity", detail.Quantity!);
            packageInfo.Add("Price", detail.Price!);
            packageInfo.Add("Currency Code", detail.CurrencyCode!);
            packageInfo.Add("Weight", detail.Weight!);
            packageInfo.Add("Box", detail.Box!);
            packageInfo.Add("Changes Type", detail.ChangesType!);
            model.Package = packageInfo;
            model.ItemImagePath = detail.ImagePath;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("Requested User", detail.ReqUser!);
            makerChecker.Add("Requested DateTime", detail.ReqDateTime!);
            makerChecker.Add("Approved User", detail.ApprovedUser!.ToDashFromNull());
            makerChecker.Add("Approved DateTime", detail.ApprovedDateTime!.ToDashFromNull());
            makerChecker.Add("Status", detail.Status!);
            makerChecker.Add("Reject Reason", detail.RejectReason!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<StockDetailModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<StockDetailModel>.Error(ex);
        }
    }

    #endregion
}
