using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestAssessment.Data;
using TestAssessment.Data.Repositories;
using TestAssessment.Extensions;
using TestAssessment.Services.CsvProcessors;
using TestAssessment.Services.EtlServices;

var serviceProvider = ConfigureServices();

var etlService = serviceProvider.GetService<IEtlService>();

if (etlService is not null)
{
    await etlService.ExecuteAsync();
}

static ServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    // Load configuration
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    services.AddSingleton(configuration);
    services.AddLogging(builder => builder.AddConsole());
    services.BindSettings(configuration);

    // Register services
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ICsvProcessor, CsvProcessor>();
    services.AddScoped<IDataRepository, DataRepository>();
    services.AddScoped<IEtlService, EtlService>();

    return services.BuildServiceProvider();
}