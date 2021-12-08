using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace SevenSegmentDismay.Core
{
  public class DisplayTestCycle
  {
    public IEnumerable<Display> TestResults
    {
      get;
      private set;
    }

    public IReadOnlyList<Display> DisplayReading
    {
      get;
      private set;
    } 

    public DisplayTestCycle(string results)
    {
      if (results == null)
      {
        throw new ArgumentNullException(nameof(results));
      }

      if (Regex.IsMatch(results, @"^(?:[a-g]{1,7} ){10}\|(?: [a-g]{1,7}){4}$") == false)
      {
        throw new ArgumentException("Results must match the format of \"{DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} | {DisplayString} {DisplayString} {DisplayString} {DisplayString}", nameof(results));
      }

      var splitResults = results.Split("|");

      TestResults = splitResults[0].Trim().Split(" ").Select(result => new Display(result));

      DisplayReading = splitResults[1].Trim().Split(" ").Select(reading => new Display(reading)).ToList();
    }
  }
}
