using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using SevenSegmentDismay.Core;

namespace SevenSegmentDismay.Cmd
{
  /// <summary>
  /// The entrypoint for SevenSegmentDismay.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// SevenSegmentDismay.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var rawRecords = formatter.FormatFile(filePath, "\n", true, true);
      // Record is of the form "afebd ecdgfb gacfed dgaeb bf acefd fgdabec bfd bedcaf bafc | afcbed fb bfd bdf"
      // First section is cycling through 0 to 9 and logging which segments are lit, second section is a 4 digit number that is displayed

      var interpreter = new DisplayInterpreter(rawRecords);
      Console.WriteLine(interpreter.FindUniqueDisplayedDigitsTotal());
      Console.WriteLine(interpreter.FindSumOfDisplays());
      _ = Console.ReadLine();
    }
  }
}
