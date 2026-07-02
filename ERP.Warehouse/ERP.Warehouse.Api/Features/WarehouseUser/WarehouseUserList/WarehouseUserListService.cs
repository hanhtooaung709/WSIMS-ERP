using DocumentFormat.OpenXml.Spreadsheet;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.WarehouseUserList;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
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
                RoleName = reqModel.RoleCode,
                BranchName = reqModel.BranchCode
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

    public async Task<Result<WarehouseUserModel>> Create(WarehouseUserReqModel reqModel)
    {
        var model = new Result<WarehouseUserModel>();
        try
        {
            #region Check Duplicate User Name

            bool reqUser = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName == reqModel.UserName);
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("UserName is already Requesed!");
                return model;
            }

            reqUser = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName == reqModel.UserName);
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("UserName is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate StaffId

            bool staffId = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.StaffId == reqModel.StaffId);
            if (staffId)
            {
                model = Result<WarehouseUserModel>.Error("StaffId is already Requesed!");
                return model;
            }

            staffId = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.StaffId == reqModel.StaffId);
            if (staffId)
            {
                model = Result<WarehouseUserModel>.Error("StaffId is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate PhoneNo

            bool phoneNo = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone == reqModel.PhoneNo);
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already Requesed!");
                return model;
            }

            phoneNo = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone == reqModel.PhoneNo);
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate Email

            bool email = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email == reqModel.Email);
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email Number is already Requesed!");
                return model;
            }

            email = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email == reqModel.Email);
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate RoleCode

            bool roleCode = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.RoleCode == reqModel.RoleCode);
            if (roleCode)
            {
                model = Result<WarehouseUserModel>.Error("RoleCode Number is already Requesed!");
                return model;
            }

            roleCode = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.RoleCode == reqModel.RoleCode);
            if (roleCode)
            {
                model = Result<WarehouseUserModel>.Error("RoleCode Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate BranchCode

            bool branchCode = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.BranchCode == reqModel.BranchCode);
            if (branchCode)
            {
                model = Result<WarehouseUserModel>.Error("BranchCode Number is already Requesed!");
                return model;
            }

            branchCode = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.BranchCode == reqModel.BranchCode);
            if (branchCode)
            {
                model = Result<WarehouseUserModel>.Error("BranchCode Number is already exist!");
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqWarehouseUser item = new TblReqWarehouseUser
            {
                ReqWarehouseUserId = DevCode.GenerateUlid(),
                UserName = reqModel.UserName!,
                FullName = reqModel.FullName!,
                StaffId = reqModel.StaffId!,
                Phone = reqModel.PhoneNo!,
                Email = reqModel.Email!,
                RoleCode = reqModel.RoleCode!,
                BranchCode = reqModel.BranchCode!,
                Status = EnumRequestedUserStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqWarehouseUsers.AddAsync(item);
            await _db.SaveChangesAsync();

            model = Result<WarehouseUserModel>.Success("Your request is pending for approval!");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<WarehouseUserModel>.Error(ex);
        }
        return model;
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

            #region Prepare Data

            var response = new WarehouseUserModel
            {
                WarehouseUserId = user.WarehouseUserId,
                UserName = user.UserName,
                FullName = user.FullName,
                StaffId = user.StaffId,
                PhoneNo = user.Phone,
                Email = user.Email,
                RoleCode = user.RoleCode,
                BranchCode = user.BranchCode
            };
            model = Result<WarehouseUserModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<WarehouseUserModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<WarehouseUserModel>> Update(WarehouseUserReqModel reqModel)
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

            #region Check Duplicate Id

            bool reqUser = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.WarehouseUserId == reqModel.UserId);
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("User is already Requesed!");
                return model;
            }

            #endregion

            #region Check Duplicate User Name

            bool userName = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName == reqModel.UserName
                          && x.WarehouseUserId != reqModel.UserId);
            if (userName)
            {
                model = Result<WarehouseUserModel>.Error("UserName is already Requesed!");
                return model;
            }

            userName = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName == reqModel.UserName
                          && x.WarehouseUserId != reqModel.UserId);
            if (userName)
            {
                model = Result<WarehouseUserModel>.Error("UserName is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate PhoneNo

            bool phoneNo = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone == reqModel.PhoneNo
                          && x.WarehouseUserId != reqModel.UserId);
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already Requesed!");
                return model;
            }

            phoneNo = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone == reqModel.PhoneNo
                          && x.WarehouseUserId != reqModel.UserId);
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate Email

            bool email = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email == reqModel.Email
                          && x.WarehouseUserId != reqModel.UserId);
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email Number is already Requesed!");
                return model;
            }

            email = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email == reqModel.Email
                          && x.WarehouseUserId != reqModel.UserId);
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate RoleCode

            bool roleCode = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.RoleCode == reqModel.RoleCode
                          && x.WarehouseUserId != reqModel.UserId);
            if (roleCode)
            {
                model = Result<WarehouseUserModel>.Error("RoleCode Number is already Requesed!");
                return model;
            }

            roleCode = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.RoleCode == reqModel.RoleCode
                          && x.WarehouseUserId != reqModel.UserId);
            if (roleCode)
            {
                model = Result<WarehouseUserModel>.Error("RoleCode Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate BranchCode

            bool branchCode = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.BranchCode == reqModel.BranchCode
                          && x.WarehouseUserId != reqModel.UserId);
            if (branchCode)
            {
                model = Result<WarehouseUserModel>.Error("BranchCode Number is already Requesed!");
                return model;
            }

            branchCode = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.BranchCode == reqModel.BranchCode
                          && x.WarehouseUserId != reqModel.UserId);
            if (branchCode)
            {
                model = Result<WarehouseUserModel>.Error("BranchCode Number is already exist!");
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqWarehouseUserChange item = new TblReqWarehouseUserChange
            {
                ReqWarehouseUserChangesId = DevCode.GenerateUlid(),
                WarehouseUserId = reqModel.UserId!,
                FullName = reqModel.FullName!,
                Phone = reqModel.PhoneNo!,
                Email = reqModel.Email!,
                RoleCode = reqModel.RoleCode!,
                BranchCode = reqModel.BranchCode!,
                ChangesType = EnumRequestedType.Update.ToString(),
                Status = EnumRequestedUserStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqWarehouseUserChanges.AddAsync(item);
            await _db.SaveChangesAsync();
            model = Result<WarehouseUserModel>.Success("Your request is pending for approval!");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<WarehouseUserModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<WarehouseUserModel>> Delete(WarehouseUserEditModel reqModel)
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

            #region Check Duplicate

            bool reqUser = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.WarehouseUserId == reqModel.UserId);
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("User is already Requesed!");
                return model;
            }

            #endregion

            #region Prepare Data

            TblReqWarehouseUserChange item = new TblReqWarehouseUserChange
            {
                ReqWarehouseUserChangesId = DevCode.GenerateUlid(),
                WarehouseUserId = user.WarehouseUserId,
                FullName = user.FullName,
                Phone = user.Phone,
                Email = user.Email,
                RoleCode = user.RoleCode,
                BranchCode = user.BranchCode,
                ChangesType = EnumRequestedType.Delete.ToString(),
                Status = EnumRequestedUserStatus.Pending.ToString(),
                ReqUserId = AuthorizedUserId,
                ReqDateTime = DevCode.GetServerDateTime()
            };
            await _db.TblReqWarehouseUserChanges.AddAsync(item);
            await _db.SaveChangesAsync();
            model = Result<WarehouseUserModel>.Success("Your request is pending for approval!");

            #endregion
        }
        catch (Exception ex)
        {
            return Result<WarehouseUserModel>.Error(ex);
        }
        return model;
    }
}
