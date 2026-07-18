using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WSIMS_ERP.Shared;

public static class ConfigurationExtension
{
    public static WebApplicationBuilder AddStageConfig(this WebApplicationBuilder builder)
    {
        var contentRootPath = builder.Environment.ContentRootPath;
        var folderPath = FindConfigFolderPath(contentRootPath);

        var appSettingFolderPath = Path.Combine(folderPath, "appsettings.json");
        builder.Configuration.AddJsonFile(appSettingFolderPath, optional: false, reloadOnChange: true);

        var config = (IConfigurationBuilder)builder.Configuration;
        var tempConfig = config.Build();
        var stage = tempConfig.GetSection("Stage")?.Value?.ToLower();

        if (string.IsNullOrEmpty(stage))
        {
            throw new Exception("The 'Stage' key is missing or empty in appsettings.json.");
        }

        var customSettingFilePath = Path.Combine(folderPath, $"custom-setting-{stage}.json");
        Console.WriteLine("Custom Setting JSON File Path: " + customSettingFilePath);

        builder.Configuration.AddJsonFile(customSettingFilePath, optional: false, reloadOnChange: true);

        builder.Services.AddOptions();
        builder.Services.Configure<CustomSettingModel>(builder.Configuration);
        builder.Services.AddSingleton<CustomSettingModel>(sp =>
            sp.GetRequiredService<IOptions<CustomSettingModel>>().Value);

        return builder;
    }

    private static string FindConfigFolderPath(string contentRootPath)
    {
        var solutionRootPath = FindSolutionRootPath(contentRootPath);
        var configFolderPath = Path.Combine(solutionRootPath, "Config");

        if (Directory.Exists(configFolderPath))
        {
            return configFolderPath;
        }

        throw new DirectoryNotFoundException("Config folder could not be found.");
    }

    private static string FindSolutionRootPath(string contentRootPath)
    {
        Console.WriteLine(contentRootPath);
        var currentDirectory = new DirectoryInfo(contentRootPath);

        while (currentDirectory is not null)
        {
            var solutionFilePath = Path.Combine(currentDirectory.FullName, "ERP.Warehouse.sln");
            if (File.Exists(solutionFilePath))
            {
                currentDirectory = currentDirectory.Parent;
                return currentDirectory.ToString();
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new DirectoryNotFoundException("Solution root could not be found.");
    }
}
