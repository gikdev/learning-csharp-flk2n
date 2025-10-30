
using App.Db;
using App.Models;
using Dapper;

namespace App.Repos;

public class RatingRepo(IDbConnectionFactory dbConnectionFactory) : IRatingRepo {
  private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

  public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var cmd = new CommandDefinition("""
      SELECT ROUND(AVG(r.rating), 1)
      FROM ratings r
      WHERE movieid = @movieId
      ;
    """, new { movieId }, cancellationToken: token);

    var res = await conn.QuerySingleOrDefaultAsync<float?>(cmd);

    return res;
  }

  public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var cmd = new CommandDefinition("""
      SELECT
        ROUND(AVG(r.rating), 1),
        (
          SELECT rating
          FROM ratings
          WHERE movieid = @movieId
            AND userid = @userId
          LIMIT 1
        )
      FROM ratings r
      WHERE movieid = @movieId
      ;
    """, new { movieId, userId }, cancellationToken: token);

    var res = await conn.QuerySingleOrDefaultAsync<(float? Rating, int? UserRating)>(cmd);

    return res;
  }

  public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var cmd = new CommandDefinition("""
      INSERT INTO ratings (userid, movieid, rating)
      VALUES (@userId, @movieId, @rating)
      ON CONFLICT (userid, movieid)
        DO UPDATE
          SET rating = @rating
      ;
    """, new { userId, movieId, rating }, cancellationToken: token);

    var result = await conn.ExecuteAsync(cmd);

    return result > 0;
  }

  public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var cmd = new CommandDefinition("""
      DELETE
      FROM ratings
      WHERE movieid = @movieId
        AND userid = @userId
      ;
    """, new { userId, movieId }, cancellationToken: token);

    var result = await conn.ExecuteAsync(cmd);

    return result > 0;
  }

  public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default) {
    using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);

    var cmd = new CommandDefinition("""
      SELECT
        r.rating,
        r.movieid,
        m.slug
      FROM ratings r
        INNER JOIN movies m
          ON r.movieid = m.id
      WHERE userid = @userId
      ;
    """, new { userId }, cancellationToken: token);

    var ratings = await conn.QueryAsync<MovieRating>(cmd);

    return ratings;
  }
}
