using App.Models;
using App.Repos;
using FluentValidation;

namespace App.Services;

public class MovieService(IMovieRepo movieRepo, IValidator<Movie> movieValidator) : IMovieService {
  private readonly IMovieRepo _movieRepo = movieRepo;
  private readonly IValidator<Movie> _movieValidator = movieValidator;

  public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default) {
    await _movieValidator.ValidateAndThrowAsync(movie);
    return await _movieRepo.CreateAsync(movie);
  }

  public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default) => _movieRepo.GetAllAsync(token);

  public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default) => _movieRepo.GetByIdAsync(id, token);

  public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default) => _movieRepo.GetBySlugAsync(slug, token);

  public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default) {
    await _movieValidator.ValidateAndThrowAsync(movie, token);
    var movieExists = await _movieRepo.ExistsByIdAsync(movie.Id, token);
    if (!movieExists) return null;

    await _movieRepo.UpdateAsync(movie, token);

    return movie;
  }

  public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default) => _movieRepo.DeleteByIdAsync(id, token);
}
