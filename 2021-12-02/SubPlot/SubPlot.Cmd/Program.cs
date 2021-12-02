using System;
using Common.Utilities.Formatter;
using Common.Utilities.IO;
using SubPlot.Core;

namespace SubPlot.Cmd
{
  /// <summary>
  /// The entrypoint for SubPlot.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// SubPlot.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var formatter = new RecordFormatter(new FileReader());
      var courseInstructions = formatter.FormatFile(filePath, "\n", true);

      int currentDepth = 0;
      int currentDistance = 0;

      foreach (var instruction in courseInstructions)
      {
        var command = new NavigationStep(instruction);
        currentDepth += command.ChangeInDepth();
        currentDistance += command.ChangeInHorizontal();
      }

      Console.WriteLine(currentDepth * currentDistance);

      int currentAim = 0;
      currentDepth = 0;
      currentDistance = 0;

      foreach (var instruction in courseInstructions)
      {
        var command = new NavigationStep(instruction);
        currentAim += command.ChangeInDepth();
        currentDistance += command.ChangeInHorizontal();
        currentDepth += (currentAim * command.ChangeInHorizontal());
      }

      Console.WriteLine(currentDepth * currentDistance);

      _ = Console.ReadLine();
    }
  }
}
