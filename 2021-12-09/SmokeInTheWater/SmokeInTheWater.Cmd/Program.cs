using System;
using System.Linq;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using SmokeInTheWater.Core;

namespace SmokeInTheWater.Cmd
{
  /// <summary>
  /// The entrypoint for SmokeInTheWater.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// SmokeInTheWater.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var fileByRows = formatter.FormatFile(filePath, "\n", true, true);

      var plot = new DepthPlot(fileByRows);

      Console.WriteLine(plot.FindRiskLevels(DepthPlot.BasicRule).Sum());

      _ = Console.ReadLine();
    }
  }
}
