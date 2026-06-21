using ERP.Warehouse.Api.Common;
using Microsoft.Extensions.Options;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models.ConfigModel;
using ERP.Warehouse.Models.Models;
using Microsoft.EntityFrameworkCore;

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
            #region Check User Exist

            var user = await _db.TblWarehouseUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.DelFlag == 0 &&
                    x.UserName == reqModel.UserName);

            if (user is null)
            {
                return Result<SigninResModel>.Error("User does not exist.");
            }

            #endregion

            #region Check Password

            if (!user.LoginPassword.Equals(reqModel.Password))
            {
                return Result<SigninResModel>.Error("User Name or Password is wrong.");
            }

            #endregion
        }
        catch (Exception ex)
        {
            return Result<SigninResModel>.Error(ex);
        }

        return Result<SigninResModel>.Success(model);
    }
}
