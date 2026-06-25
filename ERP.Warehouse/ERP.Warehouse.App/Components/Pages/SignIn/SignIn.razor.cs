using ERP.Warehouse.Models.Models.Signin.Signin;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WSIMS_ERP.Shared;
using ERP.Warehouse.App.Services.Security;

namespace ERP.Warehouse.App.Components.Pages.SignIn;

public partial class SignIn
{
    private SigninReqModel _reqModel = new();

    [Inject]
    private NavigationManager _navigationManager { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender) { }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Login()
    {
        try
        {
            string hashPassword = PasswordHash.SHA256HexHashString(_reqModel.Password, _reqModel.UserName);
            await _injectService.EnableLoading();
            var result = await _apiService.SignIn(new SigninReqModel
            {
                UserName = _reqModel.UserName,
                Password = hashPassword
            });
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            #region Chack Token

            if (!string.IsNullOrEmpty(result.Data.AccessToken))
            {
                await _injectService.SetSessionStorage("AccessToken", result.Data.AccessToken.ToEncrypt());

                var customProvider = (CustomAuthStateProvider)_authStateProvider;
                customProvider.NotifyUserLogin(result.Data.AccessToken);
            }
            else
            {
                _logger.LogError("The AccessToken provided by the API is null!");
            }

            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
            {
                await _injectService.SetSessionStorage("RefreshToken", result.Data.RefreshToken.ToEncrypt());
            }

            if (!string.IsNullOrEmpty(result.Data.SessionId))
            {
                await _injectService.SetSessionStorage("SessionId", result.Data.SessionId);
            }

            #endregion

            _navigationManager.NavigateTo("/dashboard");
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }
}