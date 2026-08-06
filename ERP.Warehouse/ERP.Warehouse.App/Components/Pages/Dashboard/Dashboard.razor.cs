using ERP.Warehouse.Models.Models.Dashboard;
using ERP.Warehouse.Models.Models.Dashboard.Box;
using ERP.Warehouse.Models.Models.Package.PackageList;
using Microsoft.JSInterop;
using MudBlazor;
using WSIMS_ERP.Shared;

namespace ERP.Warehouse.App.Components.Pages.Dashboard;

public partial class Dashboard
{
    private int _index = -1;
    private int _height = 350;
    private AxisChartOptions _chartOptions = new()
    {
        XAxisLabelRotation = 90,
        MatchBoundsToSize = true
    };
    private DashboardModel _model = new();
    private List<ChartSeries> _series = new();
    private string[] _productNames = Array.Empty<string>();

    private double StockPackagePercentage => _model.PackageCount > 0
        ? ((double)_model.PackageStockCount / _model.PackageCount) * 100
        : 0;

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

            if (result.IsError || result.Data == null)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            #region GetProductName

            var productList = result.Data.ProductName ?? new List<string>();
            _productNames = productList.ToArray();

            #endregion

            #region GetBoxType & Stock Mapping

            var boxes = result.Data.Boxes ?? new List<BoxResponseModel>();
            var packages = result.Data.Packages ?? new List<PackageModel>();

            _series = new List<ChartSeries>();

            foreach (var box in boxes)
            {
                if (string.IsNullOrEmpty(box.BoxCode)) continue;

                var dataPerProduct = new double[_productNames.Length];

                for (int i = 0; i < _productNames.Length; i++)
                {
                    string productName = _productNames[i];

                    var matchedQty = packages
                        .Where(p => (p.ProductCode == productName || p.PackageName == productName)
                                 && p.BoxCode == box.BoxCode)
                        .Sum(p => p.Quantity);

                    dataPerProduct[i] = Convert.ToDouble(matchedQty);
                }

                _series.Add(new ChartSeries
                {
                    Name = box.BoxType!,
                    Data = dataPerProduct
                });
            }

            #endregion

            #region GetProductCount

            var productCount = result.Data.ProductCount;
            _model.ProductCount = productCount;

            #endregion

            #region GetPackageCount

            var packageCount = result.Data.PackageCount;
            _model.PackageCount = packageCount;

            #endregion

            #region GetStockCount

            var stockCount = result.Data.StockCount;
            _model.StockCount = stockCount;

            #endregion

            #region GetStockPackageCount

            var packageStockCount = result.Data.PackageStockCount;
            _model.PackageStockCount = packageStockCount;

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
