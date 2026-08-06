using Microsoft.JSInterop;
using MudBlazor;
using WSIMS_ERP.Shared;

namespace ERP.Warehouse.App.Components.Pages.Dashboard;

public partial class Dashboard
{
    private int _index = -1;
    private int _height = 420;
    private AxisChartOptions _chartOptions = new()
    {
        XAxisLabelRotation = 90,
        MatchBoundsToSize = true
    };
    private List<ChartSeries> _series = new();
    private List<string> _boxTypes = new();
    private string[] _productNames = Array.Empty<string>();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await GetDashboardData();
            }

            await JSRuntime.InvokeVoidAsync("fixMudChartXAxisLabels");
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

            #region GetStock



            #endregion

            #region GetProductName

            var productList = result.Data?.ProductName ?? new List<string>();
            _productNames = productList.ToArray();

            #endregion

            #region GetBoxType

            _boxTypes = result.Data!.BoxType! ?? new();
            _series = _boxTypes.Select(boxType => new ChartSeries
            {
                Name = boxType,
                Data = new double[_productNames.Length]
            }).ToList();

            #endregion

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
