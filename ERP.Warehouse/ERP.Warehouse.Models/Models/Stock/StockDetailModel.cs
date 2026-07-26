namespace ERP.Warehouse.Models.Models.Stock;

public class StockDetailModel
{
    public List<DynamicReportModel> Package { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
    public string? ItemImagePath { get; set; }
}
