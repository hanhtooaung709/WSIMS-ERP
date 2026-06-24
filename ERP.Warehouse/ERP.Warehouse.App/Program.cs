using BlazorDownloadFile;
using ERP.Warehouse.App;
using ERP.Warehouse.App.Components;
using ERP.Warehouse.App.Services.InjectionService;
using ERP.Warehouse.App.Services.Security;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Radzen;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "JwtAuth";
        options.DefaultChallengeScheme = "JwtAuth";
    })
    .AddCookie("JwtAuth", options =>
    {
        options.LoginPath = "/";
    });
    builder.Services.AddAuthorizationCore();
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
    builder.Services.AddScoped<CustomAuthStateProvider>(provider =>
        (CustomAuthStateProvider)provider.GetRequiredService<AuthenticationStateProvider>());

    builder.Services.AddControllers();
    builder.Services.AddControllersWithViews();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<IInjectService, InjectService>();

    builder.Services.AddMudServices();
    builder.Services.AddRadzenComponents();
    builder.Services.AddBlazorDownloadFile();

    // Add services to the container.
    builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddHubOptions(options =>
            {
                options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10MB
            });

    builder.AddModularService();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}