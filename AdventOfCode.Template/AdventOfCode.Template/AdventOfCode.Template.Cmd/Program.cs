using System;
using Common.Utilities.IO;
//using AdventOfCode.Template.Core;

namespace AdventOfCode.Template.Cmd
{
  /// <summary>
  /// The entrypoint for AdventOfCode.Template.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// AdventOfCode.Template.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(filePath).Length);
      _ = Console.ReadLine();
    }
  }
}
