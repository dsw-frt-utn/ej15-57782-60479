using Dsw2026Ej15.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data;

public class Dsw2026Ej15Context : DbContext
{
    public Dsw2026Ej15Context(DbContextOptions<Dsw2026Ej15Context> options)
        : base(options) { }

    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Speciality> Specialities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Speciality>(entity =>
        {
            entity.ToTable("Specialities");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Description).HasMaxLength(300);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctors");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(150);
            entity.Property(d => d.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.Property(d => d.IsActive).IsRequired();

            entity.HasOne(d => d.Speciality)
                  .WithMany()
                  .HasForeignKey("SpecialityId")
                  .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
