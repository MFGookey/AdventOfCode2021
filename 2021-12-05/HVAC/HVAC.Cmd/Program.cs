using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using System.Linq;
using HVAC.Core;

namespace HVAC.Cmd
{
  /// <summary>
  /// The entrypoint for HVAC.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// HVAC.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var lines = formatter.FormatFile(filePath, "\n", true, true);

      var plot = new LinePlot(lines);
      plot.GenerateHeatMap((start, end) => start.IsManhattanAligned(end));
      Console.WriteLine(plot.CountHeatAboveThreshold(2));

      plot.GenerateHeatMap((start, end) => start.IsManhattanAligned(end) || start.IsDiagonallyAligned(end));
      Console.WriteLine(plot.CountHeatAboveThreshold(2));

      _ = Console.ReadLine();
    }
  }
}
