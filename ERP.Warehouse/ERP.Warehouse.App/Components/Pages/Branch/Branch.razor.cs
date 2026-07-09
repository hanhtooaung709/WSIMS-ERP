using ERP.Warehouse.Models.Models.Branch;
using WSIMS_ERP.Shared;

namespace ERP.Warehouse.App.Components.Pages.Branch;

public partial class Branch
{
    private IEnumerable<BranchModel> _model = new List<BranchModel>();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await List();
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task List()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.Get();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _model = result.Data!.list!;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }
}
