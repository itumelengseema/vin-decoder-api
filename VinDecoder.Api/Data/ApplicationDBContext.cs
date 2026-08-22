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
    public DbSet<VinRegion> VinRegions { get; set; }

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

        modelBuilder.Entity<VinRegion>()
            .Property(r => r.Prefix)
            .HasMaxLength(2);

        modelBuilder.Entity<VinRegion>()
            .HasIndex(r => r.Prefix)
            .IsUnique();

        modelBuilder.Entity<VinRegion>()
            .Property(r => r.Country)
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

        modelBuilder.Entity<VinRegion>().HasData(
            // United States
            new VinRegion
                { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Prefix = "1", Country = "United States" },
            new VinRegion
                { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Prefix = "4", Country = "United States" },
            new VinRegion
                { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Prefix = "5", Country = "United States" },
            new VinRegion { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Prefix = "2", Country = "Canada" },
            new VinRegion { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Prefix = "3", Country = "Mexico" },
            new VinRegion { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Prefix = "J", Country = "Japan" },
            new VinRegion
                { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Prefix = "K", Country = "South Korea" },
            new VinRegion
                { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Prefix = "W", Country = "Germany" },

            // South Africa
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), Prefix = "AA", Country = "South Africa" },
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000002"), Prefix = "AB", Country = "South Africa" },
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000003"), Prefix = "AC", Country = "South Africa" },
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000004"), Prefix = "AD", Country = "South Africa" },
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000005"), Prefix = "AE", Country = "South Africa" },
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000006"), Prefix = "AF", Country = "South Africa" },
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000007"), Prefix = "AG", Country = "South Africa" },
            new VinRegion
                { Id = Guid.Parse("20000000-0000-0000-0000-000000000008"), Prefix = "AH", Country = "South Africa" },

            // India
            new VinRegion { Id = Guid.Parse("30000000-0000-0000-0000-000000000001"), Prefix = "MA", Country = "India" },
            new VinRegion { Id = Guid.Parse("30000000-0000-0000-0000-000000000002"), Prefix = "MB", Country = "India" },
            new VinRegion { Id = Guid.Parse("30000000-0000-0000-0000-000000000003"), Prefix = "MC", Country = "India" },
            new VinRegion { Id = Guid.Parse("30000000-0000-0000-0000-000000000004"), Prefix = "MD", Country = "India" },
            new VinRegion { Id = Guid.Parse("30000000-0000-0000-0000-000000000005"), Prefix = "ME", Country = "India" }
        );
    }
}