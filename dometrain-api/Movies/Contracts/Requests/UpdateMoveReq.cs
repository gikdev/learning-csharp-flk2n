namespace Contracts.Requests;

public class UpdateMoveReq {
  public required string Title { get; init; }
  public required int YearOfRelease { get; init; }
  public required IEnumerable<string> Genres { get; init; } = [];
}
