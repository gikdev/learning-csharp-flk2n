using System.Data;
using App.Db;
using App.Models;
using Dapper;

namespace App.Repos;

public class MovieRepo(IDbConnectionFactory dbConnectionFactory) : IMovieRepo {
  private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

  public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
    using var transaction = conn.BeginTransaction();

    var insertMovieCmd = new CommandDefinition("""
      INSERT INTO movies (id, slug, title, yearofrelease)
      VALUES (@Id, @Slug, @Title, @YearOfRelease)
    """, movie, cancellationToken: token);

    var result = await conn.ExecuteAsync(insertMovieCmd);

    if (result > 0) {
      foreach (var genre in movie.Genres) {
        var insertGenreCmd = new CommandDefinition("""
          INSERT INTO genres (movieId, name)
          VALUES (@MovieId, @Name);
        """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token);

        await conn.ExecuteAsync(insertGenreCmd);
      }
    }

    transaction.Commit();

    return result > 0;
  }

  public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var getAllMoviesCmd = new CommandDefinition("""
      SELECT m.*, STRING_AGG(g.name, ',') AS genres
      FROM movies m
      LEFT JOIN genres g
        ON m.id = g.movieid
      GROUP BY id;
    """, cancellationToken: token);

    var result = await conn.QueryAsync(getAllMoviesCmd);

    var movies = result.Select(i => new Movie {
      Id = i.id,
      Title = i.title,
      YearOfRelease = i.yearofrelease,
      Genres = Enumerable.ToList(i.genres.Split(','))
    });

    return movies;
  }

  public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var findMovieCmd = new CommandDefinition("""
      SELECT *
      FROM movies
      WHERE id = @id
    """, new { id }, cancellationToken: token);

    var movie = await conn.QuerySingleOrDefaultAsync<Movie>(findMovieCmd);

    if (movie is null) return null;

    var getGenresCmd = new CommandDefinition("""
      SELECT name
      FROM genres
      WHERE movieid = @id
    """, new { id }, cancellationToken: token);

    var genreNames = await conn.QueryAsync<string>(getGenresCmd);

    foreach (var genreName in genreNames) {
      movie.Genres.Add(genreName);
    }

    return movie;
  }

  public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var findMovieCmd = new CommandDefinition("""
      SELECT *
      FROM movies
      WHERE slug = @slug
    """, new { slug }, cancellationToken: token);

    var movie = await conn.QuerySingleOrDefaultAsync<Movie>(findMovieCmd);

    if (movie is null) return null;

    var getGenresCmd = new CommandDefinition("""
      SELECT name
      FROM genres
      WHERE movieid = @id
    """, new { id = movie.Id }, cancellationToken: token);

    var genreNames = await conn.QueryAsync<string>(getGenresCmd);

    foreach (var genreName in genreNames) {
      movie.Genres.Add(genreName);
    }

    return movie;
  }

  public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
    using var transaction = conn.BeginTransaction();

    var removeAllGenresCmd = new CommandDefinition("""
      DELETE
      FROM genres
      WHERE movieid = @id
    """, new { id = movie.Id }, cancellationToken: token);

    await conn.ExecuteAsync(removeAllGenresCmd);

    foreach (var genre in movie.Genres) {
      var insertGenreCmd = new CommandDefinition("""
        INSERT INTO genres (movieId, name)
        VALUES (@MovieId, @Name);
      """, new { MovieId = movie.Id, Name = genre }, cancellationToken: token);

      await conn.ExecuteAsync(insertGenreCmd);
    }

    var updateMovieCmd = new CommandDefinition("""
      UPDATE movies
      SET
          slug          = @Slug
        , title         = @Title
        , yearofrelease = @YearOfRelease
      WHERE id = @Id
    """, movie, cancellationToken: token);

    var result = await conn.ExecuteAsync(updateMovieCmd);

    transaction.Commit();

    return result > 0;
  }

  public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
    using var transaction = conn.BeginTransaction();

    var deleteGenresCmd = new CommandDefinition("""
      DELETE
      FROM genres
      WHERE movieid = @id
    """, new { id }, cancellationToken: token);

    await conn.ExecuteAsync(deleteGenresCmd);

    var deleteMovieCmd = new CommandDefinition("""
      DELETE
      FROM movies
      WHERE id = @id
    """, new { id }, cancellationToken: token);

    var result = await conn.ExecuteAsync(deleteMovieCmd);

    transaction.Commit();

    return result > 0;
  }

  public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var existsCmd = new CommandDefinition("""
      SELECT COUNT(1)
      FROM movies
      WHERE id = @id
    """, new { id }, cancellationToken: token);

    var result = await conn.ExecuteScalarAsync<bool>(existsCmd);

    return result;
  }
}
