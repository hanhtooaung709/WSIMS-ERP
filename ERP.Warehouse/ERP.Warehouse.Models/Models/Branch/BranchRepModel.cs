namespace ERP.Warehouse.Models.Models.Branch;

public class BranchRepModel
{
    public List<BranchModel> list {  get; set; }
}

public class BranchModel
{
    public string? StateName { get; set; }
    public string? CityName { get; set; }
    public string? TownshipName { get; set; }
    public string? Address { get; set; }
    public int UserCount { get; set; }
}
