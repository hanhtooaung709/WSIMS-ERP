namespace ERP.Warehouse.Models.Models.Currency;

public class CurrencyRepModel
{
    public List<CurrencyModel> list { get; set; }
}

public class CurrencyModel
{
    public string? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencyDes { get; set; }
}
