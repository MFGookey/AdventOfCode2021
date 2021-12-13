using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using System.Linq;
using Spelunker.Core;

namespace Spelunker.Cmd
{
  /// <summary>
  /// The entrypoint for Spelunker.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Spelunker.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var graph = new Graph(formatter.FormatFile(filePath, "\n", true, true));

      Console.WriteLine(graph.Traverse("start", "end", Node.CanVisitRule).Count());
      Console.WriteLine(graph.Traverse("start", "end", Node.CanRevisitRule).Count());
      _ = Console.ReadLine();
    }
  }
}
