using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestAssessment.Settings;

namespace TestAssessment.Extensions;

public static class ServiceCollectionVisitor
{
    public static IServiceCollection BindSettings(this IServiceCollection services,  IConfiguration config)
    {
        services.Configure<ConnectionStrings>(config.GetSection("ConnectionStrings"));
        services.Configure<CsvFilesSettings>(config.GetSection("CsvFilesSettings"));
        services.Configure<DataRepositorySettings>(config.GetSection("DataRepositorySettings"));
        
        return services;
    }
}