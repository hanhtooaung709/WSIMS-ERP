namespace ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUserChanges;

public class ReqWarehouseUserChangesDetailsModel
{
    public List<DynamicReportModel> OldInfo { get; set; }
    public List<DynamicReportModel> UserInfo { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
}
