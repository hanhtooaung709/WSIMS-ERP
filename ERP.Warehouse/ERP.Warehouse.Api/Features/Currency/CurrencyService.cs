using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using WSIMS_ERP.Shared.Models.DynamicModel;
using ERP.Warehouse.Models.Models.Currency;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
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
                model = Result<CurrencyModel>.Error(JsonResource.WHE049);
                return model;
            }

            #endregion

            #region Check Duplicate Description

            bool type = await _db.TblCurrencies
                .AsNoTracking()
                .AnyAsync(x => x.CurrencyDescription.Trim().ToLower() == reqModel.CurrencyDes!.Trim().ToLower());
            if (type)
            {
                model = Result<CurrencyModel>.Error(JsonResource.WHE050);
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

            model = Result<CurrencyModel>.Success(JsonResource.WHS051);

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
                model = Result<CurrencyModel>.Error(JsonResource.WHE052);
                return model;
            }

            #endregion

            #region Prepare Data

            var response = new CurrencyModel
            {
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
                model = Result<CurrencyModel>.Error(JsonResource.WHE052);
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
                model = Result<CurrencyModel>.Error(JsonResource.WHE049);
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
                model = Result<CurrencyModel>.Error(JsonResource.WHE050);
                return model;
            }

            #endregion

            #region Prepare Data

            currency.CurrencyCode = reqModel.CurrencyCode!;
            currency.CurrencyDescription = reqModel.CurrencyDes!;
            currency.ModifiedUserId = AuthorizedUserId;
            currency.ModifiedDateTime = DevCode.GetServerDateTime();


            _db.Entry(currency).State = EntityState.Modified;
            _db.TblCurrencies.Update(currency);
            await _db.SaveChangesAsync();
            model = Result<CurrencyModel>.Success(JsonResource.WHS053);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<CurrencyModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<CurrencyModel>> Delete(CurrencyEditModel reqModel)
    {
        var model = new Result<CurrencyModel>();
        try
        {
            #region Check Currency

            var currency = await _db.TblCurrencies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CurrencyId == reqModel.CurrencyId);
            if (currency is null)
            {
                model = Result<CurrencyModel>.Error(JsonResource.WHE052);
                return model;
            }

            #endregion

            #region Prepare Data

            _db.TblCurrencies.Remove(currency);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<CurrencyModel>.Error(JsonResource.WHE054);
                return model;
            }
            model = Result<CurrencyModel>.Success(JsonResource.WHS055);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<CurrencyModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<CurrencyDetailModel>> Details(CurrencyEditModel reqModel)
    {
        CurrencyDetailModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<CurrencyDetailInfoModel>(
                SqlQueries.Sp_GetCurrencyDetail, new
                {
                    CurrencyId = reqModel.CurrencyId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> CurrencyInfo = new List<DynamicReportModel>();
            CurrencyInfo.Add("User Name", detail.CurrencyCode!);
            CurrencyInfo.Add("Full Name", detail.CurrencyDes!);
            model.CurrencyInfo = CurrencyInfo;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("CreatedUser", detail.CreatedUser!);
            makerChecker.Add("CreatedDateTime", detail.CreatedDateTime!);
            makerChecker.Add("Modified User", detail.ModifiedUser!.ToDashFromNull());
            makerChecker.Add("ModifiedDateTime ", detail.ModifiedDateTime!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<CurrencyDetailModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<CurrencyDetailModel>.Error(ex);
        }
    }

    #endregion
}
