namespace ERP.Warehouse.Models.Models.Product.ReqProduct;

public class ReqProductDetailModel
{
    public List<DynamicReportModel> ProductInfo { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
    public string? ItemImagePath { get; set; }
}
