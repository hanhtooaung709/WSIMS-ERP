using ERP.Warehouse.App.Common;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Package.ReqPackageChange;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Components.Pages.ApprovePackage.ApproveReqPackageChange;

public partial class ApproveReqPackageChange
{
    private ReqPackageChangeReqModel _reqModel = new();
    private IEnumerable<ReqPackageChangeModel> _model = new List<ReqPackageChangeModel>();
    private ReqPackageChangeEditModel _edit = new();
    private ReqPackageChangeDetailModel _details = new();

    private List<ProductResponseModel> _productList = new();
    private List<CurrencyResponseModel> _currencyList = new();
    private List<BoxResponseModel> _boxList = new();

    private List<SelectListModel> _lstStatus = Commons.GetStatusList();

    private MudDataGrid<ReqPackageChangeModel> _elementGrid = default!;
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

    #region Get/Approve/Reject/Details

    private async Task List()
    {
        try
        {
            await GetProduct();
            await GetBox();

            await _injectService.EnableLoading();
            var result = await _apiService.GetApproveReqPackage(_reqModel);
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

    private async Task Approve(ReqPackageChangeModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.ShowApprove();
            if (!confirm) return;

            _edit.ReqPackageChangeId = reqModel.ReqPackageChangeId!;
            _edit.PackageId = reqModel.PackageId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Approve(_edit);
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

    private async Task Reject(ReqPackageChangeModel reqModel)
    {
        try
        {
            DialogResult RejectReason = await _injectService.ShowRejectDialog("Reason for rejection");

            bool confirm = await _injectService.ShowReject();
            if (!confirm) return;

            _edit.ReqPackageChangeId = reqModel.ReqPackageChangeId!;
            _edit.PackageId = reqModel.PackageId;
            _edit.RejectReason = RejectReason.Data!.ToString();

            await _injectService.EnableLoading();
            var result = await _apiService.Reject(_edit);
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

    private async Task Details(ReqPackageChangeModel reqModel)
    {
        try
        {
            _edit.ReqPackageChangeId = reqModel.ReqPackageChangeId!;

            await _injectService.EnableLoading();
            var result = await _apiService.ApproveReqPackageDetails(_edit);
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

    #endregion

    #region DropDown

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
