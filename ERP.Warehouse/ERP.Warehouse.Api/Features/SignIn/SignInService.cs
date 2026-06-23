using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared;
using Microsoft.EntityFrameworkCore;
using ERP.Warehouse.Models.Models.Signin.Signin;

namespace ERP.Warehouse.Api.Features.SignIn;

public class SignInService
{
    private readonly AppDbContext _db;

    public SignInService(AppDbContext db)
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
                    _db.SaveChanges();
                    return Result<SigninResModel>.Error("Your account is lock.");
                }

                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
                return Result<SigninResModel>.Error("User Name or Password is wrong.");
            }

            #endregion

            #region Update LoginFailCount

            user.LoginFailCount = 0;
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();

            #endregion
        }
        catch (Exception ex)
        {
            return Result<SigninResModel>.Error(ex);
        }

        return Result<SigninResModel>.Success(model);
    }
}
