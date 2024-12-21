using TestAssessment.Models;

namespace TestAssessment.Data.Repositories;

public interface IDataRepository
{
    Task SaveRecordsAsync(IEnumerable<TripRecord> records, CancellationToken cancellationToken);
    Task<IEnumerable<TripRecord>> RemoveDuplicatesAsync(CancellationToken cancellationToken);
}