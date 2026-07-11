using DocumentFormat.OpenXml.Drawing.Diagrams;
using ERP.Warehouse.Models.Models.Product.ProductList;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;

namespace ERP.Warehouse.App.Components.Pages.Product.ProductList;

public partial class ProductList
{
    private ProductReqModel _reqModel = new();
    private IEnumerable<ProductModel> _model = new List<ProductModel>();
    private ProductEditModel _edit = new();
    private ProductDetailModel _details = new();

    private MudDataGrid<ProductModel> _elementGrid = default!;
    private EnumFormType _formType = EnumFormType.List;
    private bool hover = true;
    private bool _readOnly;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                await List();
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    #region Get/Create/Edit/Update/Delete/Details

    private async Task List()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.Get(_reqModel);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                _reqModel = new();
                return;
            }

            _model = result.Data!.list!;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Create()
    {
        var modle = new ProductModel();
        await _elementGrid.SetEditingItemAsync(modle);
        _formType = EnumFormType.Create;
    }

    private async Task Save(ProductModel reqModel)
    {
        try
        {
            #region Create

            if (_reqModel.ProductId.IsNullOrEmpty())
            {
                bool confirm = await _injectService.ShowCreateDialog();
                if (!confirm) return;

                await _injectService.EnableLoading();
                var result = await _apiService.Create(_reqModel);
                await _injectService.DisableLoading();

                if (result.IsError)
                {
                    await _injectService.ShowDialog(result);
                    _reqModel = new();
                    return;
                }
                await _injectService.ShowDialog(result);
            }

            #endregion

            #region Update

            else
            {
                bool confirm = await _injectService.ShowUpdateDialog();
                if (!confirm) return;

                await _injectService.EnableLoading();
                var result = await _apiService.Update(_reqModel);
                await _injectService.DisableLoading();

                if (result.IsError)
                {
                    await _injectService.ShowDialog(result);
                    _reqModel = new();
                    return;
                }
                await _injectService.ShowDialog(result);
            }

            #endregion

            _reqModel = new();
            await List();
        }

        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Edit(ProductModel reqModel)
    {
        try
        {
            if (reqModel is null || string.IsNullOrEmpty(reqModel.ProductId))
            {
                _formType = EnumFormType.Create;
                return;
            }

            _edit.ProductId = reqModel.ProductId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Edit(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _reqModel.ProductId = result.Data.ProductId;
            _reqModel.ProductName = result.Data.ProductName;
            _reqModel.ProductCode = result.Data.ProductCode;
            _reqModel.SupplierName = result.Data.SupplierName;

            _formType = EnumFormType.Edit;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private void Cancel()
    {
        try
        {
            _reqModel = new();
            StateHasChanged();
            List();
            _formType = EnumFormType.List;
        }
        catch (Exception ex)
        {
            _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Delete(ProductModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.Confirm();
            if (!confirm)
            {
                return;
            }

            _edit.ProductId = reqModel.ProductId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Delete(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }
            await _injectService.ShowDialog(result);

            await List();
            _formType = EnumFormType.List;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Details(ProductModel reqModel)
    {
        try
        {
            _edit.ProductId = reqModel.ProductId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Details(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _details = result.Data!;
            _formType = EnumFormType.Detail;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    #endregion
}
