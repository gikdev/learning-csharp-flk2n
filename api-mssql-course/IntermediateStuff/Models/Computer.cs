using System.Text.Json.Serialization;

namespace IntermediateStuff.Models;

public class Computer {
  [JsonPropertyName("computerId")]
  public int ComputerId { get; set; }

  [JsonPropertyName("motherboard")]
  public string Motherboard { get; set; } = string.Empty;

  public int CpuCores { get; set; }

  public bool HasWifi { get; set; }

  public bool HasLte { get; set; }

  public DateTime? ReleaseDate { get; set; }

  public decimal Price { get; set; }

  public string VideoCard { get; set; } = string.Empty;
}
