using ERP.Warehouse.Models.Models;
using ERP.Warehouse.Models.Models.Signin.Signin;
using WSIMS_ERP.Shared;

namespace ERP.Warehouse.App.Components.Pages.SignIn;

public partial class SignIn
{
    private SigninReqModel reqModel = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {

        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }
}
