using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestAssessment.Models;

namespace TestAssessment.Data.Configurations;

public class TripRecordConfiguration : IEntityTypeConfiguration<TripRecord>
{
    public void Configure(EntityTypeBuilder<TripRecord> builder)
    {
        builder.ToTable("TripRecord");
        builder.HasKey(t => t.Id);
    }
}