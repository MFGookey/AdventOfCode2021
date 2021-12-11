using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using System.Linq;
using LiterallyMyThesis.Core;

namespace LiterallyMyThesis.Cmd
{
  /// <summary>
  /// The entrypoint for LiterallyMyThesis.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// LiterallyMyThesis.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var lines = formatter.FormatFile(filePath, "\n", true, true);
      var scorer = new SyntaxScorer(lines);
      Console.WriteLine(scorer.ScoreLines());
      _ = Console.ReadLine();
    }
  }
}
