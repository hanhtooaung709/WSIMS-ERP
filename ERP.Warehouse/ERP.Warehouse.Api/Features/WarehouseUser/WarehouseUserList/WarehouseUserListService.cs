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

    public async Task<Result<WarehouseUserModel>> Edit(WarehouseUserEditModel reqModel)
    {
        var model = new Result<WarehouseUserModel>();
        try
        {
            #region Check User
            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.WarehouseUserId == reqModel.UserId && x.DelFlag == 0);
            if (user is null)
            {
                model = Result<WarehouseUserModel>.Error("User does not exist.");
                return model;
            }
            #endregion

            #region Check Role
            var role = await _db.TblWarehouseRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleCode == user.RoleCode && x.DelFlag == 0);
            if (role is null)
            {
                model = Result<WarehouseUserModel>.Error("User Role does not exist.");
                return model;
            }
            #endregion

            #region Check Branch
            var branch = await _db.TblBranches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BranchCode == user.BranchCode && x.DelFlag == 0);
            if (branch is null)
            {
                model = Result<WarehouseUserModel>.Error("Branch does not exist.");
                return model;
            }
            #endregion

            var response = new WarehouseUserModel
            {
                WarehouseUserId = user.WarehouseUserId,
                FullName = user.FullName,
                StaffId = user.StaffId,
                PhoneNo = user.Phone,
                Email = user.Email,
                RoleName = role.RoleName,
                BranchName = branch.Address
            };
            model = Result<WarehouseUserModel>.Success(response);
        }
        catch(Exception ex)
        {
            return Result<WarehouseUserModel>.Error(ex);
        }
        return model;
    }
}
