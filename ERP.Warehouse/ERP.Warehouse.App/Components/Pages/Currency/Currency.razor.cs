using ERP.Warehouse.Models.Models.Currency;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;

namespace ERP.Warehouse.App.Components.Pages.Currency;

public partial class Currency
{
    private CurrencyReqModel _reqModel = new();
    private IEnumerable<CurrencyModel> _model = new List<CurrencyModel>();
    private CurrencyEditModel _edit = new();
    private CurrencyDetailModel _details = new();

    private MudDataGrid<CurrencyModel> _elementGrid = default!;
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
        var modle = new CurrencyModel();
        await _elementGrid.SetEditingItemAsync(modle);
        _formType = EnumFormType.Create;
    }

    private async Task Save(CurrencyModel reqModel)
    {
        try
        {
            #region Create

            if (_reqModel.CurrencyId.IsNullOrEmpty())
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

    private async Task Edit(CurrencyModel reqModel)
    {
        try
        {
            if (reqModel is null || string.IsNullOrEmpty(reqModel.CurrencyId))
            {
                _formType = EnumFormType.Create;
                return;
            }

            _edit.CurrencyId = reqModel.CurrencyId!;

            await _injectService.EnableLoading();
            var result = await _apiService.Edit(_edit);
            await _injectService.DisableLoading();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _reqModel.CurrencyId = result.Data.CurrencyId;
            _reqModel.CurrencyCode = result.Data.CurrencyCode;
            _reqModel.CurrencyDes = result.Data.CurrencyDes;

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

    private async Task Delete(CurrencyModel reqModel)
    {
        try
        {
            bool confirm = await _injectService.Confirm();
            if (!confirm)
            {
                return;
            }

            _edit.CurrencyId = reqModel.CurrencyId!;

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

    private async Task Details(CurrencyModel reqModel)
    {
        try
        {
            _edit.CurrencyId = reqModel.CurrencyId!;

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
