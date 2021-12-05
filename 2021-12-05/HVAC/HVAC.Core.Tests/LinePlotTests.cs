using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HVAC.Core.Tests
{
  public class LinePlotTests
  {
    [Theory]
    [MemberData(nameof(SampleLines))]
    void LinePlot_GivenValidLineDefinitions_DoesNotThrowException(IEnumerable<string> definitions, Func<Point, Point, bool> rule, int expectedHotSpots)
    {
      var exception = Record.Exception(() => _ = new LinePlot(definitions));

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(SampleLines))]
    void LinePlot_GivenValidLineDefinitions_GeneratesExpectedManhattanHotSpots(IEnumerable<string> definitions, Func<Point, Point, bool> rule, int expectedHotSpots)
    {
      var sut = new LinePlot(definitions);
      sut.GenerateHeatMap(rule);
      Assert.Equal(expectedHotSpots, sut.CountHeatAboveThreshold(2));
    }

    public static IEnumerable<object[]> SampleLines
    {
      get {
        yield return new object[]
        {
          new[] {
            "0,9 -> 5,9",
            "8,0 -> 0,8",
            "9,4 -> 3,4",
            "2,2 -> 2,1",
            "7,0 -> 7,4",
            "6,4 -> 2,0",
            "0,9 -> 2,9",
            "3,4 -> 1,4",
            "0,0 -> 8,8",
            "5,5 -> 8,2"
          },
          new Func<Point, Point, bool>((start, end) => start.IsManhattanAligned(end)),
          5
        };

        yield return new object[]
        {
          new[] {
            "0,9 -> 5,9",
            "8,0 -> 0,8",
            "9,4 -> 3,4",
            "2,2 -> 2,1",
            "7,0 -> 7,4",
            "6,4 -> 2,0",
            "0,9 -> 2,9",
            "3,4 -> 1,4",
            "0,0 -> 8,8",
            "5,5 -> 8,2"
          },
          new Func<Point, Point, bool>((start, end) => start.IsManhattanAligned(end) || start.IsDiagonallyAligned(end)),
          12
        };
      }
    }
  }
}
