using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SevenSegmentDismay.Core
{
  public class DisplayInterpreter
  {
    public IEnumerable<DisplayTestCycle> TestCycles
    {
      get;
      private set;
    }

    public DisplayInterpreter(IEnumerable<string> displayTestCycles)
    {
      var parsedCycles = new List<DisplayTestCycle>();

      foreach (string testCycle in displayTestCycles)
      {
        parsedCycles.Add(new DisplayTestCycle(testCycle));
      }

      TestCycles = parsedCycles;
    }

    public int FindUniqueDisplayedDigitsTotal()
    {
      // Looking for any tests where the display is a digit indentifiable by the number of lit segments

      var uniqueByLitSegmentCount = new List<int>();
      uniqueByLitSegmentCount.Add(Display.ONE.LitSegmentCount);
      uniqueByLitSegmentCount.Add(Display.FOUR.LitSegmentCount);
      uniqueByLitSegmentCount.Add(Display.SEVEN.LitSegmentCount);
      uniqueByLitSegmentCount.Add(Display.EIGHT.LitSegmentCount);

      return TestCycles
        .Select(
          test =>
            test
            .DisplayReading
            .Count(
              display => 
              uniqueByLitSegmentCount.Contains(display.LitSegmentCount)
            )
        )
        .Sum();
    }
  }
}
