using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using ERP.Warehouse.App.Common;
using WSIMS_ERP.Shared.Models;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using ERP.Warehouse.Models.Models.WarehouseUser.ReqWarehouseUser;

namespace ERP.Warehouse.App.Components.Pages.WarehouseUser.ReqWarehouseUser;

public partial class ReqWarehouseUser
{
    private ReqWarehouseUserReqModel _reqModel = new();
    private IEnumerable<ReqWarehouseUserModel> _model = new List<ReqWarehouseUserModel>();
    private ReqWarehouseUserEditModel _edit = new();
    private ReqWarehouseUserDetailsModel _details = new();

    private List<RoleResponseModel> _roleList = new();
    private List<BranchResponseModel> _banchList = new();
    private List<SelectListModel> _lstStatus = Commons.GetStatusList();

    private MudDataGrid<ReqWarehouseUserModel> _elementGrid = default!;
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
            await GetRole();
            await GetBranch();
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

    private async Task Save(ReqWarehouseUserModel reqModel)
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
        }
        catch(Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Edit(ReqWarehouseUserModel reqModel)
    {
        try
        {
            if (reqModel is null || string.IsNullOrEmpty(reqModel.ReqWarehouseUserId))
            {
                _formType = EnumFormType.Create;
                return;
            }

            await GetRole();
            await GetBranch();

            _edit.UserId = reqModel.ReqWarehouseUserId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Edit(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _reqModel.UserId = result.Data.ReqWarehouseUserId;
            _reqModel.UserName = result.Data.UserName;
            _reqModel.FullName = result.Data.FullName;
            _reqModel.StaffId = result.Data.StaffId;
            _reqModel.Phone = result.Data.Phone;
            _reqModel.Email = result.Data.Email;
            _reqModel.RoleCode = result.Data.RoleCode;
            _reqModel.BranchCode = result.Data.BranchCode;

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

    private async Task Delete(ReqWarehouseUserModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.Confirm();
            if (!confirm)
            {
                return;
            }

            _edit.UserId = reqModel.ReqWarehouseUserId!;

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

    private async Task Details(ReqWarehouseUserModel reqModel)
    {
        try
        {
            _edit.UserId = reqModel.ReqWarehouseUserId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Details(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                _reqModel = new();
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

    #endregion

    #region DropDown

    private async Task GetRole()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.GetRole();
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }
            _roleList = result.Data;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

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

    #endregion
}
