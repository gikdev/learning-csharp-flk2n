using AutoMapper;
using IntermediateStuff.Data;
using IntermediateStuff.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IntermediateStuff;

public class Program {
  public static void Main() {
    var comps = File.ReadAllText("ComputersSnake.json");
  }

  public static void JsonishStuff() {
    IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    var dp = new DataContextDapper(config);

    var computersJson = File.ReadAllText("Computers.json");
    Console.WriteLine(computersJson);

    // var options = new JsonSerializerOptions {
    //   PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    // };

    // var computers = JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);
    var computers = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);

    if (computers is null) return;
    foreach (var c in computers) {
      // Console.WriteLine($"Motherboard: {c.Motherboard}");
    }

    var compsCopy = JsonConvert.SerializeObject(computers);

    File.WriteAllText("computers-copy.json", compsCopy);
  }

  public static void DbStuff() {
    IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    var dp = new DataContextDapper(config);
    var ef = new DataContextEf(config);

    var myPc = new Computer {
      Motherboard = "Z690",
      CpuCores = 8,
      HasWifi = true,
      HasLte = false,
      ReleaseDate = DateTime.Now,
      Price = 943.87m,
      VideoCard = "RTX 2060"
    };

    ef.Add(myPc);
    ef.SaveChanges();

    var computers = ef.Computer?.ToList<Computer>();

    if (computers is not null) {
      foreach (var c in computers) {
        PrintComputerDetails(c);
      }
    }
  }

  static void PrintComputerDetails(Computer pc) {
    Console.WriteLine("===== Computer Details =====");
    Console.WriteLine("{0,-15}   {1}", "PROPERTY", "VALUE");
    Console.WriteLine("{0,-15}   {1}", "ID", pc.ComputerId);
    Console.WriteLine("{0,-15}   {1}", "Motherboard", pc.Motherboard);
    Console.WriteLine("{0,-15}   {1}", "CPU Cores", pc.CpuCores);
    Console.WriteLine("{0,-15}   {1}", "Wi-Fi", pc.HasWifi ? "Yes" : "No");
    Console.WriteLine("{0,-15}   {1}", "LTE", pc.HasLte ? "Yes" : "No");
    Console.WriteLine("{0,-15}   {1:yyyy-MM-dd}", "Release Date", pc.ReleaseDate);
    Console.WriteLine("{0,-15}   ${1:F2}", "Price", pc.Price);
    Console.WriteLine("{0,-15}   {1}", "Video Card", pc.VideoCard);
    Console.WriteLine("============================");
  }
}
