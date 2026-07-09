namespace ERP.Warehouse.Models.Models.Box;

public class BoxRepModel
{
    public List<BoxModel> list { get; set; }
}

public class BoxModel
{
    public string? BoxId { get; set; }
    public string? BoxCode { get; set; }
    public string? Type { get; set; }
    public string? TareWeight { get; set; }
    public string? MaxNetWeight { get; set; }
}