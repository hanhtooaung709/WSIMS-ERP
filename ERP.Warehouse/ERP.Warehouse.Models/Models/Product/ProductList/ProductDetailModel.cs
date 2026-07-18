namespace ERP.Warehouse.Models.Models.Product.ProductList;

public class ProductDetailModel
{
    public List<DynamicReportModel> ProductInfo { get; set; }
    public List<DynamicReportModel> MakerChecker { get; set; }
    public string? ItemImagePath { get; set; }
}
