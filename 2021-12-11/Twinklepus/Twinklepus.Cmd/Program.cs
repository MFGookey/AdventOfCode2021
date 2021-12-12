using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using System.Linq;
using Twinklepus.Core;

namespace Twinklepus.Cmd
{
  /// <summary>
  /// The entrypoint for Twinklepus.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Twinklepus.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());

      var grid = new OctoGrid(formatter.FormatFile(filePath, "\n", true, true));
      grid.TickUntil(100);
      Console.WriteLine(grid.FlashCount);
      grid.TickUntilSynchronized();
      Console.Write(grid.FirstSynchronizedFlash.Value);
      _ = Console.ReadLine();
    }
  }
}
