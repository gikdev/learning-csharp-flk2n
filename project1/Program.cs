namespace Project1;

internal class Program {
  static void Main(string[] args) {
    var name = args[0];

    if (name is null)
      throw new Exception("Provide a name as an argument plz!");

    Console.WriteLine($"Hello {name}!");
  }
}
