namespace Contracts.Responses;

public class MovieRatingRes {
  public required Guid MovieId { get; init; }
  public required string Slug { get; init; }
  public required int Rating { get; init; }
}
