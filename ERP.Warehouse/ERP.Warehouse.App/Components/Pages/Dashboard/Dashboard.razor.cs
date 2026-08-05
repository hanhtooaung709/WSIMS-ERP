using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using ERP.Warehouse.Models.Models.Dashboard.Box;
using MudBlazor;
using WSIMS_ERP.Shared;

namespace ERP.Warehouse.App.Components.Pages.Dashboard;

public partial class Dashboard
{
    private int _index = -1;
    private int _height = 350;
    private string[] _xAxisLabels = { "January", "February", "March", "April", "May", "June", "July", "August", "September" };
    private List<ChartSeries> _series = new();
    private List<string> _boxTypes = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await GetDashboardData();
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task GetDashboardData()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.GetDashboardData();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _boxTypes = result.Data!.BoxType! ?? new();
            _series = _boxTypes.Select(boxType => new ChartSeries
            {
                Name = boxType
            }).ToList();

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
