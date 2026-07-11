namespace ERP.Warehouse.Models.Models.Product.ReqProductChanges;

public class ReqProductChangesRepModel
{
    public List<ReqProductChangesModel> list {  get; set; }
}

public class ReqProductChangesModel
{
    public string? ReqProductChangesId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? SupplierName { get; set; }
    public string? ChangesType { get; set; }
    public string? Status { get; set; }
}
