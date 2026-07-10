using DocumentFormat.OpenXml.Wordprocessing;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.Branch;
using ERP.Warehouse.Models.Models.Currency;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Module.CommonDbService.EfAppDbContextModels;
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

    #endregion
}
