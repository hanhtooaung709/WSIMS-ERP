using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.ApproveProduct.ApproveReqProduct;

public class ApproveReqProductService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ApproveReqProductService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
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
                model = Result<ReqProductModel>.Error("Requseted Product does not exist.");
                return model;
            }

            bool reqProduct = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductId == reqModel.ReqProductId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqProduct)
            {
                model = Result<ReqProductModel>.Error("Requseted User is not pending status");
                return model;
            }

            #endregion

            #region Prepare Data

            var productid = DevCode.GenerateUlid();

            TblProduct item = new TblProduct
            {
                ProductId = productid,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
                SupplierName = product.SupplierName,
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

            model = Result<ReqProductModel>.Success("Requested Product is successfully approved");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductModel>.Error(ex);
        }

        return model;
    }

    #endregion
}
