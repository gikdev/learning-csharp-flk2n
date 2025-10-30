using EfApi1.Models;
using EfApi1.Models.EntityMapping;
using Microsoft.EntityFrameworkCore;

namespace EfApi1.Data;

public class MoviesContext : DbContext {
  public DbSet<Movie> Movies => Set<Movie>();

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    const string connStr = """
      Data Source=localhost;
      Initial Catalog=MoviesDB;
      User Id=sa;
      Password=mmsfllfbns;
      TrustServerCertificate=true;
    """;

    optionsBuilder.UseSqlServer(connStr);

    // optionsBuilder.LogTo(Console.WriteLine);

    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.ApplyConfiguration(new GenreMapping());
    modelBuilder.ApplyConfiguration(new MovieMapping());

    base.OnModelCreating(modelBuilder);
  }
}
