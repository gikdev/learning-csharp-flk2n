using System.Text.Json.Serialization;

namespace Contracts.Responses;

public abstract class HalRes {
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public List<Link> Links { get; set; } = [];
}

public class Link {
  public required string Rel { get; init; }
  public required string Href { get; init; }
  public required string Type { get; init; }
}
