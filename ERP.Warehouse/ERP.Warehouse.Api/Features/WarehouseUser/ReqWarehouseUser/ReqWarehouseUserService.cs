using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.ReqWarehouseUser;
using ERP.Warehouse.Models.Models.WarehouseUserList;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUser;

public class ReqWarehouseUserService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqWarehouseUserService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    public async Task<Result<ReqWarehouseUserRepModel>> Get(ReqWarehouseUserReqModel reqModel)
    {
        ReqWarehouseUserRepModel model = new();
        try
        {
            var parameters = new
            {
                FullName = reqModel.FullName,
                StaffId = reqModel.StaffId,
                PhoneNo = reqModel.Phone,
                Email = reqModel.Email,
                RoleName = reqModel.RoleCode,
                BranchName = reqModel.BranchCode,
                Status = reqModel.Status
            };
            var result = await _dapperService.QueryStoredProcedureAsync<ReqWarehouseUserModel>
                (SqlQueries.Sp_GetReqWarehouseUserDetail, parameters);
            model.list = result;
            return Result<ReqWarehouseUserRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserRepModel>.Error(ex);
        }
    }

    public async Task<Result<ReqWarehouseUserModel>> Edit(ReqWarehouseUserEditModel reqModel)
    {
        var model = new Result<ReqWarehouseUserModel>();
        try
        {
            #region Check User
            var user = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqWarehouseUserId == reqModel.UserId);
            if (user is null)
            {
                model = Result<ReqWarehouseUserModel>.Error("User does not exist.");
                return model;
            }
            #endregion

            #region Check Role
            var role = await _db.TblWarehouseRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleCode == user.RoleCode && x.DelFlag == 0);
            if (role is null)
            {
                model = Result<ReqWarehouseUserModel>.Error("User Role does not exist.");
                return model;
            }
            #endregion

            #region Check Branch
            var branch = await _db.TblBranches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BranchCode == user.BranchCode && x.DelFlag == 0);
            if (branch is null)
            {
                model = Result<ReqWarehouseUserModel>.Error("Branch does not exist.");
                return model;
            }
            #endregion

            #region Prepare Data

            var response = new ReqWarehouseUserModel
            {
                ReqWarehouseUserId = user.ReqWarehouseUserId,
                UserName = user.UserName,
                FullName = user.FullName,
                StaffId = user.StaffId,
                Phone = user.Phone,
                Email = user.Email,
                RoleCode = user.RoleCode,
                BranchCode = user.BranchCode
            };
            model = Result<ReqWarehouseUserModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqWarehouseUserModel>> Update(ReqWarehouseUserReqModel reqModel)
    {
        var model = new Result<ReqWarehouseUserModel>();
        try
        {
            #region Check User

            TblReqWarehouseUser? user = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqWarehouseUserId == reqModel.UserId);
            if (user is null)
            {
                model = Result<ReqWarehouseUserModel>.Error("Requseted User does not exist.");
                return model;
            }

            bool reqUser = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.ReqWarehouseUserId == reqModel.UserId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqWarehouseUserModel>.Error("Requseted User is not pending status");
                return model;
            }
            #endregion

            #region Check Duplicate UserName

            bool userName = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.UserName!.Trim().ToLower() &&
                               x.ReqWarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqWarehouseUserModel>.Error("Phone Number is already Requested!");
                return model;
            }

            userName = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.UserName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqWarehouseUserModel>.Error("Phone Number is already Requested!");
                return model;
            }

            userName = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.UserName!.Trim().ToLower());
            if (userName)
            {
                model = Result<ReqWarehouseUserModel>.Error("Phone Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate PhoneNo

            bool phoneNo = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower() &&
                               x.ReqWarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserModel>.Error("Phone Number is already Requested!");
                return model;
            }

            phoneNo = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserModel>.Error("Phone Number is already Requested!");
                return model;
            }

            phoneNo = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserModel>.Error("Phone Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate Email

            bool email = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ReqWarehouseUserModel>.Error("Email Number is already Change Requested!");
                return model;
            }

            email = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.ReqWarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ReqWarehouseUserModel>.Error("Email Number is already Requested!");
                return model;
            }

            email = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower());
            if (email)
            {
                model = Result<ReqWarehouseUserModel>.Error("Email Number is already exist!");
                return model;
            }

            #endregion

            #region Prepare Data

            user.UserName = reqModel.UserName!;
            user.FullName = reqModel.FullName!;
            user.Phone = reqModel.Phone!;
            user.Email = reqModel.Email!;
            user.RoleCode = reqModel.RoleCode!;
            user.BranchCode = reqModel.BranchCode!;
            user.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(user).State = EntityState.Modified;
            _db.TblReqWarehouseUsers.Update(user);
            await _db.SaveChangesAsync();
            model = Result<ReqWarehouseUserModel>.Success("Reuqested User is successfully updated");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqWarehouseUserModel>> Delete(ReqWarehouseUserEditModel reqModel)
    {
        var model = new Result<ReqWarehouseUserModel>();
        try
        {
            #region Check User

            var user = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqWarehouseUserId == reqModel.UserId);
            if (user is null)
            {
                model = Result<ReqWarehouseUserModel>.Error("Requested User does not exist.");
                return model;
            }

            bool reqUser = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.ReqWarehouseUserId == reqModel.UserId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqWarehouseUserModel>.Error("Requseted User is not pending status");
                return model;
            }

            #endregion

            #region Prepare Data

            _db.TblReqWarehouseUsers.Remove(user);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<ReqWarehouseUserModel>.Error("Requsted User delete fail!");
                return model;
            }
            model = Result<ReqWarehouseUserModel>.Error("Requsted User is successfully deteted");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserModel>.Error(ex);
        }
        return model;
    }

    #endregion
}
