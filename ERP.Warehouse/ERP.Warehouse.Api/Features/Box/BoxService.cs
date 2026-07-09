using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Box;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Box;

public class BoxService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public BoxService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<BoxRepModel>> Get(BoxReqModel reqModel)
    {
        BoxRepModel model = new();
        try
        {
            var parameters = new
            {
                BoxCode = reqModel.BoxCode,
                Type = reqModel.Type
            };
            var result = await _dapperService.QueryStoredProcedureAsync<BoxModel>
                (SqlQueries.Sp_GetBox, parameters);
            model.list = result;
            return Result<BoxRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<BoxRepModel>.Error(ex);
        }
    }

    #endregion
}
