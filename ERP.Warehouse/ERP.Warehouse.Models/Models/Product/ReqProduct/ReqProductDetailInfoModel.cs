namespace ERP.Warehouse.Models.Models.Product.ReqProduct;

public class ReqProductDetailInfoModel
{
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? SupplierName {  get; set; }
    public string? ImagePath { get; set; }
    public string? Status { get; set; }
    public string? RejectReason { get; set; }
    public string? ReqUser { get; set; }
    public string? ReqDateTime { get; set; }
    public string? ApprovedUser { get; set; }
    public string? ApprovedDateTime { get; set; }
}
