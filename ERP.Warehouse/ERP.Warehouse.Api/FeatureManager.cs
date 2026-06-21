using ERP.Warehouse.Api.Features.SignIn;
using Microsoft.EntityFrameworkCore;
using Module.CommonDbService.EfAppDbContextModels;
using WSIMS_ERP.Shared;

namespace ERP.Warehouse.Api;

public static class FeatureManager
{
    public static WebApplicationBuilder AddModularService(this WebApplicationBuilder builder)
    {
        return builder
            .AddStageConfig()
            .AddDbServices()
            .AddWarehouseServices();
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

    private static WebApplicationBuilder AddWarehouseServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<SignInService>();
        return builder;
    }
}
