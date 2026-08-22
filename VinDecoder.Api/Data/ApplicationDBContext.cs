using Microsoft.EntityFrameworkCore;
using VinDecoder.Api.Models;

namespace VinDecoder.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Manufacturer> Manufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Manufacturer>()
            .Property(m => m.Wmi)
            .HasMaxLength(3);

        modelBuilder.Entity<Manufacturer>()
            .HasIndex(m => m.Wmi)
            .IsUnique();

        modelBuilder.Entity<Manufacturer>()
            .Property(m => m.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Manufacturer>().HasData(
            new Manufacturer
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Wmi = "1HG",
                Name = "Honda"
            },
            new Manufacturer
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Wmi = "KMH",
                Name = "Hyundai"
            },
            new Manufacturer
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Wmi = "WVW",
                Name = "Volkswagen"
            }
        );
    }
}