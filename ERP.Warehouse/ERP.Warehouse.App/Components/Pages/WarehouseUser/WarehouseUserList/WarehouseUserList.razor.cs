using ERP.Warehouse.Models.Models.WarehouseUserList;
using Microsoft.JSInterop;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.PageSetting;
using static MudBlazor.CategoryTypes;

namespace ERP.Warehouse.App.Components.Pages.WarehouseUser.WarehouseUserList;

public partial class WarehouseUserList
{
    private WarehouseUserReqModel _reqModel = new();
    private WarehouseUserRepModel _repModel = new();
    private IEnumerable<WarehouseUserModel> _model = new List<WarehouseUserModel>();
    private bool hover = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                List();
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    async Task List()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.Get(_reqModel);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _model = result.Data!.list!;
            StateHasChanged();
        }
        catch(Exception ex)
        {

        }
    }
}
