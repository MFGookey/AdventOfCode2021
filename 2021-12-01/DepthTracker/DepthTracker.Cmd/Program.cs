using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using DepthTracker.Core;
using System.Linq;

namespace DepthTracker.Cmd
{
  /// <summary>
  /// The entrypoint for DepthTracker.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// DepthTracker.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";

      var formatter = new RecordFormatter(new FileReader());

      var stringDepths = formatter.FormatFile(filePath, "\n", true);
      var depths = stringDepths.Select(reading => int.Parse(reading));

      var analyzer = new DepthAnalyzer(depths);

      Console.WriteLine(analyzer.CountIncreases());
      Console.WriteLine(analyzer.CountIncreases(3));
    }
  }
}
