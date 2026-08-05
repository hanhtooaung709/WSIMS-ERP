using MudBlazor;

namespace ERP.Warehouse.App.Components.Pages.Dashboard;

public partial class Dashboard
{
    private int _index = -1;
    private int _height = 350;
    private bool _matchBoundsToSize = false;
    private List<ChartSeries> _series = new()
    {
        new() { Name = "United States", Data = new double[] { 40, 20, 25, 27, 46, 60, 48, 80, 15 } },
        new() { Name = "Germany", Data = new double[] { 19, 24, 35, 13, 28, 15, 13, 16, 31 } },
        new() { Name = "Sweden", Data = new double[] { 8, 6, 11, 13, 4, 16, 10, 16, 18 } },
    };
    private string[] _xAxisLabels = { "January", "February", "March", "April", "May", "June", "July", "August", "September" };
}
