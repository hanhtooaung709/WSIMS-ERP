using ERP.Warehouse.App.Services.Security;
using Microsoft.JSInterop;

namespace ERP.Warehouse.App.Components.Layout;

public partial class MainLayout
{
    bool _drawerOpen = true;



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
