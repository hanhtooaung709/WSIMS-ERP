using ERP.Warehouse.Models.Models;
using ERP.Warehouse.Models.Models.Signin.Signin;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Components.Pages.SignIn;

public partial class SignIn
{
    private SigninReqModel _reqModel = new();

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
            await _injectService.ShowDialog(result);
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }
}
