namespace ERP.Warehouse.Models.Models.Product.ReqProductChanges;

public class ReqProductChangesDetailInfoModel
{
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? ChangesType { get; set; }
    public string? SupplierName { get; set; }
    public string? ImagePath { get; set; }
    public string? Status { get; set; }
    public string? RejectReason { get; set; }
    public string? ReqUser { get; set; }
    public string? ReqDateTime { get; set; }
    public string? ApprovedUser { get; set; }
    public string? ApprovedDateTime { get; set; }
}
