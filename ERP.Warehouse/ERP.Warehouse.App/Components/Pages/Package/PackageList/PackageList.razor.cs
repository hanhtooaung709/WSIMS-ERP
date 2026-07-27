using ERP.Warehouse.App.Common;
using ERP.Warehouse.Models.Models.Package.PackageList;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Components.Pages.Package.PackageList;

public partial class PackageList
{
    private PackageReqModel _reqModel = new();
    private IEnumerable<PackageModel> _model = new List<PackageModel>();
    private PackageEditModel _edit = new();
    private PackageDetailModel _details = new();

    private List<BranchResponseModel> _banchList = new();
    private List<ProductResponseModel> _productList = new();
    private List<CurrencyResponseModel> _currencyList = new();
    private List<BoxResponseModel> _boxList = new();

    private List<SelectListModel> _lstStatus = Commons.GetStatusList();

    private PackageModel? selectedItem;
    private MudDataGrid<PackageModel> _elementGrid = default!;
    private EnumFormType _formType = EnumFormType.List;
    private bool hover = true;
    private bool _readOnly;

    private IList<IBrowserFile> _selectedFiles = new List<IBrowserFile>();
    private string? _imagePreviewUrl;
    private string? _existingImagePath;


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
            await GetProduct();
            await GetBox();

            await _injectService.EnableLoading();
            var result = await _apiService.Get(_reqModel);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                _reqModel = new();
                selectedItem = null;
                return;
            }

            selectedItem = null;
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
        var modle = new PackageModel();
        _banchList = new List<BranchResponseModel>();
        await GetBranch();
        _productList = new List<ProductResponseModel>();
        await GetProduct();
        _currencyList = new List<CurrencyResponseModel>();
        await GetCurrency();
        _boxList = new List<BoxResponseModel>();
        await GetBox();
        await _elementGrid.SetEditingItemAsync(modle);
        _formType = EnumFormType.Create;
    }

    private async Task Save(PackageModel reqModel)
    {
        try
        {
            #region Create

            if (_reqModel.PackageInfoId.IsNullOrEmpty() && _formType == EnumFormType.Create)
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
                    selectedItem = null;
                    return;
                }
                await _injectService.ShowDialog(result);
            }

            #endregion

            #region Update

            else if (!_reqModel.PackageInfoId.IsNullOrEmpty() && _formType == EnumFormType.Edit)
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
                    selectedItem = null;
                    return;
                }
                await _injectService.ShowDialog(result);
            }

            #endregion

            #region StockModifly

            else if (_formType == EnumFormType.StockModifly)
            {
                bool confirm = await _injectService.ShowUpdateDialog();
                if (!confirm) return;

                await _injectService.EnableLoading();
                var result = await _apiService.StockModifly(_reqModel);
                await _injectService.DisableLoading();

                if (result.IsError)
                {
                    await _injectService.ShowDialog(result);
                    _reqModel = new();
                    selectedItem = null;
                    return;
                }
                await _injectService.ShowDialog(result);
            }

            #endregion

            _reqModel = new();
            selectedItem = null;
            await List();
        }

        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task OnEditClicked()
    {
        if (selectedItem != null && _elementGrid != null)
        {
            await _elementGrid.SetEditingItemAsync(selectedItem);
        }
    }

    private async Task OnStockModifly()
    {
        if (selectedItem != null && _elementGrid != null)
        {
            await _elementGrid.SetEditingItemAsync(selectedItem);
        }
        _formType = EnumFormType.StockModifly;
        _reqModel.Quantity = 0;
    }

    private async Task Edit(PackageModel reqModel)
    {
        try
        {
            if (reqModel is null || string.IsNullOrEmpty(reqModel.PackageInfoId))
            {
                _formType = EnumFormType.Create;
                return;
            }

            await GetBranch();
            await GetProduct();
            await GetCurrency();
            await GetBox();
            _edit.PackageInfoId = reqModel.PackageInfoId!;
            _edit.PackageId = reqModel.PackageId!;
            _edit.PackageInfoCode = reqModel.PackageInfoCode!;

            await _injectService.EnableLoading();
            var result = await _apiService.Edit(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _reqModel.PackageInfoId = result.Data.PackageInfoId;
            _reqModel.PackageId = result.Data.PackageId;
            _reqModel.PackageName = result.Data.PackageName;
            _reqModel.PackageInfoCode = result.Data.PackageInfoCode;
            _reqModel.BranchCode = result.Data.BranchCode;
            _reqModel.Quantity = result.Data.Quantity;
            _reqModel.ProductCode = result.Data.ProductCode;
            _reqModel.Price = result.Data.Price;
            _reqModel.CurrencyCode = result.Data.CurrencyCode;
            _reqModel.Weight = result.Data.Weight;
            _reqModel.BoxCode = result.Data.BoxCode;

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
            selectedItem = null;
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

    private async Task Delete(PackageModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.Confirm();
            if (!confirm)
            {
                return;
            }

            _edit.PackageInfoId = reqModel.PackageInfoId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Delete(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                selectedItem = null;
                return;
            }
            await _injectService.ShowDialog(result);

            selectedItem = null;
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

    private async Task Details(PackageModel reqModel)
    {
        try
        {
            _edit.PackageInfoId = reqModel.PackageInfoId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Details(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                selectedItem = null;
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

    private void OnRowClick(TableRowClickEventArgs<PackageModel> args)
    {

    }

    private void OnSingleSelect(bool isChecked, PackageModel item)
    {
        if (isChecked)
        {
            selectedItem = item;
        }
        else
        {
            selectedItem = null;
        }
    }

    private string GetBadgeClass(string status)
    {
        var statusStr = "";
        try
        {
            statusStr = _injectService.GetBadgeClass(status.ToEnum<EnumRequestedStatus>());
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            _ = _injectService.ErrorDialogMessage(ex.Message);
        }

        return statusStr;
    }

    private string GetImageUrl(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath)) return "";
        var folder = Path.GetFileName(Path.GetDirectoryName(imagePath));
        var fileName = Path.GetFileName(imagePath);
        var apiUrl = _setting?.CurrentValue?.WarehouseApp?.WarehouseApiBaseUrl;
        var baseUrl = string.IsNullOrEmpty(apiUrl) ? _nav.BaseUri.TrimEnd('/') : apiUrl.TrimEnd('/');
        return $"{baseUrl}/api/image/{folder}/{fileName}";
    }

    private void OnProductCodeChanged(string value)
    {
        _reqModel.ProductCode = value;
        UpdatePackageName();
    }

    private void OnBoxCodeChanged(string value)
    {
        _reqModel.BoxCode = value;
        UpdatePackageName();
    }

    private void UpdatePackageName()
    {
        var productName = _productList?.FirstOrDefault(x => x.ProductCode == _reqModel.ProductCode)?.ProductName;

        var boxName = _boxList?.FirstOrDefault(x => x.BoxCode == _reqModel.BoxCode)?.Box;

        if (!string.IsNullOrEmpty(productName) || !string.IsNullOrEmpty(boxName))
        {
            _reqModel.PackageName = $"{productName} ({boxName})";
        }
        else
        {
            _reqModel.PackageName = string.Empty;
        }
    }

    #endregion

    #region DropDown

    private async Task GetBranch()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.GetBranch();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }
            _banchList = result.Data;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task GetProduct()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.GetProduct();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }
            _productList = result.Data;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task GetCurrency()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.GetCurrency();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }
            _currencyList = result.Data;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task GetBox()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.GetBox();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }
            _boxList = result.Data;
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

    #region Get Other Branch

    private async Task GetOtherBranch()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.GetOtherBranch();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }
            _banchList = result.Data;
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
