using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using System.Linq;
using IRigami.Core;

namespace IRigami.Cmd
{
  /// <summary>
  /// The entrypoint for IRigami.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// IRigami.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var records = formatter.FormatFile(filePath, "\n\n", true, true);
      var coordinates = formatter.FormatRecord(records.First(), "\n", true);
      var folds = formatter.FormatRecord(records.Skip(1).First(), "\n", true);

      var sheet = new FoldableSheet(coordinates, folds);
      sheet.Tick();
      Console.WriteLine(sheet.CountVisiblePoints());

      _ = Console.ReadLine();
    }
  }
}
