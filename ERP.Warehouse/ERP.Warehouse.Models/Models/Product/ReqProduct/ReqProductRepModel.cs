namespace ERP.Warehouse.Models.Models.Product.ReqProduct;

public class ReqProductRepModel
{
    public List<ReqProductModel> list { get; set; }
}

public class ReqProductModel
{
    public string? ReqProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? Status { get; set; }
}