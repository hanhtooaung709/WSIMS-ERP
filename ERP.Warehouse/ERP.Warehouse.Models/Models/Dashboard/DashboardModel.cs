using ERP.Warehouse.Models.Models.Dashboard.Box;

namespace ERP.Warehouse.Models.Models.Dashboard;

public class DashboardModel
{
    public List<int> StockQty {  get; set; }
    public List<string> BoxType { get; set; }
    public List<string> ProductName { get; set; }
}
