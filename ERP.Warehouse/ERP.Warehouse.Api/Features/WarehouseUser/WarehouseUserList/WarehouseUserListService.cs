using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.WarehouseUserList;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using System.Data;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using WSIMS_ERP.Shared.Models.DynamicModel;

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

    #region Get/Create/Edit/Update/Delete/Details

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
                .AnyAsync(x => x.UserName.Trim().ToLower() == reqModel.UserName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("UserName is already Requested!");
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
                .AnyAsync(x => x.StaffId.Trim().ToLower() == reqModel.StaffId.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (staffId)
            {
                model = Result<WarehouseUserModel>.Error("StaffId is already Requested!");
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
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.PhoneNo!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already Requested!");
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

            phoneNo = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Phone == reqModel.PhoneNo &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already Change Requested!");
                return model;
            }

            #endregion

            #region Check Duplicate Email

            bool email = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email is already Requested!");
                return model;
            }

            email = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower());
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email is already exist!");
                return model;
            }

            email = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email is already Change Requested!");
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
                Status = EnumRequestedStatus.Pending.ToString(),
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
                .AnyAsync(x => x.WarehouseUserId == reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("User is already Requested!");
                return model;
            }

            #endregion

            #region Check Duplicate PhoneNo

            bool phoneNo = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.PhoneNo!.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already Change Requested!");
                return model;
            }

            phoneNo = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.PhoneNo!.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already Requesed!");
                return model;
            }

            phoneNo = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.PhoneNo!.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.UserId);
            if (phoneNo)
            {
                model = Result<WarehouseUserModel>.Error("Phone Number is already exist!");
                return model;
            }

            #endregion

            #region Check Duplicate Email

            bool email = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email Number is already Change Requested!");
                return model;
            }

            email = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email Number is already Requesed!");
                return model;
            }

            email = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.UserId);
            if (email)
            {
                model = Result<WarehouseUserModel>.Error("Email Number is already exist!");
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
                Status = EnumRequestedStatus.Pending.ToString(),
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
                .AnyAsync(x => x.WarehouseUserId == reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("User is already Requested!");
                return model;
            }

            reqUser = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.WarehouseUserId == reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<WarehouseUserModel>.Error("User is already Requested!");
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
                Status = EnumRequestedStatus.Pending.ToString(),
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

    public async Task<Result<WarehouseUserDetailsModel>> Details(WarehouseUserEditModel reqModel)
    {
        WarehouseUserDetailsModel model = new WarehouseUserDetailsModel();
        try
        {
            var detail = await _dapperService.GetDetailAsync<WarehouseUserDetailsInfoModel>(
                SqlQueries.Sp_GetWarehouseUserDetail, new
                {
                    UserId = reqModel.UserId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> userInfo = new List<DynamicReportModel>();
            userInfo.Add("User Name", detail.UserName!);
            userInfo.Add("Full Name", detail.FullName!);
            userInfo.Add("Staff Id", detail.StaffId!);
            userInfo.Add("Role ", detail.RoleName!);
            userInfo.Add("Branch ", detail.BranchName!);
            userInfo.Add("Phone", detail.PhoneNo!);
            userInfo.Add("Email", detail.Email!);
            userInfo.Add("Lock", detail.LockFlag!);
            model.UserInfo = userInfo;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("CreatedUser", detail.CreatedUser!);
            makerChecker.Add("CreatedDateTime", detail.CreatedDateTime!);
            makerChecker.Add("Modified User", detail.ModifiedUser!.ToDashFromNull());
            makerChecker.Add("ModifiedDateTime ", detail.ModifiedDateTime!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<WarehouseUserDetailsModel>.Success(model);
        }
        catch(Exception ex)
        {
            return Result<WarehouseUserDetailsModel>.Error(ex);
        }
    }

    #endregion

    #region DropDown

    public async Task<Result<List<RoleResponseModel>>> GetRole()
    {
        try
        {
            var result = await _db.TblWarehouseRoles
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .Select(x => new RoleResponseModel
                {
                    RoleName = x.RoleName,
                    RoleCode = x.RoleCode
                })
                .ToListAsync();

            return Result<List<RoleResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<RoleResponseModel>>.Error(ex);
        }
    }

    public async Task<Result<List<BranchResponseModel>>> GetBranch()
    {
        try
        {
            var result = await _db.TblBranches
                .AsNoTracking()
                .Where(x => x.DelFlag == 0)
                .Select(x => new BranchResponseModel
                {
                    Address = x.Address,
                    BranchCode = x.BranchCode
                })
                .ToListAsync();

            return Result<List<BranchResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<BranchResponseModel>>.Error(ex);
        }
    }

    #endregion
}
