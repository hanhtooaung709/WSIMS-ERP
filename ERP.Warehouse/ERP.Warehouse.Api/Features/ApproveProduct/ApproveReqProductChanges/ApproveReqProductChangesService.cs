using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models.DynamicModel;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using WSIMS_ERP.Shared.Enums;

namespace ERP.Warehouse.Api.Features.ApproveProduct.ApproveReqProductChanges;

public class ApproveReqProductChangesService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ApproveReqProductChangesService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Approve/Reject/Details

    public async Task<Result<ReqProductChangesRepModel>> Get(ReqProductChangesReqModel reqModel)
    {
        ReqProductChangesRepModel model = new();
        try
        {
            IQueryable<TblReqProductChange>? product = _db.TblReqProductChanges
                .AsNoTracking()
                .Where(x => x.ReqUserId != AuthorizedUserId);


            #region Filters

            if (!reqModel.ProductName.IsNullOrEmpty())
            {
                product = product.Where(x => x.ProductName.ToUpper().Trim() == reqModel.ProductName!.ToUpper().Trim());
            }
            if (!reqModel.ProductCode.IsNullOrEmpty())
            {
                product = product.Where(x => x.ProductName.ToUpper().Trim() == reqModel.ProductCode!.ToUpper().Trim());
            }
            if (!reqModel.SupplierName.IsNullOrEmpty())
            {
                product = product.Where(x => x.SupplierName.ToUpper().Trim() == reqModel.SupplierName!.ToUpper().Trim());
            }
            if (!reqModel.Status.IsNullOrEmpty())
            {
                product = product.Where(x => x.Status.ToUpper().Trim() == reqModel.Status!.ToUpper().Trim());
            }
            #endregion

            #region Prepare Data

            var result = await product
                    .AsNoTracking()
                    .Select(x => new ReqProductChangesModel
                    {
                        ReqProductChangesId = x.ReqProductChangesId,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        ProductCode = x.ProductCode,
                        SupplierName = x.SupplierName,
                        ChangesType = x.ChangesType,
                        Status = x.Status
                    })
                    .ToListAsync();

            model.list = result;
            return Result<ReqProductChangesRepModel>.Success(model);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductChangesRepModel>.Error(ex);
        }

    }

    public async Task<Result<ReqProductChangesModel>> Approve(ReqProductChangesEditModel reqModel)
    {
        var model = new Result<ReqProductChangesModel>();
        try
        {
            #region Check Product

            TblProduct? product = await _db.TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == reqModel.ProductId);
            if (product is null)
            {
                model = Result<ReqProductChangesModel>.Error(JsonResource.WHE067);
                return model;
            }

            TblReqProductChange? productChanges = await _db.TblReqProductChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqProductChangesId == reqModel.ReqProductChangesId);
            if (product is null)
            {
                model = Result<ReqProductChangesModel>.Error(JsonResource.WHE068);
                return model;
            }

            bool reqProduct = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductChangesId == reqModel.ReqProductChangesId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqProduct)
            {
                model = Result<ReqProductChangesModel>.Error(JsonResource.WHE069);
                return model;
            }

            #endregion

            #region Prepare Data

            if(productChanges!.ChangesType == EnumRequestedType.Update.ToString())
            {
                product.ProductName = productChanges.ProductName!;
                product.ProductCode = productChanges.ProductCode!;
                product.SupplierName = productChanges.SupplierName!;
                product.ModifiedUserId = AuthorizedUserId;
                product.ModifiedDateTime = DevCode.GetServerDateTime();
                _db.Entry(product).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            else
            {
                product.DelFlag = 1;
                _db.Entry(product).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }

            productChanges.Status = EnumRequestedStatus.Approved.ToString();
            productChanges.ApprovedUserId = AuthorizedUserId;
            productChanges.ApprovedDateTime = DevCode.GetServerDateTime();
            _db.Entry(productChanges).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            model = Result<ReqProductChangesModel>.Success(JsonResource.WHS070);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductChangesModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ReqProductChangesModel>> Reject(ReqProductChangesEditModel reqModel)
    {
        var model = new Result<ReqProductChangesModel>();
        try
        {
            #region Check Product

            TblProduct? product = await _db.TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == reqModel.ProductId);
            if (product is null)
            {
                model = Result<ReqProductChangesModel>.Error(JsonResource.WHE067);
                return model;
            }

            TblReqProductChange? productChanges = await _db.TblReqProductChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqProductChangesId == reqModel.ReqProductChangesId);
            if (product is null)
            {
                model = Result<ReqProductChangesModel>.Error(JsonResource.WHE068);
                return model;
            }

            bool reqProduct = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductChangesId == reqModel.ReqProductChangesId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqProduct)
            {
                model = Result<ReqProductChangesModel>.Error(JsonResource.WHE069);
                return model;
            }

            #endregion

            #region Prepare Data

            productChanges!.Status = EnumRequestedStatus.Rejected.ToString();
            productChanges!.RejectReason = reqModel.RejectReason;
            productChanges.ApprovedUserId = AuthorizedUserId;
            productChanges.ApprovedDateTime = DevCode.GetServerDateTime();
            _db.Entry(productChanges).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            model = Result<ReqProductChangesModel>.Success(JsonResource.WHS071);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductChangesModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ReqProductChangesDetailModel>> Details(ReqProductChangesEditModel reqModel)
    {
        ReqProductChangesDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<ReqProductChangesDetailInfoModel>(
                SqlQueries.Sp_GetReqProductChangesDetail, new
                {
                    ReqProductChangesId = reqModel.ReqProductChangesId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> productInfo = new List<DynamicReportModel>();
            productInfo.Add("Product Name", detail.ProductName!);
            productInfo.Add("Product Code", detail.ProductCode!);
            productInfo.Add("Supplier Name", detail.SupplierName!);
            model.ProductInfo = productInfo;

            List<DynamicReportModel> oldInfo = new List<DynamicReportModel>();
            oldInfo.Add("Product Name", detail.OldName!);
            oldInfo.Add("Product Code", detail.OldCode!);
            oldInfo.Add("Supplier Name", detail.OldSupplierName!);
            oldInfo.Add("Changes Type", detail.ChangesType!);
            model.OldInfo = oldInfo;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("Requested User", detail.ReqUser!);
            makerChecker.Add("Requested DateTime", detail.ReqDateTime!);
            makerChecker.Add("Approved User", detail.ApprovedUser!.ToDashFromNull());
            makerChecker.Add("Approved DateTime ", detail.ApprovedDateTime!.ToDashFromNull());
            makerChecker.Add("Status", detail.Status!);
            makerChecker.Add("Reject Reason", detail.RejectReason!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<ReqProductChangesDetailModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqProductChangesDetailModel>.Error(ex);
        }
    }

    #endregion
}
