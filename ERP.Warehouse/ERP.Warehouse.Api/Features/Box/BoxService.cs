using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Box;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared;
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

    public async Task<Result<BoxModel>> Create(BoxReqModel reqModel)
    {
        var model = new Result<BoxModel>();
        try
        {
            #region Check Duplicate Box Code

            bool code = await _db.TblBoxes
                .AsNoTracking()
                .AnyAsync(x => x.BoxCode.Trim().ToLower() == reqModel.BoxCode!.Trim().ToLower());
            if (code)
            {
                model = Result<BoxModel>.Error("Box Code is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate Type

            bool type = await _db.TblBoxes
                .AsNoTracking()
                .AnyAsync(x => x.Type.Trim().ToLower() == reqModel.Type!.Trim().ToLower());
            if (type)
            {
                model = Result<BoxModel>.Error("Box Type is already exist!");
                return model;
            }

            #endregion

            #region Prepare Data

            TblBox item = new TblBox
            {
                BoxId = DevCode.GenerateUlid(),
                BoxCode = reqModel.BoxCode!,
                Type = reqModel.Type!,
                Size = reqModel.Size!,
                TareWeight = reqModel.TareWeight!,
                MaxNetWeight = reqModel.MaxNetWeight!,
                CreatedUserId = AuthorizedUserId,
                CreatedDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblBoxes.AddAsync(item);
            await _db.SaveChangesAsync();

            model = Result<BoxModel>.Success("Box is successfully created");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<BoxModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<BoxModel>> Edit(BoxEditModel reqModel)
    {
        var model = new Result<BoxModel>();
        try
        {
            #region Check Box
            var box = await _db.TblBoxes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BoxId == reqModel.BoxId && x.DelFlag == 0);
            if (box is null)
            {
                model = Result<BoxModel>.Error("Box does not exist.");
                return model;
            }
            #endregion

            #region Prepare Data

            var response = new BoxModel
            {
                BoxId = box.BoxId!,
                BoxCode = box.BoxCode!,
                Type = box.Type!,
                Size = box.Size!,
                TareWeight = box.TareWeight!,
                MaxNetWeight = box.TareWeight!,
            };
            model = Result<BoxModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<BoxModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
