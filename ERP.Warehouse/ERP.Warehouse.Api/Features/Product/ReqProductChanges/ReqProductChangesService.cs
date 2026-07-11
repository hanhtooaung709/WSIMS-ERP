using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared;
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

    public async Task<Result<ReqProductChangesModel>> Update(ReqProductChangesReqModel reqModel)
    {
        var model = new Result<ReqProductChangesModel>();
        try
        {
            #region Check Product

            TblReqProductChange? product = await _db.TblReqProductChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqProductChangesId == reqModel.ReqProductChangesId);
            if (product is null)
            {
                model = Result<ReqProductChangesModel>.Error("Requseted Product does not exist.");
                return model;
            }

            bool reqUser = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqProductChangesId == reqModel.ReqProductChangesId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqProductChangesModel>.Error("Requseted User is not pending status");
                return model;
            }
            #endregion

            #region Check Duplicate Product Name

            bool userName = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower());
            if (userName)
            {
                model = Result<ReqProductChangesModel>.Error("Product Name is already exist!");
                return model;
            }

            userName = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqProductChangesModel>.Error("Product Name is already Requested!");
                return model;
            }

            userName = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductName.Trim().ToLower() == reqModel.ProductName!.Trim().ToLower() &&
                               x.ReqProductChangesId != reqModel.ReqProductChangesId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqProductChangesModel>.Error("Product Name is already Requested!");
                return model;
            }

            #endregion

            #region Check Duplicate Product code

            bool code = await _db.TblProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower());
            if (code)
            {
                model = Result<ReqProductChangesModel>.Error("Product Code is already exist!");
                return model;
            }

            code = await _db.TblReqProducts
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (code)
            {
                model = Result<ReqProductChangesModel>.Error("Product Code is already Requested!");
                return model;
            }

            code = await _db.TblReqProductChanges
                .AsNoTracking()
                .AnyAsync(x => x.ProductCode.Trim().ToLower() == reqModel.ProductCode!.Trim().ToLower() &&
                               x.ReqProductChangesId != reqModel.ReqProductChangesId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (code)
            {
                model = Result<ReqProductChangesModel>.Error("Product Code is already Requested!");
                return model;
            }

            #endregion

            #region Prepare Data

            product.ProductName = reqModel.ProductName!;
            product.ProductCode = reqModel.ProductCode!;
            product.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(product).State = EntityState.Modified;
            _db.TblReqProductChanges.Update(product);
            await _db.SaveChangesAsync();
            model = Result<ReqProductChangesModel>.Success("Requested Product is successfully updated");

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
