using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using WSIMS_ERP.Shared.Models.DynamicModel;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using WSIMS_ERP.Shared.Models.ConfigModel;

namespace ERP.Warehouse.Api.Features.Product.ReqProduct;

public class ReqProductService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;
    private readonly CustomSettingModel _setting;

    public ReqProductService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger,
        CustomSettingModel setting) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
        _setting = setting;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<ReqProductRepModel>> Get(ReqProductReqModel reqModel)
    {
        ReqProductRepModel model = new();
        try
        {
            IQueryable<TblReqProduct>? product = _db.TblReqProducts
                .AsNoTracking()
                .Where(x => x.ReqUserId == AuthorizedUserId);

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
                    .Select(x => new ReqProductModel
                    {
                        ReqProductId = x.ReqProductId,
                        ProductName = x.ProductName,
                        ProductCode = x.ProductCode,
                        SupplierName = x.SupplierName,
                        Status = x.Status
                    })
                    .ToListAsync();

                model.list = result;
                return Result<ReqProductRepModel>.Success(model);

                #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductRepModel>.Error(ex);
        }
        
     }

    public async Task<Result<ReqProductModel>> Edit(ReqProductEditModel reqModel)
    {
        var model = new Result<ReqProductModel>();
        try
        {
            #region Check ReqProduct

            var product = await _db.TblReqProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqProductId == reqModel.ReqProductId);
            if (product is null)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE038);
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new ReqProductModel
            {
                ReqProductId = product.ReqProductId,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                SupplierName = product.SupplierName,
                ImagePath = product.ImagePath,
            };
            model = Result<ReqProductModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqProductModel>> Update(ReqProductReqModel reqModel)
    {
        var model = new Result<ReqProductModel>();
        try
        {
            #region Check Product

            TblReqProduct? product = await _db.TblReqProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqProductId == reqModel.ReqProductId);
            if (product is null)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE038);
                return model;
            }

            bool reqProduct = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductId == reqModel.ReqProductId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqProduct)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE039);
                return model;
            }
            #endregion

            #region Check Duplicate Product Name

            bool userName = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower());
            if (userName)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE029);
                return model;
            }

            userName = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.ReqProductId != reqModel.ReqProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE030);
                return model;
            }

            userName = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE031);
                return model;
            }

            #endregion

            #region Check Duplicate Product code

            bool code = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE032);
                return model;
            }

            code = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.ReqProductId != reqModel.ReqProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (code)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE033);
                return model;
            }

            code = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (code)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE034);
                return model;
            }

            #endregion

            #region Prepare Data

            var imagePath = "";
            if (!reqModel.ImageData.IsNullOrEmpty())
            {
                if (!product.ImagePath.IsNullOrEmpty() && File.Exists(product.ImagePath))
                {
                    File.Delete(product.ImagePath);
                }
                var imgFolder = _setting.Image.ReqProduct;
                Directory.CreateDirectory(imgFolder);
                var fileName = product.ReqProductId + ".jpg";
                imagePath = Path.Combine(imgFolder, fileName);
                await DevCode.WriteBase64ToFileAsync(reqModel.ImageData, imagePath);
            }

            product.ProductName = reqModel.ProductName!;
            product.ProductCode = reqModel.ProductCode!;
            product.SupplierName = reqModel.SupplierName!;
            product.ImagePath = imagePath;
            product.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(product).State = EntityState.Modified;
            _db.TblReqProducts.Update(product);
            await _db.SaveChangesAsync();
            model = Result<ReqProductModel>.Success(JsonResource.WHS040);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqProductModel>> Delete(ReqProductEditModel reqModel)
    {
        var model = new Result<ReqProductModel>();
        try
        {
            #region Check Product

            var product = await _db.TblReqProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqProductId == reqModel.ReqProductId);
            if (product is null)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE038);
                return model;
            }

            bool reqUser = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductId == reqModel.ReqProductId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE041);
                return model;
            }

            #endregion

            #region Prepare Data

            if (!product.ImagePath.IsNullOrEmpty() && File.Exists(product.ImagePath))
            {
                File.Delete(product.ImagePath);
            }

            _db.TblReqProducts.Remove(product);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE042);
                return model;
            }
            model = Result<ReqProductModel>.Success(JsonResource.WHS043);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqProductDetailModel>> Details(ReqProductEditModel reqModel)
    {
        ReqProductDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<ReqProductDetailInfoModel>(
                SqlQueries.Sp_GetReqProductDetail, new
                {
                    ReqProductId = reqModel.ReqProductId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> productInfo = new List<DynamicReportModel>();
            productInfo.Add("Product Name", detail.ProductName!);
            productInfo.Add("Product Code", detail.ProductCode!);
            productInfo.Add("Supplier Name", detail.SupplierName!);
            model.ProductInfo = productInfo;
            model.ItemImagePath = detail.ImagePath;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("Requested User", detail.ReqUser!);
            makerChecker.Add("Requested DateTime", detail.ReqDateTime!);
            makerChecker.Add("Approved User", detail.ApprovedUser!.ToDashFromNull());
            makerChecker.Add("Approved DateTime ", detail.ApprovedDateTime!.ToDashFromNull());
            makerChecker.Add("Status", detail.Status!);
            makerChecker.Add("Reject Reason", detail.RejectReason!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<ReqProductDetailModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqProductDetailModel>.Error(ex);
        }
    }

    #endregion
}
