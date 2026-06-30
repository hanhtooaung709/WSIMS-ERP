using ERP.Warehouse.Models.Models.WarehouseUserList;
using Microsoft.JSInterop;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared.Models.PageSetting;
using static MudBlazor.CategoryTypes;

namespace ERP.Warehouse.App.Components.Pages.WarehouseUser.WarehouseUserList;

public partial class WarehouseUserList
{
    private WarehouseUserReqModel _reqModel = new();
    private IEnumerable<WarehouseUserModel> _model = new List<WarehouseUserModel>();
    private bool hover = true;
    private bool _readOnly;

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
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private void Edit(WarehouseUserModel item)
    {

    }

    private void Cancel(WarehouseUserModel item)
    {

    }
    private async Task Save(WarehouseUserModel item)
    {

    }
}
