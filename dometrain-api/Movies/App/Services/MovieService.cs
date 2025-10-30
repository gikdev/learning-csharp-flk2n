using App.Models;
using App.Repos;
using FluentValidation;

namespace App.Services;

public class MovieService(
  IMovieRepo movieRepo,
  IValidator<Movie> movieValidator,
  IRatingRepo ratingRepo
  ) : IMovieService {
  private readonly IMovieRepo _movieRepo = movieRepo;
  private readonly IValidator<Movie> _movieValidator = movieValidator;
  private readonly IRatingRepo _ratingRepo = ratingRepo;

  public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default) {
    await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
    return await _movieRepo.CreateAsync(movie, token);
  }

  public Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default) => _movieRepo.GetAllAsync(userId, token);

  public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default) => _movieRepo.GetByIdAsync(id, userId, token);

  public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default) => _movieRepo.GetBySlugAsync(slug, userId, token);

  public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default) {
    await _movieValidator.ValidateAndThrowAsync(movie, token);
    var movieExists = await _movieRepo.ExistsByIdAsync(movie.Id, token);
    if (!movieExists) return null;

    await _movieRepo.UpdateAsync(movie, token);

    if (!userId.HasValue) {
      var rating = await _ratingRepo.GetRatingAsync(movie.Id, token);
      movie.Rating = rating;
      return movie;
    }

    var (Rating, UserRating) = await _ratingRepo.GetRatingAsync(movie.Id, userId.Value, token);
    movie.Rating = Rating;
    movie.UserRating = UserRating;
    return movie;
  }

  public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default) => _movieRepo.DeleteByIdAsync(id, token);
}
