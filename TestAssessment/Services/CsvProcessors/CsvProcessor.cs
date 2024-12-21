using System.Globalization;
using CsvHelper;
using TestAssessment.Data.Repositories;
using TestAssessment.Helpers;
using TestAssessment.Models;

namespace TestAssessment.Services.CsvProcessors;

public class CsvProcessor(IDataRepository dataRepository) : ICsvProcessor
{
    public IEnumerable<TripRecord> ParseCsv(string csvFilePath)
    {
        using var reader = new StreamReader(csvFilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            yield return new TripRecord
            {
                PickupDatetime = ConversionUtils.ConvertToUtc(csv.GetField<DateTime>(CsvColumns.PickupDateTime), TimeZones.EasternStandard),
                DropoffDatetime = ConversionUtils.ConvertToUtc(csv.GetField<DateTime>(CsvColumns.DropOffDateTime), TimeZones.EasternStandard),
                PassengerCount = csv.TryGetField<int>(CsvColumns.PassengerCount, out var passengerCount) ? passengerCount : 0,
                TripDistance = csv.GetField<double>(CsvColumns.TripDistance),
                StoreAndFwdFlag = ConversionUtils.ConvertFlag(csv.TryGetField<string>(CsvColumns.StoreAndFwdFlag, out var flag) ? flag : "N"),
                PULocationID = csv.GetField<int>(CsvColumns.PuLocationId),
                DOLocationID = csv.GetField<int>(CsvColumns.DoLocationId),
                FareAmount = csv.GetField<decimal>(CsvColumns.FareAmount),
                TipAmount = csv.GetField<decimal>(CsvColumns.TipAmount)
            };
        }
    }
    
    public async Task DeleteDuplicatesAndWriteToCsvAsync(string outputFilePath, CancellationToken cancellationToken)
    {
        var rootPath = AppContext.BaseDirectory;
        
        var projectRoot = Path.GetFullPath(Path.Combine(rootPath, @"..\..\..\")); 
        
        var duplicates = await dataRepository.RemoveDuplicatesAsync(cancellationToken);
        
        await using var writer = new StreamWriter(Path.Combine(projectRoot, outputFilePath));
        
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        await csv.WriteRecordsAsync(duplicates, cancellationToken);
    }
}
