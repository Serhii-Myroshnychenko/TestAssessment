using TestAssessment.Models;

namespace TestAssessment.Services.CsvProcessors;

public interface ICsvProcessor
{
    IEnumerable<TripRecord> ParseCsv(string csvFilePath);
    Task DeleteDuplicatesAndWriteToCsvAsync(string outputFilePath, CancellationToken cancellationToken);
}