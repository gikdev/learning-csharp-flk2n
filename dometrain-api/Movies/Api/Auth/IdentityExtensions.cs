namespace Api.Auth;

public static class IdentityExtensions {
  public static Guid? GetUserId(this HttpContext ctx) {
    var userId = ctx.User.Claims.SingleOrDefault(c => c.Type == "userid");

    if (Guid.TryParse(userId?.Value, out var parsedId)) return parsedId;

    return null;
  }
}
