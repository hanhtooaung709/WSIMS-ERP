using ERP.Warehouse.Api.Common;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using WSIMS_ERP.Shared.Models.DynamicModel;
using System.Data;
using ERP.Warehouse.Models;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;

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
                CurrentUserId = AuthorizedUserId,
                FullName = reqModel.FullName,
                StaffId = reqModel.StaffId,
                PhoneNo = reqModel.Phone,
                Email = reqModel.Email,
                RoleName = reqModel.RoleCode,
                BranchName = reqModel.BranchCode,
                Status = reqModel.Status
            };
            var result = await _dapperService.QueryStoredProcedureAsync<ReqWarehouseUserModel>
                (SqlQueries.Sp_GetReqWarehouseUserList, parameters);
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
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE001);
                return model;
            }
            #endregion

            #region Check Role
            var role = await _db.TblWarehouseRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleCode == user.RoleCode && x.DelFlag == 0);
            if (role is null)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE015);
                return model;
            }
            #endregion

            #region Check Branch
            var branch = await _db.TblBranches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BranchCode == user.BranchCode && x.DelFlag == 0);
            if (branch is null)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE016);
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
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE019);
                return model;
            }

            bool reqUser = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.ReqWarehouseUserId == reqModel.UserId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE020);
                return model;
            }
            #endregion

            #region Check Duplicate UserName

            bool userName = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName.Trim().ToLower() == reqModel.UserName!.Trim().ToLower());
            if (userName)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE004);
                return model;
            }

            userName = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName.Trim().ToLower() == reqModel.UserName!.Trim().ToLower() &&
                               x.ReqWarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE005);
                return model;
            }


            #endregion

            #region Check Duplicate StaffId

            bool staffId = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.StaffId.Trim().ToLower() == reqModel.StaffId!.Trim().ToLower());
            if (staffId)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE006);
                return model;
            }

            staffId = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.StaffId.Trim().ToLower() == reqModel.StaffId!.Trim().ToLower() &&
                               x.ReqWarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (staffId)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE007);
                return model;
            }

            #endregion

            #region Check Duplicate PhoneNo

            bool phoneNo = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE008);
                return model;
            }

            phoneNo = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower() &&
                               x.ReqWarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE009);
                return model;
            }

            phoneNo = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE010);
                return model;
            }


            #endregion

            #region Check Duplicate Email

            bool email = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower());
            if (email)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE011);
                return model;
            }

            email = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.ReqWarehouseUserId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE012);
                return model;
            }

            email = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE013);
                return model;
            }

            #endregion

            #region Prepare Data

            user.UserName = reqModel.UserName!;
            user.FullName = reqModel.FullName!;
            user.StaffId = reqModel.StaffId!;
            user.Phone = reqModel.Phone!;
            user.Email = reqModel.Email!;
            user.RoleCode = reqModel.RoleCode!;
            user.BranchCode = reqModel.BranchCode!;
            user.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(user).State = EntityState.Modified;
            _db.TblReqWarehouseUsers.Update(user);
            await _db.SaveChangesAsync();
            model = Result<ReqWarehouseUserModel>.Success(JsonResource.WHS021);

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
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE019);
                return model;
            }

            bool reqUser = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.ReqWarehouseUserId == reqModel.UserId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE020);
                return model;
            }

            #endregion

            #region Prepare Data

            _db.TblReqWarehouseUsers.Remove(user);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<ReqWarehouseUserModel>.Error(JsonResource.WHE022);
                return model;
            }
            model = Result<ReqWarehouseUserModel>.Success(JsonResource.WHS023);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqWarehouseUserDetailsModel>> Details(ReqWarehouseUserEditModel reqModel)
    {
        ReqWarehouseUserDetailsModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<ReqWarehouseUserDetailsInfoModel>(
                SqlQueries.Sp_GetReqWarehouseUserDetail, new
                {
                    UserId = reqModel.UserId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> userInfo = new List<DynamicReportModel>();
            userInfo.Add("User Name", detail.UserName!);
            userInfo.Add("Full Name", detail.FullName!);
            userInfo.Add("Staff Id", detail.StaffId!);
            userInfo.Add("Role ", detail.RoleName!);
            userInfo.Add("Branch ", detail.BranchName!);
            userInfo.Add("Phone", detail.Phone!);
            userInfo.Add("Email", detail.Email!);
            
            model.UserInfo = userInfo;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("CreatedUser", detail.RequestedUser!);
            makerChecker.Add("CreatedDateTime", detail.RequestedDateTime!);
            makerChecker.Add("Modified User", detail.ApprovedUser!.ToDashFromNull());
            makerChecker.Add("ModifiedDateTime ", detail.ApprovedDateTime!.ToDashFromNull());
            makerChecker.Add("Status", detail.Status!);
            makerChecker.Add("Reject Reason", detail.RejectReason!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<ReqWarehouseUserDetailsModel>.Success(model);
        }
        catch(Exception ex)
        {
            return Result<ReqWarehouseUserDetailsModel>.Error(ex);
        }
    }

    #endregion
}
