using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUserChanges;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models.DynamicModel;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Queries;
using WSIMS_ERP.Shared.Services;
using ERP.Warehouse.Models;
using System.Data;

namespace ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUserChanges;

public class ReqWarehouseUserChangesService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly DapperService _dapperService;

    public ReqWarehouseUserChangesService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _dapperService = dapperService;
    }

    #region Get/Edit/Update/Delete/Details

    public async Task<Result<ReqWarehouseUserChangesRepModel>> Get(ReqWarehouseUserChangesReqModel reqModel)
    {
        ReqWarehouseUserChangesRepModel model = new();
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
            var result = await _dapperService.QueryStoredProcedureAsync<ReqWarehouseUserChangesModel>
                (SqlQueries.Sp_GetReqWarehouseUserChangesList, parameters);
            model.list = result;
            return Result<ReqWarehouseUserChangesRepModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserChangesRepModel>.Error(ex);
        }
    }

    public async Task<Result<ReqWarehouseUserChangesModel>> Edit(ReqWarehouseUserChangesEditModel reqModel)
    {
        var model = new Result<ReqWarehouseUserChangesModel>();
        try
        {
            #region Check User
            var user = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqWarehouseUserChangesId == reqModel.UserId);
            if (user is null)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE001);
                return model;
            }
            #endregion

            #region Check Role
            var role = await _db.TblWarehouseRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleCode == user.RoleCode && x.DelFlag == 0);
            if (role is null)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE015);
                return model;
            }
            #endregion

            #region Check Branch
            var branch = await _db.TblBranches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BranchCode == user.BranchCode && x.DelFlag == 0);
            if (branch is null)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE016);
                return model;
            }
            #endregion

            #region Prepare Data

            var response = new ReqWarehouseUserChangesModel
            {
                ReqWarehouseUserChangesId = user.ReqWarehouseUserChangesId,
                WarehouseUserId = user.WarehouseUserId,
                FullName = user.FullName,
                Phone = user.Phone,
                Email = user.Email,
                RoleCode = user.RoleCode,
                BranchCode = user.BranchCode
            };
            model = Result<ReqWarehouseUserChangesModel>.Success(response);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserChangesModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqWarehouseUserChangesModel>> Update(ReqWarehouseUserChangesReqModel reqModel)
    {
        var model = new Result<ReqWarehouseUserChangesModel>();
        try
        {
            #region Check User

            TblReqWarehouseUserChange? user = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqWarehouseUserChangesId == reqModel.UserId);
            if (user is null)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE024);
                return model;
            }

            bool reqUser = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqWarehouseUserChangesId == reqModel.UserId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE025);
                return model;
            }
            #endregion

            #region Check Duplicate UserName


            bool userName = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName.Trim().ToLower() == reqModel.UserName!.Trim().ToLower() &&
                          x.WarehouseUserId != reqModel.WarehouseUserId);
            if (userName)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE004);
                return model;
            }

            userName = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.UserName.Trim().ToLower() == reqModel.UserName!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (userName)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE005);
                return model;
            }


            #endregion

            #region Check Duplicate StaffId

            bool staffId = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.StaffId.Trim().ToLower() == reqModel.StaffId!.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.WarehouseUserId);
            if (staffId)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE006);
                return model;
            }

            staffId = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.StaffId.Trim().ToLower() == reqModel.StaffId!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (staffId)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE007);
                return model;
            }

            #endregion

            #region Check Duplicate PhoneNo

            bool phoneNo = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower() &&
                          x.WarehouseUserId != reqModel.WarehouseUserId);
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE008);
                return model;
            }

            phoneNo = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE009);
                return model;
            }

            phoneNo = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Phone.Trim().ToLower() == reqModel.Phone!.Trim().ToLower() &&
                               x.ReqWarehouseUserChangesId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (phoneNo)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE010);
                return model;
            }


            #endregion

            #region Check Duplicate Email

            bool email = await _db.TblWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.WarehouseUserId != reqModel.WarehouseUserId);
            if (email)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE011);
                return model;
            }

            email = await _db.TblReqWarehouseUsers
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE012);
                return model;
            }

            email = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.Email.Trim().ToLower() == reqModel.Email!.Trim().ToLower() &&
                               x.ReqWarehouseUserChangesId != reqModel.UserId &&
                               x.Status == EnumRequestedStatus.Pending.ToString());
            if (email)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE013);
                return model;
            }



            #endregion

            #region Check Change Data

            if (user.FullName == reqModel.FullName && user.Phone == reqModel.Phone &&
                user.Email == reqModel.Email && user.RoleCode == reqModel.RoleCode &&
                user.BranchCode == reqModel.BranchCode)
            {
                model = Result<ReqWarehouseUserChangesModel>.Warning(JsonResource.WHE101);
                return model;
            }

            #endregion

            #region Prepare Data

            user.FullName = reqModel.FullName!;
            user.Phone = reqModel.Phone!;
            user.Email = reqModel.Email!;
            user.RoleCode = reqModel.RoleCode!;
            user.BranchCode = reqModel.BranchCode!;
            user.ReqDateTime = DevCode.GetServerDateTime();

            _db.Entry(user).State = EntityState.Modified;
            _db.TblReqWarehouseUserChanges.Update(user);
            await _db.SaveChangesAsync();
            model = Result<ReqWarehouseUserChangesModel>.Success(JsonResource.WHS026);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserChangesModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqWarehouseUserChangesModel>> Delete(ReqWarehouseUserChangesEditModel reqModel)
    {
        var model = new Result<ReqWarehouseUserChangesModel>();
        try
        {
            #region Check User

            var user = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ReqWarehouseUserChangesId == reqModel.UserId);
            if (user is null)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE024);
                return model;
            }

            bool reqUser = await _db.TblReqWarehouseUserChanges
                .AsNoTracking()
                .AnyAsync(x => x.ReqWarehouseUserChangesId == reqModel.UserId &&
                               x.Status != EnumRequestedStatus.Pending.ToString());
            if (reqUser)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE025);
                return model;
            }

            #endregion

            #region Prepare Data

            _db.TblReqWarehouseUserChanges.Remove(user);
            var result = _db.SaveChanges();
            if (result <= 0)
            {
                model = Result<ReqWarehouseUserChangesModel>.Error(JsonResource.WHE027);
                return model;
            }
            model = Result<ReqWarehouseUserChangesModel>.Success(JsonResource.WHS028);

            #endregion
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserChangesModel>.Error(ex);
        }
        return model;
    }

    public async Task<Result<ReqWarehouseUserChangesDetailsModel>> Details(ReqWarehouseUserChangesEditModel reqModel)
    {
        ReqWarehouseUserChangesDetailsModel model = new();
        try
        {
            var detail = await _dapperService.GetDetailAsync<ReqWarehouseUserChangesDetailsInfoModel>(
                SqlQueries.Sp_GetReqWarehouseUserChangesDetail, new
                {
                    UserId = reqModel.UserId
                }, CommandType.StoredProcedure);

            List<DynamicReportModel> oldInfo = new List<DynamicReportModel>();
            oldInfo.Add("User Name", detail.OldName!);
            oldInfo.Add("Full Name", detail.OldFullName!);
            oldInfo.Add("Staff Id", detail.OldStaffId!);
            oldInfo.Add("Role ", detail.OldRole!);
            oldInfo.Add("Branch ", detail.OldBranch!);
            oldInfo.Add("Phone", detail.OldPhone!);
            oldInfo.Add("Email", detail.OldEmail!);
            model.OldInfo = oldInfo;

            List<DynamicReportModel> userInfo = new List<DynamicReportModel>();
            userInfo.Add("User Name", detail.UserName!);
            userInfo.Add("Full Name", detail.FullName!);
            userInfo.Add("Staff Id", detail.StaffId!);
            userInfo.Add("Role ", detail.RoleName!);
            userInfo.Add("Branch ", detail.BranchName!);
            userInfo.Add("Phone", detail.Phone!);
            userInfo.Add("Email", detail.Email!);
            userInfo.Add("Changes Type", detail.ChangesType!);
            model.UserInfo = userInfo;

            List<DynamicReportModel> makerChecker = new List<DynamicReportModel>();
            makerChecker.Add("CreatedUser", detail.RequestedUser!);
            makerChecker.Add("CreatedDateTime", detail.RequestedDateTime!);
            makerChecker.Add("Modified User", detail.ApprovedUser!.ToDashFromNull());
            makerChecker.Add("ModifiedDateTime ", detail.ApprovedDateTime!.ToDashFromNull());
            makerChecker.Add("Status", detail.Status!);
            makerChecker.Add("Reject Reason", detail.RejectReason!.ToDashFromNull());
            model.MakerChecker = makerChecker;

            return Result<ReqWarehouseUserChangesDetailsModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<ReqWarehouseUserChangesDetailsModel>.Error(ex);
        }
    }

    #endregion
}
