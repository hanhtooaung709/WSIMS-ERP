using ERP.Warehouse.Models.Models.Signin.Signin;
using Microsoft.AspNetCore.Components;
using WSIMS_ERP.Shared;

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
            if (firstRender)
            {

            }
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
