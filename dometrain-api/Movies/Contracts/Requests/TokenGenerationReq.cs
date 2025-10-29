namespace Contracts.Requests;

public class TokenGenerationReq {
  public required Guid UserId { get; set; }

  public required string Email { get; set; }

  public required Dictionary<string, object> CustomClaims { get; set; } = [];
}
