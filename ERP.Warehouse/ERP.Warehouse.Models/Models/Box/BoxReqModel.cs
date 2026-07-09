namespace ERP.Warehouse.Models.Models.Box;

public class BoxReqModel
{
    public string? BoxId { get; set; }
    public string? BoxCode { get; set; }
    public string? Type { get; set; }
    public string? Size { get; set;}
    public string? TareWeight { get; set; }
    public string? MaxNetWeight { get; set; }
}
