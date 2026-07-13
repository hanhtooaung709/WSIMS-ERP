using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ERP.Warehouse.Models;
using WSIMS_ERP.Shared.Enums;
using WSIMS_ERP.Shared.Models;
using WSIMS_ERP.Shared;
using System.Text.Json;
using ERP.Warehouse.Models.Models;
using ERP.Warehouse.App.Components.DialogBoxs;
using ERP.Warehouse.App.Components.Shared;

namespace ERP.Warehouse.App.Services.InjectionService;

public class InjectService : IInjectService
{
    private readonly ILogger<InjectService> _logger;
    private readonly IJSRuntime _jSRuntime;
    private readonly IDialogService _dialogService;
    private readonly NavigationManager _navigationManager;
    private readonly ProtectedSessionStorage _protectedSessionStorage;
    private readonly ISnackbar _snackbar;

    public InjectService(IJSRuntime jSRuntime,
        IDialogService dialogService,
        NavigationManager navigationManager,
        ProtectedSessionStorage protectedSessionStorage,
        ILogger<InjectService> logger,
        ISnackbar snackbar)
    {
        _jSRuntime = jSRuntime;
        _dialogService = dialogService;
        _navigationManager = navigationManager;
        _protectedSessionStorage = protectedSessionStorage;
        _logger = logger;
        _snackbar = snackbar;
    }

    public void Go(string? url, bool foreceReload = false)
    {
        try
        {
            _navigationManager.NavigateTo(url, foreceReload);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task EnableLoading()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("enableLoading", true);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task DisableLoading()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("enableLoading", false);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task FormSubmitAsync(string identifier)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync(identifier);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    /*public async Task<DialogResult> ShowDialog(string title, string message, EnumRespType respType = EnumRespType.Error,
        EnumMessageBoxType messageBoxType = EnumMessageBoxType.Ok, string? url = null,
        DialogOptions? options = null,
        bool reload = false)
    {
        var parameters = new DialogParameters
        {
            {
                "Setting", new MessageBoxModel
                {
                    Title = title,
                    Message = message,
                    MessageType = respType,
                    MessageBoxType = messageBoxType,
                    Url = url,
                    Reload = reload
                }
            }
        };
        options ??= new MudBlazor.DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await _dialogService.ShowAsync<MessageBoxComponent>("", parameters, options);
        return await dialog.Result;
    }*/

    public async Task<DialogResult> ShowDialog<T>(Result<T> result, string? url = null,
        bool reload = false)
    {
        try
        {
            var parameters = new DialogParameters
            {
                {
                    "Setting", new MessageBoxModel
                    {
                        Title = result.RespType.GetEnumDescription(),
                        Message = result.RespDesp,
                        MessageType = result.RespType,
                        MessageBoxType = EnumMessageBoxType.Ok,
                        Url = url,
                        Reload = reload
                    }
                }
            };
            DialogOptions options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await _dialogService.ShowAsync<MessageBoxComponent>("", parameters, options);
            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task<DialogResult> WarningDialog(string message)
    {
        try
        {
            var parameters = new DialogParameters
            {
                {
                    "Setting", new MessageBoxModel
                    {
                        Title = "Warning",
                        Message = message,
                        MessageType = EnumRespType.Warning,
                        MessageBoxType = EnumMessageBoxType.Ok,
                        Url = null,
                        Reload = false
                    }
                }
            };
            DialogOptions options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await _dialogService.ShowAsync<MessageBoxComponent>("", parameters, options);
            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task<DialogResult> ShowDialogMessage(string message)
    {
        try
        {
            var parameters = new DialogParameters
            {
                {
                    "Setting", new MessageBoxModel
                    {
                        Title = "Information",
                        Message = message,
                        MessageType = EnumRespType.Success,
                        MessageBoxType = EnumMessageBoxType.Ok,
                        Url = null,
                        Reload = false
                    }
                }
            };
            DialogOptions options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await _dialogService.ShowAsync<MessageBoxComponent>("", parameters, options);
            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task<DialogResult> ErrorDialogMessage(string message)
    {
        try
        {
            var parameters = new DialogParameters
            {
                {
                    "Setting", new MessageBoxModel
                    {
                        Title = "Error",
                        Message = message,
                        MessageType = EnumRespType.Error,
                        MessageBoxType = EnumMessageBoxType.Ok,
                        Url = null,
                        Reload = false
                    }
                }
            };
            DialogOptions options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await _dialogService.ShowAsync<MessageBoxComponent>("", parameters, options);
            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task<DialogResult> ErrorDialog<T>(Result<T> result)
    {
        try
        {
            var parameters = new DialogParameters
            {
                {
                    "Setting", new MessageBoxModel
                    {
                        Title = result.RespType.GetEnumDescription(),
                        Message = result.RespDesp,
                        MessageType = result.RespType,
                        MessageBoxType = EnumMessageBoxType.Ok,
                        Url = null,
                        Reload = false
                    }
                }
            };
            DialogOptions options = new MudBlazor.DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };
            var dialog = await _dialogService.ShowAsync<MessageBoxComponent>("", parameters, options);
            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task AllowAlphaAndSpecialCharactersOnly(string className = "clsEngNumberAndSpecialOnly")
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("allowAlphaAndSpecialcharactersOnly", className);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task AllowAlphabetNumberAndSpecialCharacter(
        string className = "clsAllowAlphabetNumberandSpecialCharacter")
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("allowAlphabetNumberandSpecialCharacter", className);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task AllowAlphaNumberAndSpace()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("allowAlphaNumberAndSpace");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            this._logger.LogCustomError(ex);
        }
    }

    public async Task RemoveMyanmarFontByClass(string className = "clsRemoveMyanmarFont")
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            await _jSRuntime.InvokeVoidAsync("removeMyanmarFontByClass", className);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task RemoveMyanmarFontNoSpace(string className = "removeMyanmarFontNoSpace")
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("removeMMFontNoSpaceByClass", className);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task RemoveMyanmarFontWithSpace(string className = "removeMyanmarFontWithSpace")
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("removeMMFontWithSpaceByClass", className);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task FormComma()
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            await _jSRuntime.InvokeVoidAsync("formcomma");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyAlphaAndSpace()
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            await _jSRuntime.InvokeVoidAsync("onlyAlphaAndSpace");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task NumberOnly(int number = 20)
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            await _jSRuntime.InvokeVoidAsync("numberonly", number);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task NumberOnlyAndDot()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("numberonlyanddot");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyNumber()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("onlyNumber");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyNoAndSpacialAndSpace()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("onlyNoAndSpecialAndSpace");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyAlphaAndNumber()
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            await _jSRuntime.InvokeVoidAsync("onlyAlphaAndNumber");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task AllowNumberAndSpecialCharacterOnly()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("allowNumberAndSpecialCharacter");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyAlpha()
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(600));
            await _jSRuntime.InvokeVoidAsync("onlyAlpha");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }


    public async Task ClearSessionStorageData(string key)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("clearSessionStorageData", key);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task BarChart(List<int> activeUser, List<string> month)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("barChart", activeUser, month);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task AreaChart(int[] walletData, int[] bankData)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("areaChart", walletData, bankData);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task AllowAlphaNumberSpaceAndSpecialCharacter()
    {
        try
        {
            // await Task.Delay(TimeSpan.FromMilliseconds(500));
            await _jSRuntime.InvokeVoidAsync("allowAlphaNumberSpaceAndSpecialCharacter");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task BindJsTreeView(string id, string selectedId, object data)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("bindJsTreeView", id, selectedId, data);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task<string> GetSessionStorageData(string id)
    {
        try
        {
            var jsonStr = await _jSRuntime.InvokeAsync<string>("getSessionStorageData", id);
            return jsonStr;
        }
        catch (JSDisconnectedException)
        {
            return string.Empty;
        }
        catch (TaskCanceledException)
        {
            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return string.Empty;
        }
    }

    public async Task PreventEnterKeyByClass()
    {
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            await _jSRuntime.InvokeVoidAsync("PreventEnterKeyByClass");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyNumberAndSpace()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("onlyNumberAndSpace");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyAlphaNumberAndDot()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("onlyAlphaNumberAndDot");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyAlphaNumberAndDash()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("onlyAlphaNumberAndDash");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyAlphaNumberAndSpace()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("allowAlphaNumberAndSpace");
        }
        catch (JSDisconnectedException)
        {
            throw;
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            throw;
        }
    }

    public async Task OnlyAlphaNumAndUnderScore()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("allowAlphaNumberAndUnderScore");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }
    public async Task TwoLetterAndFourDigit()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("allowTwoLetterAndFourDigit");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyNumberDash()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("onlyNumberDash");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task OnlyNumberDot()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("onlyNumberDot");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task ResetReport(string id = "iframe_report")
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("iframeInterop.clearIframeContent", id);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task EmailFormat(string className = "emailFormat")
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("emailFormat", className);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task PushNotification(string title, string content)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("jsFunctions.pushNotification", title, content);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task SetConnectionId(string connectionId)
    {
        try
        {
            await _protectedSessionStorage.SetAsync("ConnectionId", connectionId);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task ReomveConnectionId()
    {
        try
        {
            await _protectedSessionStorage.DeleteAsync("ConnectionId");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task DeleteSession(string key)
    {
        try
        {
            await _protectedSessionStorage.DeleteAsync(key);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task DeleteSession(string key, string keyName)
    {
        try
        {
            await _protectedSessionStorage.DeleteAsync(key + keyName);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    // public async Task ExportReport(string url)
    // {
    //     await _jSRuntime.InvokeVoidAsync("exportReport", url);
    // }


    public async Task DownloadFile(string fileName, DotNetStreamReference dotNetStreamReference)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, dotNetStreamReference);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }


    public async Task NrcFormat(string className = "nrcFormat")
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("nrcFormat", className);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    //public async Task ShowMerchantQR(string reportLink, string merchantCode, MerchantQrViewModel model)
    //{
    //    var parameters = new DialogParameters();
    //    parameters.Add("ReportLink", reportLink);
    //    parameters.Add("MerchantCode", merchantCode);
    //    parameters.Add("ReqModel", model);
    //    IDialogReference dialog = _dialogService.Show<QrComponent>("QR Detail", parameters);
    //}

    public async Task WriteCookieAsync(string name, string value, int days)
    {
        try
        {
            await _jSRuntime.InvokeAsync<object>("setCookie", name, value, days);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task RemoveCookieAsync(string name)
    {
        try
        {
            await _jSRuntime.InvokeAsync<object>("removeCookie", name);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }


    public async Task<DialogResult> ShowDialog<T>(
        string title,
        params DialogParameterModel[] parameters) where T : IComponent
    {
        try
        {
            IDialogReference dialog = null!;
            if (parameters is null || parameters.Length == 0)
            {
                dialog = await _dialogService.ShowAsync<T>(title);
            }
            else
            {
                var parameter = new DialogParameters();
                foreach (var item in parameters)
                {
                    parameter.Add(item.Name, item.Value);
                }

                dialog = await _dialogService.ShowAsync<T>(title, parameter);
            }

            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task<bool> LockConfirm(string title, string message)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
    }

    public async Task ExportReport(string url, string fileName, string exportType)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task RenderReport(string url, string fileName, string exportType = "pdf")
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    /*public async Task<DialogResult> ShowAddDispute(string title, params DialogParameterModel[] parameters)
    {
        try
        {
            IDialogReference dialog = null!;
            if (parameters is null || parameters.Length == 0)
            {
                dialog = await _dialogService.ShowAsync<DisputeComponent>(title);
            }
            else
            {
                var parameter = new DialogParameters();
                foreach (var item in parameters)
                {
                    parameter.Add(item.Name, item.Value);
                }
                dialog = await _dialogService.ShowAsync<DisputeComponent>(title, parameter);
            }

            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }*/

    public async Task<DialogResult> ShowAgentCommission(string title, params DialogParameterModel[] parameters)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task LoadJs()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("addScript", new string[]
            {
                "theme/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js",
                "theme/assets/js/main.js",
                "theme/assets/vendor/js/menu.js",
                "theme/assets/vendor/js/bootstrap.js",
                "theme/assets/vendor/js/dropdown-hover.js",
                "js/menuToggle.js",
            }.ToList());
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task MenuToggle()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("initMenuToggle", true);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task ToggleSidebar()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("toggleMenu", true);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task HoverMenu()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("initAsideHover", true);
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task InitFromStorage()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("layoutManager.initFromStorage");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task SetupHover()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("layoutManager.setupHover");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task ToggleState()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("layoutHelper.initToggleState");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task PasswordToggle()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("passwordToggle.init");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task<bool> Confirm(string title = "Confirm", string message = "Are you sure want to delete?",
        List<DynamicReportModel>? lstData = null)
    {
        try
        {
            //var parameters = new DialogParameters { ["Title"] = title, ["message"] = message };
            var parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            if (lstData != null && lstData.Count > 0)
            {
                parameters.Add("lstDynamicReport", lstData);
            }

            var dialog = _dialogService.Show<DialogComponent>("", parameters);
            var result = await dialog.Result;

            return !result.Canceled;
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
    }

    public string GetBadgeClass(EnumRequestedStatus status)
    {
        try
        {
            return status switch
            {
                EnumRequestedStatus.Approved => "badge rounded-pill bg-label-success me-1",
                EnumRequestedStatus.Pending => "badge rounded-pill bg-label-warning me-1",
                EnumRequestedStatus.Rejected => "badge rounded-pill bg-label-danger me-1",
                EnumRequestedStatus.Active => "badge rounded-pill bg-label-success me-1",
                EnumRequestedStatus.InActive => "badge rounded-pill bg-label-danger me-1",
                _ => "badge rounded-pill bg-label-secondary me-1"
            };
        }
        catch (JSDisconnectedException)
        {
            return string.Empty;
        }
        catch (TaskCanceledException)
        {
            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return string.Empty;
        }
    }

    public async Task<DialogResult> ShowRejectDialog(string title)
    {
        try
        {
            var parameters = new DialogParameters();
            parameters.Add("RejectModel", new RejectReasonModel { Title = title });
            IDialogReference dialog = _dialogService.Show<RejectReasonComponent>("", parameters);
            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public async Task SetSessionStorage(string key, object value)
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("sessionStorage.setItem", key, value?.ToString());
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task<T> GetSessionStorage<T>(string key)
    {
        try
        {
            var result = await _protectedSessionStorage.GetAsync<T>(key);
            return result.Value!;
        }
        catch (JSDisconnectedException)
        {
            return default(T)!;
        }
        catch (TaskCanceledException)
        {
            return default(T)!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default(T)!;
        }
    }

    public async Task<bool> ShowApprove(string title = "Confirm", string message = "Are you sure want to approve?",
        List<DynamicReportModel>? lstData = null)
    {
        try
        {
            //var parameters = new DialogParameters { ["Title"] = title, ["message"] = message };
            var parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            if (lstData != null && lstData.Count > 0)
            {
                parameters.Add("lstDynamicReport", lstData);
            }

            var dialog = _dialogService.Show<DialogComponent>("", parameters);
            var result = await dialog.Result;

            return !result.Canceled;
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
    }

    public async Task<bool> ShowReject(string title = "Confirm", string message = "Are you sure want to Reject?",
        List<DynamicReportModel>? lstData = null)
    {
        try
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            if (lstData != null && lstData.Count > 0)
            {
                parameters.Add("lstDynamicReport", lstData);
            }

            var dialog = _dialogService.Show<DialogComponent>("", parameters);
            var result = await dialog.Result;

            return !result.Canceled;
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
    }

    public async Task<bool> ShowCreateDialog(string title = "Confirm", string message = "Are you sure want to save?",
        List<DynamicReportModel>? lstData = null)
    {
        try
        {
            //var parameters = new DialogParameters { ["Title"] = title, ["message"] = message };
            var parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            if (lstData != null && lstData.Count > 0)
            {
                parameters.Add("lstDynamicReport", lstData);
            }

            var dialog = _dialogService.Show<DialogComponent>("", parameters);
            var result = await dialog.Result;

            return !result.Canceled;
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
    }

    public async Task<bool> ShowUpdateDialog(string title = "Confirm", string message = "Are you sure want to update?",
        List<DynamicReportModel>? lstData = null)
    {
        try
        {
            //var parameters = new DialogParameters { ["Title"] = title, ["message"] = message };
            var parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            if (lstData != null && lstData.Count > 0)
            {
                parameters.Add("lstDynamicReport", lstData);
            }

            var dialog = _dialogService.Show<DialogComponent>("", parameters);
            var result = await dialog.Result;

            return !result.Canceled;
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
    }

    public async Task ClearSession()
    {
        try
        {
            await _jSRuntime.InvokeVoidAsync("clearSession");
        }
        catch (JSDisconnectedException)
        {
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
        }
    }

    public async Task<bool> EodRunConfirm(string title = "EOD Manual Run Confirm",
       string message = "Are you sure want to run?")
    {
        try
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            var dialog = _dialogService.Show<DialogComponent>("", parameters);
            var result = await dialog.Result;
            return !result.Canceled;
        }
        catch (JSDisconnectedException)
        {
            return false;
        }
        catch (TaskCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return false;
        }
    }


    public async Task<DialogResult> RequiredValidationDialog(List<string> errors)
    {
        try
        {
            IDialogReference dialog = null!;
            var parameter = new DialogParameters();
            parameter.Add("Errors", errors);
            dialog = await _dialogService.ShowAsync<RequiredValidationComponent>("", parameter);
            return await dialog.Result;
        }
        catch (JSDisconnectedException)
        {
            return default!;
        }
        catch (TaskCanceledException)
        {
            return default!;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return default!;
        }
    }

    public string GetStatusClass(bool status)
    {
        try
        {
            return status switch
            {
                true => "bg-label-success",
                false => "bg-label-secondary",
            };
        }
        catch (JSDisconnectedException)
        {
            return string.Empty;
        }
        catch (TaskCanceledException)
        {
            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogCustomError(ex);
            return string.Empty;
        }
    }
}
