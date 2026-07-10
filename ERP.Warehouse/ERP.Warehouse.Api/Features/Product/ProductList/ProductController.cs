using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Warehouse.Api.Features.Product.ProductList;

[Route("api/product-list")]
[ApiController]
public class ProductController : BaseController
{
    private readonly ProductListService _productListService;

    public ProductController(ProductListService productListService)
    {
        _productListService = productListService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ProductReqModel reqModel)
    {
        var result = await _productListService.Get(reqModel);
        return Execute(result);
    }

    #endregion
}
