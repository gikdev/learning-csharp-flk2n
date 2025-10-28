using App.Models;

namespace App.Repos;

public class MovieRepo : IMovieRepo {
  private readonly List<Movie> _movies = [];

  public Task<bool> CreateAsync(Movie movie) {
    _movies.Add(movie);

    return Task.FromResult(true);
  }

  public Task<bool> DeleteByIdAsync(Guid id) {
    var removedCount = _movies.RemoveAll(m => m.Id == id);
    var movieRemoved = removedCount > 0;

    return Task.FromResult(movieRemoved);
  }

  public Task<IEnumerable<Movie>> GetAllAsync() {
    var movies = _movies.AsEnumerable();

    return Task.FromResult(movies);
  }

  public Task<Movie?> GetByIdAsync(Guid id) {
    var movie = _movies.SingleOrDefault(m => m.Id == id);

    return Task.FromResult(movie);
  }

  public Task<bool> UpdateAsync(Movie movie) {
    var idx = _movies.FindIndex(m => m.Id == movie.Id);

    if (idx == -1) return Task.FromResult(false);

    _movies[idx] = movie;

    return Task.FromResult(true);
  }
}
