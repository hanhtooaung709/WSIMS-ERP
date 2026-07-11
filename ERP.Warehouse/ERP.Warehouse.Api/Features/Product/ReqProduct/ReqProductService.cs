using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Currency;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Product.ReqProduct;

public class ReqProductService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqProductService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<ReqProductRepModel>> Get(ReqProductReqModel reqModel)
    {
        ReqProductRepModel model = new();
        try
        {
            IQueryable<TblReqProduct>? product = _db.TblReqProducts
                .AsNoTracking();

            #region Filters

            if (!reqModel.ProductName.IsNullOrEmpty())
            {
                product = product.Where(x => x.ProductName.ToUpper().Trim() == reqModel.ProductName!.ToUpper().Trim());
            }
            if (!reqModel.ProductCode.IsNullOrEmpty())
            {
                product = product.Where(x => x.ProductName.ToUpper().Trim() == reqModel.ProductCode!.ToUpper().Trim());
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
    #endregion
}
