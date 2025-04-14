using Microsoft.Extensions.Options;
using Project.Configs;
using Project.Services.FilePathsLoader;
using Project.Services.FileStreamer;
using Project.Services.LineValidator;

namespace Project.Services.AppProcessor;

public class AppProcessor(
    IFilePathsLoader filePathsLoader,
    IFileStreamer fileStreamer,
    ILineValidator lineValidator,
    IOptions<AppConfig> appConfig,
    IOptions<AppRuntimeConfig> appRuntimeConfig)
{
    private AppConfig AppConfig => appConfig.Value;
    private AppRuntimeConfig AppRuntimeConfig => appRuntimeConfig.Value;

    public void Run()
    {
        var paths = filePathsLoader.LoadFilePaths(AppRuntimeConfig.WorkingDirectory!, AppConfig.ExtensionFilter!);

        foreach (var path in paths)
        {
            fileStreamer.OpenFile(path);

            while (fileStreamer.GetLine(out string line))
            {
                if (lineValidator.IsBadLine(line, out var correctLine))
                    fileStreamer.ModifyPreviousLine(correctLine);
            }

            fileStreamer.CloseFile();
        }
    }
}
