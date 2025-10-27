using IntermediateStuff.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntermediateStuff.Data;

public class DataContextEf(IConfiguration config) : DbContext {
  private readonly IConfiguration _config = config;
  private string? ConnStr => _config.GetConnectionString("DefaultConnection");
  public DbSet<ComputerSnake>? Computer { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    // base.OnConfiguring(optionsBuilder);

    if (optionsBuilder.IsConfigured) return;

    optionsBuilder
      .UseSqlServer(
        ConnStr,
        options => options.EnableRetryOnFailure()
      );
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.HasDefaultSchema("TutorialAppSchema");

    modelBuilder.Entity<ComputerSnake>().HasKey(c => c.ComputerId);
  }
}
