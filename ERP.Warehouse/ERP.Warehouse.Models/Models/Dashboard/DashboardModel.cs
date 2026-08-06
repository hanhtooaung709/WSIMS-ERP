using ERP.Warehouse.Models.Models.Dashboard.Box;
using ERP.Warehouse.Models.Models.Package.PackageList;

namespace ERP.Warehouse.Models.Models.Dashboard;

public class DashboardModel
{
    public List<int> StockQty {  get; set; }
    public List<BoxResponseModel> Boxes { get; set; } = new();
    public List<string> ProductName { get; set; }
    public List<PackageModel> Packages { get; set; } = new();
    public int ProductCount { get; set; }
    public int PackageCount { get; set; }
    public int StockCount { get; set; }
}
