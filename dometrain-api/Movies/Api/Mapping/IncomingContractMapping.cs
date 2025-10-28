using App.Models;
using Contracts.Requests;

namespace Api.Mapping;

public static class IncomingContractMapping {
  public static Movie MapToMovie(this CreateMoveReq req) {
    var movie = new Movie {
      Id = Guid.NewGuid(),
      Title = req.Title,
      YearOfRelease = req.YearOfRelease,
      Genres = [.. req.Genres],
    };

    return movie;
  }

  public static Movie MapToMovie(this UpdateMoveReq req, Guid id) {
    var movie = new Movie {
      Id = id,
      Title = req.Title,
      YearOfRelease = req.YearOfRelease,
      Genres = [.. req.Genres],
    };

    return movie;
  }
}
