using App.Models;

namespace App.Repos;

public interface IMovieRepo {
  Task<bool> CreateAsync(Movie movie);
  Task<Movie?> GetByIdAsync(Guid id);
  Task<IEnumerable<Movie>> GetAllAsync();
  Task<bool> UpdateAsync(Movie movie);
  Task<bool> DeleteByIdAsync(Guid id);
}
