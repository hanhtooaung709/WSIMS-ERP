using ERP.Warehouse.Models.Models.Dashboard.Box;
using ERP.Warehouse.Models.Models.Package.PackageList;

namespace ERP.Warehouse.Models.Models.Dashboard;

public class DashboardModel
{
    public List<int> StockQty {  get; set; }
    public List<string> BoxType { get; set; }
    public List<string> BoxCode { get; set; }
    public List<string> ProductName { get; set; }

    public List<PackageModel> Packages { get; set; } = new();
}
