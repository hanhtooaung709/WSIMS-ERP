using ApexCharts;
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
    private int _height = 270;
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

    private List<RadarItem> _radarItems = new();
    private List<string> _radarBoxTypes = new();
    private ApexChartOptions<RadarItem> _radarOptions = new()
    {
        Chart = new Chart { Height = 350 },
        DataLabels = new DataLabels { Enabled = true }
    };

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

            _radarItems.Clear();
            _radarBoxTypes = boxes.Select(b => b.BoxType ?? "Unknown").Distinct().ToList();

            foreach (var prodName in _productNames)
            {
                _radarItems.Add(new RadarItem
                {
                    ProductName = prodName
                });
            }

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

                    double qty = Convert.ToDouble(matchedQty);
                    dataPerProduct[i] = qty;

                    var item = _radarItems.FirstOrDefault(r => r.ProductName == productName);
                    if (item != null && !string.IsNullOrEmpty(box.BoxType))
                    {
                        item.BoxQuantities[box.BoxType] = qty;
                    }
                }

                _series.Add(new ChartSeries
                {
                    Name = box.BoxType!,
                    Data = dataPerProduct
                });
            }

            #endregion

            #region GetCounts

            _model.ProductCount = result.Data.ProductCount;
            _model.PackageCount = result.Data.PackageCount;
            _model.StockCount = result.Data.StockCount;
            _model.PackageStockCount = result.Data.PackageStockCount;

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

    public class RadarItem
    {
        public string ProductName { get; set; } = string.Empty;
        public Dictionary<string, double> BoxQuantities { get; set; } = new();
    }
}
