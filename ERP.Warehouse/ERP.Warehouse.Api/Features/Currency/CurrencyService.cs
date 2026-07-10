using DocumentFormat.OpenXml.Wordprocessing;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.Branch;
using ERP.Warehouse.Models.Models.Currency;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Currency;

public class CurrencyService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public CurrencyService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<CurrencyRepModel>> Get(CurrencyReqModel reqModel)
    {
        CurrencyRepModel model = new();
        try
        {
            IQueryable<TblCurrency>? currency = _db.TblCurrencies
                .AsNoTracking()
                .Where(x => x.DelFlag == 0);

            #region Filters

            if (!reqModel.CurrencyCode.IsNullOrEmpty())
            {
                currency = currency.Where(x => x.CurrencyCode.ToUpper().Trim() == reqModel.CurrencyCode!.ToUpper().Trim());
            }
            if (!reqModel.CurrencyDes.IsNullOrEmpty())
            {
                currency = currency.Where(x => x.CurrencyDescription.ToUpper().Trim() == reqModel.CurrencyDes!.ToUpper().Trim());
            }

            #endregion

            #region Prepare Data

            var result = await currency
                .AsNoTracking()
                .Select(x => new CurrencyModel
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyCode = x.CurrencyCode,
                    CurrencyDes = x.CurrencyDescription
                })
                .ToListAsync();

            model.list = result;
            return Result<CurrencyRepModel>.Success(model);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<CurrencyRepModel>.Error(ex);
        }
    }

    public async Task<Result<CurrencyModel>> Create(CurrencyReqModel reqModel)
    {
        var model = new Result<CurrencyModel>();
        try
        {
            #region Check Duplicate Currency Code

            bool code = await _db.TblCurrencies
                .AsNoTracking()
                .AnyAsync(x => x.CurrencyCode.Trim().ToLower() == reqModel.CurrencyCode!.Trim().ToLower());
            if (code)
            {
                model = Result<CurrencyModel>.Error("Currency Code is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate Description

            bool type = await _db.TblCurrencies
                .AsNoTracking()
                .AnyAsync(x => x.CurrencyDescription.Trim().ToLower() == reqModel.CurrencyDes!.Trim().ToLower());
            if (type)
            {
                model = Result<CurrencyModel>.Error("Currency Description is already exist!");
                return model;
            }

            #endregion

            #region Prepare Data

            TblCurrency item = new TblCurrency
            {
                CurrencyId = DevCode.GenerateUlid(),
                CurrencyCode = reqModel.CurrencyCode!,
                CurrencyDescription = reqModel.CurrencyDes!,
                CreatedUserId = AuthorizedUserId,
                CreatedDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblCurrencies.AddAsync(item);
            await _db.SaveChangesAsync();

            model = Result<CurrencyModel>.Success("Curreycy is successfully created");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<CurrencyModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<CurrencyModel>> Edit(CurrencyEditModel reqModel)
    {
        var model = new Result<CurrencyModel>();
        try
        {
            #region Check Currency

            var box = await _db.TblCurrencies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CurrencyId == reqModel.CurrencyId && x.DelFlag == 0);
            if (box is null)
            {
                model = Result<CurrencyModel>.Error("Currency does not exist.");
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new CurrencyModel
            {
                CurrencyId = box.CurrencyId!,
                CurrencyCode = box.CurrencyCode!,
                CurrencyDes = box.CurrencyDescription!
            };
            model = Result<CurrencyModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<CurrencyModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<CurrencyModel>> Update(CurrencyReqModel reqModel)
    {
        var model = new Result<CurrencyModel>();
        try
        {
            #region Check Currency

            var currency = await _db.TblCurrencies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CurrencyId == reqModel.CurrencyId && x.DelFlag == 0);
            if (currency is null)
            {
                model = Result<CurrencyModel>.Error("Currency does not exist.");
                return model;
            }

            #endregion

            #region Check Currency Code

            bool code = await _db.TblCurrencies
                .AsNoTracking()
                .AnyAsync(x => x.CurrencyCode.Trim().ToLower() == reqModel.CurrencyCode!.Trim().ToLower() &&
                          x.CurrencyId != reqModel.CurrencyId);
            if (code)
            {
                model = Result<CurrencyModel>.Error("Box Code is already exist!");
                return model;
            }

            #endregion

            #region Check Currency 

            bool type = await _db.TblCurrencies
                .AsNoTracking()
                .AnyAsync(x => x.CurrencyDescription.Trim().ToLower() == reqModel.CurrencyDes!.Trim().ToLower() &&
                          x.CurrencyId != reqModel.CurrencyId);
            if (type)
            {
                model = Result<CurrencyModel>.Error("Currency Description is already exist!");
                return model;
            }

            #endregion

            #region Prepare Data

            currency.CurrencyId = reqModel.CurrencyId!;
            currency.CurrencyCode = reqModel.CurrencyCode!;
            currency.CurrencyDescription = reqModel.CurrencyDes!;
            currency.ModifiedUserId = AuthorizedUserId;
            currency.ModifiedDateTime = DevCode.GetServerDateTime();


            _db.Entry(currency).State = EntityState.Modified;
            _db.TblCurrencies.Update(currency);
            await _db.SaveChangesAsync();
            model = Result<CurrencyModel>.Success("Currency is successfully updated");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<CurrencyModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
