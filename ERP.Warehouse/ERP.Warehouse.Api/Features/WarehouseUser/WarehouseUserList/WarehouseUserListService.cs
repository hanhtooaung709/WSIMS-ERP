using DocumentFormat.OpenXml.Spreadsheet;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Signin;
using ERP.Warehouse.Models.Models.WarehouseUserList;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;

public class WarehouseUserListService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public WarehouseUserListService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    public async Task<Result<WarehouseUserRepModel>> Get(WarehouseUserReqModel reqModel)
    {
        WarehouseUserRepModel model = new();
        try
        {
            var parameters = new
            {
                FullName = reqModel.FullName,
                StaffId = reqModel.StaffId,
                PhoneNo = reqModel.PhoneNo,
                Email = reqModel.Email,
                RoleName = reqModel.RoleName,
                BranchName = reqModel.BranchName
            };
            var result = await _dapperService.QueryStoredProcedureAsync<WarehouseUserModel>
                (SqlQueries.Sp_GetWarehouseUserList, parameters);
            model.list = result;
            return Result<WarehouseUserRepModel>.Success(model);
        }
        catch(Exception ex)
        {
            return Result<WarehouseUserRepModel>.Error(ex);
        }
    }
}
