namespace ERP.Warehouse.Models.Models.Product.ReqProductChanges;

public class ReqProductChangesDetailModel
{
    public List<DynamicReportModel> OldInfo { get; set; }
    public List<DynamicReportModel> ProductInfo { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
}
