using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using SquidGame.Core;
using System.Linq;

namespace SquidGame.Cmd
{
  /// <summary>
  /// The entrypoint for SquidGame.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// SquidGame.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());

      // The first record is a comma delimited list of integers representing draws from the available numbers
      // Every record from then on is a bingo board
      var records = formatter.FormatFile(filePath, "\n\n", true, true);

      int i = 0;

      var draws = records.First();

      var boards = records.Skip(1);

      var processor = new BingoGameProcessor(draws, boards);
      var winningBoard = processor.PlayUntilBingo();
      Console.WriteLine(winningBoard.Score);

      _ = Console.ReadLine();
    }
  }
}
