using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Product.ReqProductChanges;

public class ReqProductChangesService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqProductChangesService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<ReqProductChangesRepModel>> Get(ReqProductChangesReqModel reqModel)
    {
        ReqProductChangesRepModel model = new();
        try
        {
            IQueryable<TblReqProductChange>? product = _db.TblReqProductChanges
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
                        ProductName = x.ProductName,
                        ProductCode = x.ProductCode,
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

    public async Task<Result<ReqProductChangesModel>> Edit(ReqProductChangesEditModel reqModel)
    {
        var model = new Result<ReqProductChangesModel>();
        try
        {
            #region Check ReqProduct

            var product = await _db.TblReqProductChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqProductChangesId == reqModel.ReqProductChangesId);
            if (product is null)
            {
                model = Result<ReqProductChangesModel>.Error("Requested Product does not exist.");
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new ReqProductChangesModel
            {
                ReqProductChangesId = product.ReqProductChangesId,
                ProductName = product.ProductName,
                ProductCode = product.ProductCode,
            };
            model = Result<ReqProductChangesModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqProductChangesModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
