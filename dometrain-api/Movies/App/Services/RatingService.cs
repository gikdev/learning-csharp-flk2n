
using App.Models;
using App.Repos;
using FluentValidation;
using FluentValidation.Results;

namespace App.Services;

public class RatingService(IRatingRepo ratingRepo, IMovieRepo movieRepo) : IRatingService {
  private readonly IRatingRepo _ratingRepo = ratingRepo;
  private readonly IMovieRepo _movieRepo = movieRepo;

  public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default) {
    if (rating is <= 0 or > 5) {
      var ratingValidationFailure = new ValidationFailure {
        PropertyName = "Rating",
        ErrorMessage = "Rating must be between 1 and 5"
      };

      throw new ValidationException([ratingValidationFailure]);
    }

    var movieExists = await _movieRepo.ExistsByIdAsync(movieId, token);

    if (!movieExists) return false;

    var result = await _ratingRepo.RateMovieAsync(movieId, rating, userId, token);

    return result;
  }

  public Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default) {
    return _ratingRepo.DeleteRatingAsync(movieId, userId, token);
  }

  public Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default) {
    return _ratingRepo.GetRatingsForUserAsync(userId, token);
  }
}
