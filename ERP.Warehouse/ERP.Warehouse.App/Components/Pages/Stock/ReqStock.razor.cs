using ERP.Warehouse.App.Common;
using ERP.Warehouse.Models.Models.Package.ReqPackage;
using ERP.Warehouse.Models.Models.Stock;
using ERP.Warehouse.Models.Models.WarehouseUser.WarehouseUserList;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Components.Pages.Stock;

public partial class ReqStock
{
    private StockReqModel _reqModel = new();
    private IEnumerable<StockModel> _model = new List<StockModel>();
    private StockEditModel _edit = new();
    private StockDetailModel _details = new();

    private List<BranchResponseModel> _banchList = new();
    private List<ProductResponseModel> _productList = new();
    private List<CurrencyResponseModel> _currencyList = new();
    private List<BoxResponseModel> _boxList = new();
    private List<OtherBranchResponseModel> _otherBanchList = new();

    private List<SelectListModel> _lstStatus = Commons.GetStatusList();

    private MudDataGrid<StockModel> _elementGrid = default!;
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
                /*await List();*/
            }
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            await _injectService.ErrorDialogMessage(ex.Message);
        }
    }

    #region Get/Edit/Update/Delete/Details



    #endregion
}
