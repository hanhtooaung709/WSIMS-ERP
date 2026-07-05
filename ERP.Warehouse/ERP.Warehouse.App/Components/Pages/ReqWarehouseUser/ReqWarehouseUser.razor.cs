using ERP.Warehouse.Models.Models.ReqWarehouseUser;
using ERP.Warehouse.Models.Models.WarehouseUserList;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;

namespace ERP.Warehouse.App.Components.Pages.ReqWarehouseUser;

public partial class ReqWarehouseUser
{
    private ReqWarehouseUserReqModel _reqModel = new();
    private IEnumerable<ReqWarehouseUserModel> _model = new List<ReqWarehouseUserModel>();
    private ReqWarehouseUserEditModel _edit = new();
    private ReqWarehouseUserDetailsModel _details = new();

    private List<RoleResponseModel> _roleList = new();
    private List<BranchResponseModel> _banchList = new();

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
                List();
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

    }

    private async Task Edit(ReqWarehouseUserModel reqModel)
    {

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
            _logger.LogCustomError(ex);
        }
    }

    private async Task Delete(ReqWarehouseUserModel reqModel)
    {

    }

    private async Task Details(ReqWarehouseUserModel reqModel)
    {

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
