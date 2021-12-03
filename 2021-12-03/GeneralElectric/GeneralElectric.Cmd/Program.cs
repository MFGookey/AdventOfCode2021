using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using GeneralElectric.Core;
using System.Linq;

namespace GeneralElectric.Cmd
{
  /// <summary>
  /// The entrypoint for GeneralElectric.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// GeneralElectric.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var readings = reader.ReadFileByLines(filePath);
      var calc = new ConsumptionCalculator(
        readings.Select(reading => new ConsumptionReading(reading))
      );

      Console.WriteLine(calc.CalculateGamma() * calc.CalculateEpsilon());
      
      _ = Console.ReadLine();
    }
  }
}
