using Microsoft.Extensions.Options;
using Project.Configs;
using Project.Services.FilePathsLoader;

namespace Project.Services.AppProcessor;

public class AppProcessor(
    IFilePathsLoader filePathsLoader,
    IOptions<AppConfig> appConfig,
    IOptions<AppRuntimeConfig> appRuntimeConfig)
{
    private AppConfig AppConfig => appConfig.Value;
    private AppRuntimeConfig AppRuntimeConfig => appRuntimeConfig.Value;

    public void Run()
    {
        var paths = filePathsLoader.LoadFilePaths(
            AppRuntimeConfig.WorkingDirectory!, AppConfig.ExtensionFilter!);

        foreach (var path in paths)
        {
            Console.WriteLine(path);
        }
    }
}
