using App.Models;
using Contracts.Responses;

namespace Api.Mapping;

public static class OutgoingContractMapping {
  public static MovieRes MapToResponse(this Movie movie) {
    var res = new MovieRes {
      Id = movie.Id,
      Title = movie.Title,
      Slug = movie.Slug,
      Rating = movie.Rating,
      UserRating = movie.UserRating,
      YearOfRelease = movie.YearOfRelease,
      Genres = movie.Genres,
    };

    return res;
  }

  public static MoviesRes MapToResponse(this IEnumerable<Movie> movies) {
    var res = new MoviesRes {
      Items = movies.Select(MapToResponse)
    };

    return res;
  }

  public static IEnumerable<MovieRatingRes> MapToResponse(this IEnumerable<MovieRating> ratings) {
    var res = ratings.Select(r => new MovieRatingRes {
      Rating = r.Rating,
      MovieId = r.MovieId,
      Slug = r.Slug,
    });

    return res;
  }
}
