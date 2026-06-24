using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared;
using Microsoft.EntityFrameworkCore;
using ERP.Warehouse.Models.Models.Signin.Signin;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Signin;

namespace ERP.Warehouse.Api.Features.SignIn;

public class SignInService : AuthorizationService
{
    private readonly AppDbContext _db;

    public SignInService(IHttpContextAccessor httpContextAccessor, 
        AppDbContext db) : base(httpContextAccessor)
    {
        _db = db;
    }

    public async Task<Result<SigninResModel>> SignIn(SigninReqModel reqModel)
    {
        Result<SigninResModel> responseModel = new();
        SigninResModel model = new();

        try
        {
            #region Check User

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.DelFlag == 0 &&
                    x.UserName == reqModel.UserName);

            if (user is null)
            {
                return Result<SigninResModel>.Error("User does not exist.");
            }

            if (user.LockFlag)
            {
                return Result<SigninResModel>.Error("Your account is lock.");
            }

            if (!user.LoginPassword.Equals(reqModel.Password))
            {
                user.LoginFailCount += 1;

                if (user.LoginFailCount == 3)
                {
                    user.LockFlag = true;
                    _db.Entry(user).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return Result<SigninResModel>.Error("Your account is lock.");
                }

                _db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Result<SigninResModel>.Error("User Name or Password is wrong.");
            }

            #endregion

            #region Update LoginFailCount

            user.LoginFailCount = 0;
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            #endregion
        }
        catch (Exception ex)
        {
            return Result<SigninResModel>.Error(ex);
        }

        return Result<SigninResModel>.Success(model);
    }

    public async Task<Result<WarehouseUserInfoListModel>> GetUserData()
    {
        WarehouseUserInfoListModel model = new();
        try
        {
            IQueryable<TblWarehouseUser> user = _db.TblWarehouseUsers
                .AsNoTracking()
                .Where(x =>
                    x.DelFlag == 0 &&
                    x.WarehouseUserId == AuthorizedUserId);
            if (user is null)
            {
                return Result<WarehouseUserInfoListModel>.Error("User does't exist!");
            }

            var result = await user
                .AsNoTracking()
                .Select(x => new WarehouseUserInfoModel
                {
                    FullName = x.FullName,
                    StaffId = x.StaffId,
                    BranchCode = x.BranchCode,
                    RoleCode = x.RoleCode
                })
                .ToListAsync();

            model.lstData = result;
            return Result<WarehouseUserInfoListModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<WarehouseUserInfoListModel>.Error(ex);
        }
    }
}