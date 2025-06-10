using Microsoft.EntityFrameworkCore;
using Test.DataAccess.Entities;

namespace Test.DataAccess;

public class TestDbContext : DbContext
{
    public DbSet<Meteorite> Meteorites { get; set; }
    public DbSet<MeteoriteClass> MeteoriteClasses { get; set; }
    public DbSet<DiscoveryYear> DiscoveryYears { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // primary keys
        modelBuilder.Entity<Meteorite>().HasKey(x => x.Id);
        modelBuilder.Entity<MeteoriteClass>().HasKey(x => x.Id);
        modelBuilder.Entity<MeteoriteClass>().HasKey(x => x.Id);

        // foreign keys
        modelBuilder.Entity<Meteorite>()
            .HasOne(x => x.Class)
            .WithOne(y => y.Meteorite)
            .HasForeignKey<Meteorite>(x => x.ClassId);

        modelBuilder.Entity<Meteorite>()
            .HasOne(x => x.Year)
            .WithOne(y => y.Meteorite)
            .HasForeignKey<Meteorite>(x => x.YearId);

        // types
        modelBuilder.Entity<Meteorite>().Property(x => x.Name).HasColumnType("varchar(100)");
        modelBuilder.Entity<MeteoriteClass>().Property(x => x.Name).HasColumnType("varchar(20)");

        // index
        modelBuilder.Entity<Meteorite>().HasIndex(x => x.Name);
        modelBuilder.Entity<MeteoriteClass>().HasIndex(x => x.Name);
        modelBuilder.Entity<DiscoveryYear>().HasIndex(x => x.Year);
    }
}
