using ERP.Warehouse.App.Common;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Product.ReqProduct;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Components.Pages.Package.ReqPackage;

public partial class ReqPackage
{
    private ReqPackageReqModel _reqModel = new();
    private IEnumerable<ReqPackageModel> _model = new List<ReqPackageModel>();
    private ReqPackageEditModel _edit = new();
    private ReqPackageDetailModel _details = new();

    private List<BranchResponseModel> _banchList = new();
    private List<ProductResponseModel> _productList = new();
    private List<CurrencyResponseModel> _currencyList = new();
    private List<BoxResponseModel> _boxList = new();

    private List<SelectListModel> _lstStatus = Commons.GetStatusList();

    private MudDataGrid<ReqPackageModel> _elementGrid = default!;
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

    private async Task Save(ReqPackageModel reqModel)
    {
        try
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

    private async Task Edit(ReqPackageModel reqModel)
    {
        try
        {
            if (reqModel is null || string.IsNullOrEmpty(reqModel.ReqPackageId))
            {
                _formType = EnumFormType.Create;
                return;
            }

            await GetBranch();
            await GetProduct();
            await GetCurrency();
            await GetBox();
            _edit.ReqPackageId = reqModel.ReqPackageId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Edit(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _reqModel.ReqPackageId = result.Data.ReqPackageId;
            _reqModel.PackageName = result.Data.PackageName;
            _reqModel.PackageInfoCode = result.Data.PackageInfoCode;
            _reqModel.BranchCode = result.Data.BranchCode;
            _reqModel.Quanity = result.Data.Quanity;
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

    private async Task Delete(ReqPackageModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.Confirm();
            if (!confirm)
            {
                return;
            }

            _edit.ReqPackageId = reqModel.ReqPackageId!;

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

    private async Task Details(ReqPackageModel reqModel)
    {
        try
        {
            _edit.ReqPackageId = reqModel.ReqPackageId!;

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
}
