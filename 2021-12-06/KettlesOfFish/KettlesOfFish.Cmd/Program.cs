using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using System.Linq;
using KettlesOfFish.Core;

namespace KettlesOfFish.Cmd
{
  /// <summary>
  /// The entrypoint for KettlesOfFish.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// KettlesOfFish.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());

      var inputs = formatter.FormatFile(filePath, ",", true, true).Select(s => s.Replace("\n", string.Empty));

      var kettles = new FishKettles(inputs, 7, 2);

      kettles.Tick(80);

      Console.WriteLine(kettles.CurrentPopulation);

      // keep ticking until 256 total ticks
      kettles.Tick(256 - 80);
      Console.WriteLine(kettles.CurrentPopulation);

      _ = Console.ReadLine();
    }
  }
}
