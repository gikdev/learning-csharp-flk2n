namespace Contracts.Responses;

public class ValidationFailureRes {
  public required IEnumerable<ValidationRes> Errors { get; init; }
}

public class ValidationRes {
  public required string PropertyName { get; init; }
  public required string Message { get; init; }
}
