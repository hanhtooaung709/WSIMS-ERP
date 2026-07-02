using ERP.Warehouse.Models.Models.WarehouseUserList;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;

namespace ERP.Warehouse.App.Components.Pages.WarehouseUser.WarehouseUserList;

public partial class WarehouseUserList
{
    private WarehouseUserReqModel _reqModel = new();
    private IEnumerable<WarehouseUserModel> _model = new List<WarehouseUserModel>();
    private WarehouseUserEditModel _edit = new();

    private List<RoleResponseModel> _roleList = new();
    private List<BranchResponseModel> _banchList = new();

    private MudDataGrid<WarehouseUserModel> _elementGrid = default!;
    private EnumFormType _formType = EnumFormType.List;
    private bool hover = true;
    private bool _readOnly;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                List();
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    async Task List()
    {
        try
        {
            await _injectService.EnableLoading();
            var result = await _apiService.Get(_reqModel);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _model = result.Data!.list!;
            StateHasChanged();
        }
        catch(Exception ex)
        {
            await _injectService.DisableLoading();
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    private async Task Create()
    {
        var modle = new WarehouseUserModel();
        _roleList = new List<RoleResponseModel>();
        await GetRole();
        await GetBranch();
        await _elementGrid.SetEditingItemAsync(modle);
    }

    private async Task Save(WarehouseUserModel reqModel)
    {
        #region Create

        if (_reqModel.UserId.IsNullOrEmpty())
        {
            bool confirm = await _injectService.ShowCreateDialog();
            if (!confirm) return;

            await _injectService.EnableLoading();
            var result = await _apiService.Create(_reqModel);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
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
                return;
            }
            await _injectService.ShowDialog(result);
        }

        #endregion
    }

    private async Task Edit(WarehouseUserModel reqModel)
    {
        try
        {
            if (reqModel is null || string.IsNullOrEmpty(reqModel.WarehouseUserId))
            {
                _formType = EnumFormType.Create;
                return;
            }

            await GetRole();
            await GetBranch();

            _edit.UserId = reqModel.WarehouseUserId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Edit(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _reqModel.UserId = result.Data.WarehouseUserId;
            _reqModel.UserName = result.Data.UserName;
            _reqModel.FullName = result.Data.FullName;
            _reqModel.StaffId = result.Data.StaffId;
            _reqModel.PhoneNo = result.Data.PhoneNo;
            _reqModel.Email = result.Data.Email;
            _reqModel.RoleCode = result.Data.RoleCode;
            _reqModel.BranchCode = result.Data.BranchCode;

            _formType = EnumFormType.Edit;
        }
        catch(Exception ex )
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
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    private async Task Delete(WarehouseUserModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.Confirm();
            if (!confirm)
            {
                return;
            }

            _edit.UserId = reqModel.WarehouseUserId!;

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
        catch( Exception ex )
        {
            _logger.LogCustomError(ex);
        }
    }

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
