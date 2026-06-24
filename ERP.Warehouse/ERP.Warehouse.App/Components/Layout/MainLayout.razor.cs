using ERP.Warehouse.App.Services.Security;
using ERP.Warehouse.Models.Models.Signin;
using ERP.Warehouse.Models.Models.Signin.Signin;
using Microsoft.JSInterop;

namespace ERP.Warehouse.App.Components.Layout;

public partial class MainLayout
{
    bool _drawerOpen = true;
    WarehouseUserInfoListModel _listModel = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var result = await _apiService.GetUserData();

            if (result.IsError)
            {
                await _injectService.ShowDialog(result);
                return;
            }

            _listModel = result.Data;

            StateHasChanged();
        }
    }

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task HandleLogout()
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "AccessToken");
            await JSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "RefreshToken");
            await JSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "SessionId");
            await JSRuntime.InvokeVoidAsync("sessionStorage.removeItem", "UserId");

            if (_authStateProvider is CustomAuthStateProvider customProvider)
            {
                customProvider.NotifyUserLogout();
            }

            _nav.NavigateTo("/", forceLoad: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logout Error: {ex.Message}");
        }
    }
}
