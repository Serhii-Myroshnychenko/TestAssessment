namespace TestAssessment.Services.EtlServices;

public interface IEtlService
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}