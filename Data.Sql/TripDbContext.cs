using System.Data.SqlClient;
using Data.Sql.Models;
using Data.Sql.Models.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.Sql;

public class TripDbContext: DbContext
{
    public DbSet<TripRecord> TripRecords { get; set; }
    
    private DbConfig _dbConfig;

    public TripDbContext(IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig.Value;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_dbConfig != null)
        {
            var connectionString = new SqlConnectionStringBuilder()
            {
                InitialCatalog = _dbConfig.InitialCatalog,
                DataSource = _dbConfig.DataSource,
                IntegratedSecurity = _dbConfig.IntegratedSecurity,
                TrustServerCertificate = true
            };
            
            optionsBuilder.UseSqlServer(connectionString.ConnectionString);
        }
        base.OnConfiguring(optionsBuilder);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripRecord>(entity =>
        {
            entity.ToTable("TripRecord");
            entity.Property(e => e.PickupDatetime).HasColumnName("tpep_pickup_datetime");
            entity.Property(e => e.DropoffDatetime).HasColumnName("tpep_dropoff_datetime");
            entity.Property(e => e.PassengerCount).HasColumnName("passenger_count");
            entity.Property(e => e.TripDistance).HasColumnName("trip_distance");
            entity.Property(e => e.StoreAndFwdFlag).HasColumnName("store_and_fwd_flag");
            entity.Property(e => e.PULocationID).HasColumnName("PULocationID");
            entity.Property(e => e.DOLocationID).HasColumnName("DOLocationID");
            entity.Property(e => e.FareAmount).HasColumnName("fare_amount");
            entity.Property(e => e.TipAmount).HasColumnName("tip_amount");
        });
    }
}