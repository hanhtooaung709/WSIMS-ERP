using ERP.Warehouse.Api.Common;
using ERP.Warehouse.Api.Features.Box;
using ERP.Warehouse.Api.Features.Branch;
using ERP.Warehouse.Api.Features.Currency;
using ERP.Warehouse.Api.Features.Product.ProductList;
using ERP.Warehouse.Api.Features.Product.ReqProduct;
using ERP.Warehouse.Api.Features.Product.ReqProductChanges;
using ERP.Warehouse.Api.Features.SignIn;
using ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUser;
using ERP.Warehouse.Api.Features.WarehouseUser.ReqWarehouseUserChanges;
using ERP.Warehouse.Api.Features.WarehouseUser.WarehouseUserList;
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
        builder.Services.AddScoped<AuthorizationService>();
        builder.Services.AddScoped<JwtTokenHelper>();
        builder.Services.AddScoped<SignInService>();
        builder.Services.AddScoped<WarehouseUserListService>();
        builder.Services.AddScoped<ReqWarehouseUserService>();
        builder.Services.AddScoped<ReqWarehouseUserChangesService>();
        builder.Services.AddScoped<BranchService>();
        builder.Services.AddScoped<BoxService>();
        builder.Services.AddScoped<CurrencyService>();
        builder.Services.AddScoped<ProductListService>();
        builder.Services.AddScoped<ReqProductService>();
        builder.Services.AddScoped<ReqProductChangesService>();
        return builder;
    }
}
