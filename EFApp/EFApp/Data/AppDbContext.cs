using EFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFApp.Data;

public class AppDbContext : DbContext {

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
  }

  public DbSet<Person> Persons => Set<Person>();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Person>(entity => {
      entity.Property(p => p.Name).HasMaxLength(100);
      entity.Property(p => p.City).HasMaxLength(100);
    });
  }
}
