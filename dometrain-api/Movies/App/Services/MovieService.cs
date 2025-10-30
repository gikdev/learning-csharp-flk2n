using App.Models;
using App.Repos;
using FluentValidation;

namespace App.Services;

public class MovieService(
  IMovieRepo movieRepo,
  IValidator<Movie> movieValidator,
  IRatingRepo ratingRepo,
  IValidator<GetAllMoviesOptions> optionsValidator
) : IMovieService {
  public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default) {
    await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
    return await movieRepo.CreateAsync(movie, token);
  }

  public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default) {
    await optionsValidator.ValidateAndThrowAsync(options, token);

    return await movieRepo.GetAllAsync(options, token);
  }

  public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default) =>
    movieRepo.GetByIdAsync(id, userId, token);

  public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default) =>
    movieRepo.GetBySlugAsync(slug, userId, token);

  public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default) {
    await movieValidator.ValidateAndThrowAsync(movie, token);
    var movieExists = await movieRepo.ExistsByIdAsync(movie.Id, token);
    if (!movieExists) return null;

    await movieRepo.UpdateAsync(movie, token);

    if (!userId.HasValue) {
      var rating = await ratingRepo.GetRatingAsync(movie.Id, token);
      movie.Rating = rating;
      return movie;
    }

    var ratings = await ratingRepo.GetRatingAsync(movie.Id, userId.Value, token);
    movie.Rating = ratings.Rating;
    movie.UserRating = ratings.UserRating;
    return movie;
  }

  public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default) =>
    movieRepo.DeleteByIdAsync(id, token);

  public Task<int> GetCountAsync(string? title, int? year, CancellationToken token = default) =>
    movieRepo.GetCountAsync(title, year, token);
}
