using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared.Models.DynamicModel;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using WSIMS_ERP.Shared.Models.ConfigModel;

namespace ERP.Warehouse.Api.Features.ApproveProduct.ApproveReqProduct;

public class ApproveReqProductService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;
    private readonly CustomSettingModel _setting;

    public ApproveReqProductService(IHttpContextAccessor httpContextAccessor,
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

    public async Task<Result<ReqProductRepModel>> Get(ReqProductReqModel reqModel)
    {
        ReqProductRepModel model = new();
        try
        {
            IQueryable<TblReqProduct>? product = _db.TblReqProducts
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

    public async Task<Result<ReqProductModel>> Approve(ReqProductEditModel reqModel)
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
                model = Result<ReqProductModel>.Error(JsonResource.WHE063);
                return model;
            }

            bool reqProduct = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductId == reqModel.ReqProductId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqProduct)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE064);
                return model;
            }

            #endregion

            #region Prepare Data

            var imagePath = "";
            if (!product.ImagePath.IsNullOrEmpty() && File.Exists(product.ImagePath))
            {
                var imgFolder = _setting.Image.Product;
                Directory.CreateDirectory(imgFolder);
                var fileName = DevCode.GenerateUlid() + ".jpg";
                imagePath = Path.Combine(imgFolder, fileName);
                File.Copy(product.ImagePath, imagePath);
            }

            var productid = DevCode.GenerateUlid();

            TblProduct item = new TblProduct
            {
                ProductId = productid,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                SupplierName = product.SupplierName,
                ImagePath = imagePath,
                CreatedUserId = AuthorizedUserId,
                CreatedDateTime = DevCode.GetServerDateTime()
            };

            await _db.TblProducts.AddAsync(item);
            await _db.SaveChangesAsync();

            product.Status = EnumRequestedStatus.Approved.ToString();
            product.ProductId = productid;
            product.ApprovedUserId = AuthorizedUserId;
            product.ApprovedDateTime = DevCode.GetServerDateTime();
            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            model = Result<ReqProductModel>.Success(JsonResource.WHS065);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductModel>.Error(ex);
        }

        return model;
    }

    public async Task<Result<ReqProductModel>> Reject(ReqProductEditModel reqModel)
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
                model = Result<ReqProductModel>.Error(JsonResource.WHE063);
                return model;
            }

            bool reqProduct = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductId == reqModel.ReqProductId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqProduct)
            {
                model = Result<ReqProductModel>.Error(JsonResource.WHE064);
                return model;
            }

            #endregion

            #region Prepare Data

            product!.Status = EnumRequestedStatus.Rejected.ToString();
            product.ApprovedUserId = AuthorizedUserId;
            product.ApprovedDateTime = DevCode.GetServerDateTime();
            product.RejectReason = reqModel.RejectReason;
            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            model = Result<ReqProductModel>.Success(JsonResource.WHS066);

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
