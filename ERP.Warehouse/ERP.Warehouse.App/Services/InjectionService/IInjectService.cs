using ERP.Warehouse.Models;
using MudBlazor;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;

namespace ERP.Warehouse.App.Services.InjectionService;

public interface IInjectService
{
    Task<DialogResult> WarningDialog(string message);
    Task<DialogResult> ErrorDialogMessage(string message);
    Task<DialogResult> ErrorDialog<T>(Result<T> result);
    Task<DialogResult> RequiredValidationDialog(List<string> errors);
    Task<DialogResult> ShowDialog<T>(Result<T> result, string? url = null,
        bool reload = false);
    Task<DialogResult> ShowDialogMessage(string message);
    void Go(string? url, bool foreceReload = false);
    Task EnableLoading();
    Task DisableLoading();
    Task LoadJs();
    Task MenuToggle();
    Task ToggleSidebar();
    Task HoverMenu();
    Task InitFromStorage();
    Task SetupHover();

    Task ToggleState();

    Task PasswordToggle();

    Task AllowAlphaAndSpecialCharactersOnly(string className = "clsEngNumberAndSpecialOnly");
    Task AllowAlphabetNumberAndSpecialCharacter(string className = "clsAllowAlphabetNumberandSpecialCharacter");
    Task AllowAlphaNumberAndSpace();
    Task RemoveMyanmarFontWithSpace(string className = "removeMyanmarFontWithSpace");
    Task RemoveMyanmarFontByClass(string className = "clsRemoveMyanmarFont");
    Task OnlyAlphaAndSpace();
    Task TwoLetterAndFourDigit();
    Task FormSubmitAsync(string identifier);
    Task NumberOnly(int number = 20);
    Task OnlyAlphaNumberAndDot();
    Task OnlyAlphaNumberAndDash();
    Task NumberOnlyAndDot();
    Task OnlyNumber();
    Task OnlyAlphaAndNumber();
    Task OnlyNoAndSpacialAndSpace();
    Task OnlyAlpha();
    Task FormComma();
    Task<bool> LockConfirm(string title, string message);
    //Task<DialogResult> ShowRejectDialog(string title);
    //Task<List<MenuPermissionModel>> GetMenuPermission(EnumMenuPermissionType menuPermissionType, string userId);
    Task ClearSessionStorageData(string key);
    Task AllowAlphaNumberSpaceAndSpecialCharacter();
    Task AllowNumberAndSpecialCharacterOnly();
    Task BindJsTreeView(string id, string selectedId, object data);
    Task RemoveMyanmarFontNoSpace(string className = "removeMyanmarFontNoSpace");
    Task<string> GetSessionStorageData(string id);
    Task PreventEnterKeyByClass();
    Task OnlyNumberAndSpace();
    Task OnlyAlphaNumberAndSpace();

    Task OnlyAlphaNumAndUnderScore();
    Task OnlyNumberDash();
    Task OnlyNumberDot();
    Task AreaChart(int[] walletData, int[] bankData);
    Task BarChart(List<int> activeUser, List<string> month);
    //Task HighPieChart(List<UserAccountPercentageChartModel> _userAccount);
    Task ResetReport(string id = "iframe_report");
    Task EmailFormat(string className = "emailFormat");
    Task PushNotification(string title, string content);
    Task SetConnectionId(string connectionId);
    Task ReomveConnectionId();
    Task DeleteSession(string key);
    Task DeleteSession(string key, string keyName);
    Task ExportReport(string url, string fileName, string exportType);
    Task RenderReport(string url, string fileName, string exportType = "pdf");

    Task SetSessionStorage(string key, object value);
    Task<T> GetSessionStorage<T>(string key);
    Task NrcFormat(string className = "nrcFormat");
    Task WriteCookieAsync(string name, string value, int days);
    Task RemoveCookieAsync(string name);

    Task<DialogResult> ShowAgentCommission(string title,
        params DialogParameterModel[] parameters);

    Task<bool> Confirm(string title = "Confirm", string message = "Are you sure want to delete?",
        List<DynamicReportModel>? lstData = null);
    string GetBadgeClass(EnumRequestedStatus status);
    Task<DialogResult> ShowRejectDialog(string title);
    Task<bool> ShowApprove(string title = "Confirm", string message = "Are you sure want to Approve?",
        List<DynamicReportModel>? lstData = null);
    Task<bool> ShowReject(string title = "Confirm", string message = "Are you sure want to Reject?",
        List<DynamicReportModel>? lstData = null);
    Task<bool> ShowCreateDialog(string title = "Confirm", string message = "Are you sure want to Create?",
        List<DynamicReportModel>? lstData = null);
    Task<bool> ShowUpdateDialog(string title = "Confirm", string message = "Are you sure want to Update?",
        List<DynamicReportModel>? lstData = null);

    Task<bool> ShowTransferDialog(string title = "Confirm", string message = "Are you sure want to Transfer?",
        List<DynamicReportModel>? lstData = null);
    Task ClearSession();
    Task<bool> EodRunConfirm(string title = "Confirm", string message = "Are you sure want to run?");
}
