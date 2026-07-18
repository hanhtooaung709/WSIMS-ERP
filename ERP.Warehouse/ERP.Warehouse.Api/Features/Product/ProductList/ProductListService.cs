using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.Product.ProductList;
using Microsoft.EntityFrameworkCore;
using WSIMS_ERP.Shared.Models.DynamicModel;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Product.ProductList;

public class ProductListService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ProductListService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<ProductRepModel>> Get(ProductReqModel reqModel)
    {
        ProductRepModel model = new();
        try
        {
            IQueryable<TblProduct>? product = _db.TblProducts
                .AsNoTracking()
                .Where(x => x.DelFlag == 0);

            #region Filters

            if (!reqModel.ProductName.IsNullOrEmpty())
            {
                product = product.Where(x => x.ProductName.ToUpper().Trim() == reqModel.ProductName!.ToUpper().Trim());
            }
            if (!reqModel.ProductName.IsNullOrEmpty())
            {
                product = product.Where(x => x.ProductName.ToUpper().Trim() == reqModel.ProductName!.ToUpper().Trim());
            }
            if (!reqModel.SupplierName.IsNullOrEmpty())
            {
                product = product.Where(x => x.SupplierName.ToUpper().Trim() == reqModel.SupplierName!.ToUpper().Trim());
            }

            #endregion

            #region Prepare Data

            var result = await product
                .AsNoTracking()
                .Select(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductCode = x.ProductCode,
                    SupplierName = x.SupplierName
                })
                .ToListAsync();

            model.list = result;
            return Result<ProductRepModel>.Success(model);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ProductRepModel>.Error(ex);
        }
    }

    public async Task<Result<ProductModel>> Create(ProductReqModel reqModel)
    {
        var model = new Result<ProductModel>();
        try
        {
            #region Check Duplicate Product Name

            bool name = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower());
            if (name)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE029);
                return model;
            }

            name = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower());
            if (name)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE030);
                return model;
            }

            name = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower());
            if (name)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE031);
                return model;
            }

            #endregion

            #region Check Duplicate Product Code

            bool code = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE032);
                return model;
            }

            code = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE033);
                return model;
            }

            code = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE034);
                return model;
            }

            #endregion

            #region Prepare Data

            var imagePath = "";
            if (!reqModel.ImageData.IsNullOrEmpty())
            {
                var imgFolder = @"D:\Website Portfolio\Wholesale & Inventory Management System\Image\ReqProduct";
                Directory.CreateDirectory(imgFolder);
                var fileName = DevCode.GenerateUlid() + ".jpg";
                imagePath = Path.Combine(imgFolder, fileName);
                await DevCode.WriteBase64ToFileAsync(reqModel.ImageData, imagePath);
            }

            TblReqProduct item = new TblReqProduct
            {
                ReqProductId = DevCode.GenerateUlid(),
                ProductName = reqModel.ProductName!,
                ProductCode = reqModel.ProductCode!,
                SupplierName = reqModel.SupplierName!,
                ImagePath = imagePath,
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqProducts.AddAsync(item);
            await _db.SaveChangesAsync();

            model = Result<ProductModel>.Success(JsonResource.WHS014);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ProductModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ProductModel>> Edit(ProductEditModel reqModel)
    {
        var model = new Result<ProductModel>();
        try
        {
            #region Check Procuct

            var product = await _db.TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == reqModel.ProductId && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE035);
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new ProductModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                SupplierName = product.SupplierName,
                ImagePath = product.ImagePath,
            };
            model = Result<ProductModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ProductModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ProductModel>> Update(ProductReqModel reqModel)
    {
        var model = new Result<ProductModel>();
        try
        {
            #region Check Prodcut

            var product = await _db.TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == reqModel.ProductId && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE035);
                return model;
            }

            #endregion

            #region Check Duplicate Id

            bool reqUser = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductId == reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE036);
                return model;
            }

            #endregion

            #region Check Duplicate Name

            bool name = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId);
            if (name)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE029);
                return model;
            }

            name = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE030);
                return model;
            }

            name = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE031);
                return model;
            }

            #endregion

            #region Check Duplicate Code

            bool email = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId);
            if (email)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE032);
                return model;
            }

            email = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE033);
                return model;
            }

            email = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE033);
                return model;
            }

            #endregion

            #region Prepare Data

            var imagePath = "";
            if (!reqModel.ImageData.IsNullOrEmpty())
            {
                var imgFolder = @"D:\Website Portfolio\Wholesale & Inventory Management System\Image\ReqProductChange";
                Directory.CreateDirectory(imgFolder);
                var fileName = DevCode.GenerateUlid() + ".jpg";
                imagePath = Path.Combine(imgFolder, fileName);
                await DevCode.WriteBase64ToFileAsync(reqModel.ImageData, imagePath);
            }

            TblReqProductChange item = new TblReqProductChange
            {
                ReqProductChangesId = DevCode.GenerateUlid(),
                ProductId = reqModel.ProductId!,
                ProductName = reqModel.ProductName!,
                ProductCode = reqModel.ProductCode!,
                SupplierName = reqModel.SupplierName!,
                ImagePath = imagePath,
                ChangesType = EnumRequestedType.Update.ToString(),
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqProductChanges.AddAsync(item);
            await _db.SaveChangesAsync();
            model = Result<ProductModel>.Success(JsonResource.WHS014);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ProductModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ProductModel>> Delete(ProductEditModel reqModel)
    {
        var model = new Result<ProductModel>();
        try
        {
            #region Check Product

            var product = await _db.TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == reqModel.ProductId && x.DelFlag == 0);
            if (product is null)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE035);
                return model;
            }

            #endregion

            #region Check Duplicate

            bool reqUser = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductId == reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE036);
                return model;
            }

            reqUser = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductId == reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ProductModel>.Error(JsonResource.WHE037);
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqProductChange item = new TblReqProductChange
            {
                ReqProductChangesId = DevCode.GenerateUlid(),
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                SupplierName = product.SupplierName,
                ChangesType = EnumRequestedType.Delete.ToString(),
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqProductChanges.AddAsync(item);
            await _db.SaveChangesAsync();
            model = Result<ProductModel>.Success(JsonResource.WHS014);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ProductModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ProductDetailModel>> Details(ProductEditModel reqModel)
    {
        ProductDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<ProductDetailInfoModel>(
                SqlQueries.Sp_GetProductDetail, new
                {
                    ProductId = reqModel.ProductId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> productInfo = new List<DynamicReportModel>();
            productInfo.Add("Product Name", detail.ProductName!);
            productInfo.Add("Product Code", detail.ProductCode!);
            productInfo.Add("Supplier Name", detail.SupplierName!);
            model.ProductInfo = productInfo;
            model.ItemImagePath = detail.ImagePath;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("CreatedUser", detail.CreatedUser!);
            makerChecker.Add("CreatedDateTime", detail.CreatedDateTime!);
            makerChecker.Add("Modified User", detail.ModifiedUser!.ToDashFromNull());
            makerChecker.Add("ModifiedDateTime ", detail.ModifiedDateTime!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<ProductDetailModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ProductDetailModel>.Error(ex);
        }
    }

    #endregion
}
