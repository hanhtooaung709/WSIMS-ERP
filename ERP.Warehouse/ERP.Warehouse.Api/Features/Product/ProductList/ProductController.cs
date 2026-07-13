using ERP.Warehouse.Api.Controller;
using ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Mvc;
using WSIMS_ERP.Shared.Services;

namespace ERP.Warehouse.Api.Features.Product.ProductList;

[Route("api/product-list")]
[ApiController]
public class ProductController : BaseController
{
    private readonly ProductListService _productListService;

    public ProductController(ProductListService productListService, ResponseService responseService) : base(responseService)
    {
        _productListService = productListService;
    }

    #region Get/Create/Edit/Update/Delete/Details

    [HttpPost]
    [Route("Get")]
    public async Task<IActionResult> Get(ProductReqModel reqModel)
    {
        var result = await _productListService.Get(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create(ProductReqModel reqModel)
    {
        var result = await _productListService.Create(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Edit")]
    public async Task<IActionResult> Edit(ProductEditModel reqModel)
    {
        var result = await _productListService.Edit(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Update")]
    public async Task<IActionResult> Update(ProductReqModel reqModel)
    {
        var result = await _productListService.Update(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ProductEditModel reqModel)
    {
        var result = await _productListService.Delete(reqModel);
        return await Execute(result);
    }

    [HttpPost]
    [Route("Details")]
    public async Task<IActionResult> Details(ProductEditModel reqModel)
    {
        var result = await _productListService.Details(reqModel);
        return await Execute(result);
    }

    #endregion
}
