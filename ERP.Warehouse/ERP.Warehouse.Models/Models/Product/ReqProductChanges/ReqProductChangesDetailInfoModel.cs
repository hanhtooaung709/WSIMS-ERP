namespace ERP.Warehouse.Models.Models.Product.ReqProductChanges;

public class ReqProductChangesDetailInfoModel
{
    // -- Change data --
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? ChangesType { get; set; }
    public string? SupplierName { get; set; }

    // -- Old data --
    public string? OldName { get; set; }
    public string? OldCode { get; set; }
    public string? OldSupplierName { get; set; }

    // -- Maker/Chacker --
    public string? Status { get; set; }
    public string? RejectReason { get; set; }
    public string? ReqUser { get; set; }
    public string? ReqDateTime { get; set; }
    public string? ApprovedUser { get; set; }
    public string? ApprovedDateTime { get; set; }
}
