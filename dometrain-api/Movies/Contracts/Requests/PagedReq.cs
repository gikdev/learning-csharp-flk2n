using Microsoft.AspNetCore.Mvc;

namespace Contracts.Requests;

public class PagedReq {
  [FromQuery(Name = "page")]
  public required int Page { get; init; } = 1;

  [FromQuery(Name = "page_size")]
  public required int PageSize { get; init; } = 10;
}
