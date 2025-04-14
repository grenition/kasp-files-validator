using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Project.Extensions;

public static class DIExtensions
{
    public static IServiceCollection AddYamlConfiguration(this IServiceCollection services, out IConfiguration configuration)
    {
        configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddYamlFile("Configuration.yaml", optional: false, reloadOnChange: true)
            .Build();

        services.AddSingleton(configuration);

        return services;
    }
}
