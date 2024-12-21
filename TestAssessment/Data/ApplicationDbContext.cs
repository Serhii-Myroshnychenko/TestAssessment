using Microsoft.EntityFrameworkCore;
using TestAssessment.Data.Configurations;
using TestAssessment.Models;

namespace TestAssessment.Data;
public class ApplicationDbContext : DbContext
{
    public DbSet<TripRecord> TripRecords { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TripRecordConfiguration());
    }
}