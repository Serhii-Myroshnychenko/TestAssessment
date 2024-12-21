using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestAssessment.Data.Repositories;
using TestAssessment.Services.CsvProcessors;
using TestAssessment.Settings;

namespace TestAssessment.Services.EtlServices;

public class EtlService(ICsvProcessor csvProcessor, IDataRepository dataRepository,
    ILogger<EtlService> logger, IOptions<CsvFilesSettings> csvOptions) : IEtlService
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting ETL process...");

        var records = csvProcessor.ParseCsv(csvOptions.Value.CsvFileToReadPath);
        
        logger.LogInformation("Parsed CSV file.");

        await dataRepository.SaveRecordsAsync(records, cancellationToken);
        
        logger.LogInformation("Saved all the data to DB.");

        await csvProcessor.DeleteDuplicatesAndWriteToCsvAsync(csvOptions.Value.DuplicatesOutputFilePath, cancellationToken);
        
        logger.LogInformation("Deleted duplicates and saved to the file.");

        logger.LogInformation("ETL process completed successfully.");
    }
}