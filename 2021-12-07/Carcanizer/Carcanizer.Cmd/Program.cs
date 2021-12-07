using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using System.Linq;
using Carcanizer.Core;

namespace Carcanizer.Cmd
{
  /// <summary>
  /// The entrypoint for Carcanizer.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Carcanizer.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var crabNavy = formatter.FormatFile(filePath, ",", true, true).Select(s => s.Replace("\n", string.Empty));

      var commander = new CrabCommander(crabNavy);
      Console.WriteLine(commander.CalculateCrabsOfTheLinePositionCost(CrabCommander.FlatConsumptionRule));

      Console.WriteLine(commander.CalculateCrabsOfTheLinePositionCost(CrabCommander.SumConsumptionRule));

      _ = Console.ReadLine();
    }
  }
}
