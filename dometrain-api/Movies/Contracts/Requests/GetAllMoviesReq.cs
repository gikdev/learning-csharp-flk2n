using Microsoft.AspNetCore.Mvc;

namespace Contracts.Requests;

public class GetAllMoviesReq : PagedReq {
  [FromQuery(Name = "title")]
  public required string? Title { get; init; }

  [FromQuery(Name = "year")]
  public required int? Year { get; init; }

  [FromQuery(Name = "sort_by")]
  public required string? SortBy { get; init; }
}
