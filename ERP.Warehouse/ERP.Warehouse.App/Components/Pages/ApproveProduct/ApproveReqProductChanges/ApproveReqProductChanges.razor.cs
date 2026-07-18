using ERP.Warehouse.App.Common;
using ERP.Warehouse.Models.Models.Product.ReqProductChanges;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Components.Pages.ApproveProduct.ApproveReqProductChanges;

public partial class ApproveReqProductChanges
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
            await _injectService.EnableLoading();
            var result = await _apiService.GetApproveReqProduct(_reqModel);
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

    private async Task Approve(ReqProductChangesModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.ShowApprove();
            if (!confirm) return;

            _edit.ReqProductChangesId = reqModel.ReqProductChangesId!;
            _edit.ProductId = reqModel.ProductId!;

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

    private async Task Reject(ReqProductChangesModel reqModel)
    {
        try
        {
            DialogResult RejectReason = await _injectService.ShowRejectDialog("Reason for rejection");

            bool confirm = await _injectService.ShowReject();
            if (!confirm) return;

            _edit.ReqProductChangesId = reqModel.ReqProductChangesId!;
            _edit.ProductId = reqModel.ProductId;
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

    private async Task Details(ReqProductChangesModel reqModel)
    {
        try
        {
            _edit.ReqProductChangesId = reqModel.ReqProductChangesId!;

            await _injectService.EnableLoading();
            var result = await _apiService.ApproveReqProductDetails(_edit);
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
        var fileName = Path.GetFileName(imagePath);
        var baseUrl = _setting.CurrentValue.WarehouseApp.WarehouseApiBaseUrl.TrimEnd('/');
        return $"{baseUrl}/api/image/product/{fileName}";
    }

    #endregion
}
