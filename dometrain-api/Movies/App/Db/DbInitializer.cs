using Dapper;

namespace App.Db;

public class DbInitializer(IDbConnectionFactory dbConnectionFactory) {
  private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

  public async Task InitializeAsync() {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync();

    await conn.ExecuteAsync("""
      CREATE TABLE IF NOT EXISTS movies (
        id UUID PRIMARY KEY,
        slug TEXT NOT NULL,
        title TEXT NOT NULL,
        yearofrelease INTEGER NOT NULL
      );
    """);

    await conn.ExecuteAsync("""
      CREATE UNIQUE INDEX CONCURRENTLY IF NOT EXISTS movies_slug_idx
      ON movies
      USING BTREE(slug);
    """);

    await conn.ExecuteAsync("""
      CREATE TABLE IF NOT EXISTS genres (
        movieId UUID REFERENCES movies (id),
        name TEXT NOT NULL
      );
    """);
  }
}
