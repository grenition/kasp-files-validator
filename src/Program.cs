using Microsoft.Extensions.DependencyInjection;
using Project.Configs;
using Project.Extensions;
using Project.Services.AppProcessor;
using Project.Services.FilePathsLoader;

try
{
    var services = new ServiceCollection();

    services.AddYamlConfiguration(out var configuration);
    services.Configure<AppConfig>(configuration.GetSection("AppConfig"));
    
    var workingDirectory = args.FirstOrDefault();
    services.Configure<AppRuntimeConfig>(options =>
    {
        options.WorkingDirectory = workingDirectory;
    });
    
    services.AddTransient<IFilePathsLoader, FilePathsLoader>();
    services.AddSingleton<AppProcessor>();
    
    var serviceProvider = services.BuildServiceProvider();
    var appProcessor = serviceProvider.GetRequiredService<AppProcessor>();

    appProcessor.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during programm execution: {ex.Message}");
}
