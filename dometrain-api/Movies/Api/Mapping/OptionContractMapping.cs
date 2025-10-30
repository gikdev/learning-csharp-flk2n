using App.Models;
using Contracts.Requests;

namespace Api.Mapping;

public static class OptionContractMapping {
  public static GetAllMoviesOptions MapToOptions(this GetAllMoviesReq req) {
    var options = new GetAllMoviesOptions {
      Page = req.Page,
      PageSize = req.PageSize,
      Title = req.Title,
      YearOfRelease = req.Year,
      SortField = req.SortBy?.Trim('+', '-'),
      SortOrder = req.SortBy == null
        ? SortOrder.Unsorted
        : req.SortBy.StartsWith('-')
          ? SortOrder.Descending
          : SortOrder.Ascending
    };

    return options;
  }

  public static GetAllMoviesOptions WithUser(this GetAllMoviesOptions options, Guid? userId) {
    options.UserId = userId;

    return options;
  }
}
