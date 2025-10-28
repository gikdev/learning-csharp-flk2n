namespace Contracts.Responses;

public class MoviesRes {
  public required IEnumerable<MovieRes> Items { get; init; } = [];
}
