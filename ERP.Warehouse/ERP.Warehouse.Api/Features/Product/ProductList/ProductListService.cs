using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Currency;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
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

            #endregion

            #region Prepare Data

            var result = await product
                .AsNoTracking()
                .Select(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProductCode = x.ProductCode
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
                model = Result<ProductModel>.Error("Currency Name is already exist!");
                return model;
            }

            name = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower());
            if (name)
            {
                model = Result<ProductModel>.Error("Currency Name is already Requested!");
                return model;
            }

            name = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower());
            if (name)
            {
                model = Result<ProductModel>.Error("Currency Name is already Requested!");
                return model;
            }

            #endregion

            #region Check Duplicate Product Code

            bool code = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ProductModel>.Error("Currency Code is already exist!");
                return model;
            }

            code = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ProductModel>.Error("Currency Code is already Requested!");
                return model;
            }

            code = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ProductModel>.Error("Currency Code is already Requested!");
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqProduct item = new TblReqProduct
            {
                ReqProductId = DevCode.GenerateUlid(),
                ProductName = reqModel.ProductName!,
                ProductCode = reqModel.ProductCode!,
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqProducts.AddAsync(item);
            await _db.SaveChangesAsync();

            model = Result<ProductModel>.Success("Your request is pending for approval!");

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

            var user = await _db.TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == reqModel.ProductId && x.DelFlag == 0);
            if (user is null)
            {
                model = Result<ProductModel>.Error("Product does not exist.");
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new ProductModel
            {
                ProductId = user.ProductId,
                ProductName = user.ProductName,
                ProductCode = user.ProductCode
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

            var user = await _db.TblProducts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == reqModel.ProductId && x.DelFlag == 0);
            if (user is null)
            {
                model = Result<ProductModel>.Error("Product does not exist.");
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
                model = Result<ProductModel>.Error("Product is already Requested!");
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
                model = Result<ProductModel>.Error("Product Name is already exist!");
                return model;
            }

            name = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ProductModel>.Error("Product Name is already Requested!");
                return model;
            }

            name = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (name)
            {
                model = Result<ProductModel>.Error("Product Name is already Requested!");
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
                model = Result<ProductModel>.Error("Product Code is already exist!");
                return model;
            }

            email = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ProductModel>.Error("Product Code is already Requested!");
                return model;
            }

            email = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.ProductId != reqModel.ProductId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ProductModel>.Error("Product Code is already Requested!");
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqProductChange item = new TblReqProductChange
            {
                ReqProductChangesId = DevCode.GenerateUlid(),
                ProductId = reqModel.ProductId!,
                ProductName = reqModel.ProductName!,
                ProductCode = reqModel.ProductCode!,
                ChangesType = EnumRequestedType.Update.ToString(),
                Status = EnumRequestedStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqProductChanges.AddAsync(item);
            await _db.SaveChangesAsync();
            model = Result<ProductModel>.Success("Your request is pending for approval!");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ProductModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
