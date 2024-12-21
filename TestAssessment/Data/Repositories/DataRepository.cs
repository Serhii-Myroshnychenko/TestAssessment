using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestAssessment.Models;
using TestAssessment.Settings;

namespace TestAssessment.Data.Repositories;

public class DataRepository(ApplicationDbContext context, IOptions<DataRepositorySettings> options) : IDataRepository
{
    public async Task SaveRecordsAsync(IEnumerable<TripRecord> records, CancellationToken cancellationToken)
    {
        var batchSize = options.Value.BatchSize;
        
        var recordList = records.ToList();

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        for (var i = 0; i < recordList.Count; i += batchSize)
        {
            var batch = recordList.Skip(i).Take(batchSize);
            
            await context.TripRecords.AddRangeAsync(batch, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<IEnumerable<TripRecord>> RemoveDuplicatesAsync(CancellationToken cancellationToken)
    {
        // I know this is a bad approach, but i had to use due to lack of time 
        
        var duplicates = await context.TripRecords
            .FromSqlInterpolated($@"WITH CTE AS ( SELECT *, ROW_NUMBER() OVER (
                       PARTITION BY PickupDatetime, DropoffDatetime, PassengerCount
                       ORDER BY Id
                   ) AS RowNum FROM TripRecord) SELECT * FROM CTE WHERE RowNum > 1 ORDER BY PickupDatetime, DropoffDatetime, PassengerCount")
            .ToListAsync(cancellationToken);

        context.TripRecords.RemoveRange(duplicates);

        await context.SaveChangesAsync(cancellationToken);

        return duplicates;
    }
}