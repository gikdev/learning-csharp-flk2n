namespace Contracts.Responses;

public class PagedRes<TRes> {
  public required IEnumerable<TRes> Items { get; init; } = [];
  public required int Page { get; init; }
  public required int PageSize { get; init; }
  public required int Total { get; init; }
  public bool HasNextPage => Total > (Page * PageSize);
}
