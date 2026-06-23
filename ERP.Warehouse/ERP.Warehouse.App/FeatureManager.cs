using ERP.Warehouse.App.Api;
using ERP.Warehouse.App.Services.InjectionService;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using MudBlazor.Services;
using WSIMS_ERP.Shared;
using WSIMS_ERP.Shared.HttpClients;

namespace ERP.Warehouse.App;

public static class FeatureManager
{
    public static WebApplicationBuilder AddModularService(this WebApplicationBuilder builder)
    {
        return builder
            .AddStageConfig()
            .AddDbServices()
            .AddWarehouseBackendApi();
    }

    private static WebApplicationBuilder AddDbServices(this WebApplicationBuilder builder)
    {
        #region Add Db Services

        builder.Services.AddDbContext<AppDbContext>(opt =>
        {
            var connectionString = builder.Configuration.GetSection("DbConnection").Value;
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            opt.UseSqlServer(connectionString);
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        #endregion

        return builder;
    }

    private static WebApplicationBuilder AddWarehouseBackendApi(this WebApplicationBuilder builder)
    {
        string baseUrl = builder.Configuration["WarehouseApp:WarehouseApiBaseUrl"]!;

        builder.Services.AddHttpClient<HttpClientService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromMinutes(5);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });

        builder.Services.AddTransient<WarehouseApiService>();

        return builder;
    }
}
