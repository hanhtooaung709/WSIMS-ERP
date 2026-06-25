using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared;
using Microsoft.EntityFrameworkCore;
using ERP.Warehouse.Models.Models.Signin.Signin;
using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Models.Models.Signin;
using WSIMS_ERP.Shared.Queries;
using WalletEbmb.Shared.Services;

namespace ERP.Warehouse.Api.Features.SignIn;

public class SignInService : AuthorizationService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly DapperService _dapperService;

    public SignInService(IHttpContextAccessor httpContextAccessor,
        AppDbContext db,
        JwtTokenHelper jwtTokenHelper,
         DapperService dapperService,
        ILogger<AuthorizationService> logger) : base(httpContextAccessor, logger)
    {
        _db = db;
        _jwtTokenHelper = jwtTokenHelper;
        _dapperService = dapperService;
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

            var sessionId = DevCode.GenerateUlid();

            #region Generate Token & Create Session

            var accessToken = _jwtTokenHelper.GenerateToken(user.WarehouseUserId, user.UserName);

            var session = new TblWarehouseUserSession
            {
                SessionId = sessionId,
                UserId = user.WarehouseUserId,
                SessionToken = accessToken,
                IsActive = true,
                LoginTime = DevCode.GetServerDateTime(),
                LogoutTime = new DateTime(1753, 1, 1)
            };
            _db.TblWarehouseUserSessions.Add(session);
            await _db.SaveChangesAsync();

            model.UserId = user.WarehouseUserId;
            model.SessionId = session.SessionId;
            model.AccessToken = accessToken;

            #endregion
        }
        catch (Exception ex)
        {
            return Result<SigninResModel>.Error(ex);
        }

        return Result<SigninResModel>.Success(model);
    }

    public async Task<Result<bool>> Logout(LogoutReqModel reqModel)
    {
        try
        {
            var session = await _db.TblWarehouseUserSessions
                .FirstOrDefaultAsync(x =>
                    x.SessionId == reqModel.SessionId &&
                    x.IsActive);

            if (session is null)
            {
                return Result<bool>.Success(true, "Session already inactive or not found.");
            }

            session.IsActive = false;
            session.LogoutTime = DevCode.GetServerDateTime();
            _db.Entry(session).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return Result<bool>.Success(true, "Logged out successfully.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error(ex);
        }
    }

    public async Task<Result<WarehouseUserInfoModel>> GetUserData()
    {
        WarehouseUserInfoModel model = new();
        try
        {
            IQueryable<TblWarehouseUser> user = _db.TblWarehouseUsers
                .AsNoTracking()
                .Where(x =>
                    x.DelFlag == 0 &&
                    x.WarehouseUserId == AuthorizedUserId);
            if (user is null)
            {
                return Result<WarehouseUserInfoModel>.Error("User does't exist!");
            }

            return Result<WarehouseUserInfoModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<WarehouseUserInfoModel>.Error(ex);
        }
    }
}