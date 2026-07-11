namespace ERP.Warehouse.Models.Models.Product.ProductList;

public class ProductRepModel
{
    public List<ProductModel> list { get; set; }
}

public class ProductModel
{
    public string? ProductId { get; set; }
    public string? ProductName { get; set;}
    public string? ProductCode { get; set;}
    public string? SupplierName { get; set;}
}