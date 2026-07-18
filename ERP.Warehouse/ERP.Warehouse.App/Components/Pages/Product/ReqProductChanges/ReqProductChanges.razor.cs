using ERP.Warehouse.App.Common;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Components.Pages.Product.ReqProductChanges;

public partial class ReqProductChanges
{
    private ReqProductChangesReqModel _reqModel = new();
    private IEnumerable<ReqProductChangesModel> _model = new List<ReqProductChangesModel>();
    private ReqProductChangesEditModel _edit = new();
    private ReqProductChangesDetailModel _details = new();

    private List<SelectListModel> _lstStatus = Commons.GetStatusList();

    private MudDataGrid<ReqProductChangesModel> _elementGrid = default!;
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

    private async Task Save(ReqProductChangesModel reqModel)
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
            _selectedFiles.Clear();
            _imagePreviewUrl = null;
            _existingImagePath = null;
            await List();
        }

        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Edit(ReqProductChangesModel reqModel)
    {
        try
        {
            if (reqModel is null || string.IsNullOrEmpty(reqModel.ReqProductChangesId))
            {
                _formType = EnumFormType.Create;
                return;
            }

            _edit.ReqProductChangesId = reqModel.ReqProductChangesId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Edit(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _reqModel.ReqProductChangesId = result.Data.ReqProductChangesId;
            _reqModel.ProductName = result.Data.ProductName;
            _reqModel.ProductCode = result.Data.ProductCode;
            _reqModel.SupplierName = result.Data.SupplierName;
            _existingImagePath = result.Data.ImagePath;
            _imagePreviewUrl = null;

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
            _selectedFiles.Clear();
            _imagePreviewUrl = null;
            _existingImagePath = null;
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

    private async Task Delete(ReqProductChangesModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.Confirm();
            if (!confirm)
            {
                return;
            }

            _edit.ReqProductChangesId = reqModel.ReqProductChangesId!;

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

    private async Task Details(ReqProductChangesModel reqModel)
    {
        try
        {
            _edit.ReqProductChangesId = reqModel.ReqProductChangesId!;

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

    private async Task OnImageUpload(InputFileChangeEventArgs args)
    {
        var file = args.File;
        if (file is null) return;

        _selectedFiles.Clear();
        _selectedFiles.Add(file);
        using var ms = new MemoryStream();
        await file.OpenReadStream(10 * 1024 * 1024).CopyToAsync(ms);
        var bytes = ms.ToArray();
        _reqModel.ImageData = Convert.ToBase64String(bytes);
        _imagePreviewUrl = $"data:{file.ContentType};base64,{_reqModel.ImageData}";
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
}
