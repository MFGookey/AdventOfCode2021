using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace SmokeInTheWater.Core.Tests
{
  public class DepthPlotTests
  {
    [Theory]
    [MemberData(nameof(MalformedDepthReadings))]
    void DepthPlot_GivenMalformedDepthReading_ThrowsArgumentException(IEnumerable<string> badReadings)
    {
      Assert.Throws<ArgumentException>(
        () => _ = new DepthPlot(badReadings)
      );
    }

    [Theory]
    [MemberData(nameof(WellFormedDepthReadings))]
    void DepthPLot_GivenWellFormedDepthReading_DoesNotThrowException(IEnumerable<string> goodReadings)
    {
      var exception = Record.Exception(
          () => _ = new DepthPlot(goodReadings)
        );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(WellFormedDepthReadings))]
    void FindLocalMinimums_DoesNotThrowException(IEnumerable<string> goodReadings)
    {
      var sut = new DepthPlot(goodReadings);
      var exception = Record.Exception(
          () => _ = sut.FindLocalMinimums()
        );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(DepthsWithMinimums))]
    void FindLocalMinimums_ReturnsExpectedMinimums(IEnumerable<string> readings, IEnumerable<int> expectedMinimums)
    {
      var sut = new DepthPlot(readings);
      Assert.Equal(expectedMinimums, sut.FindLocalMinimums());
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(-1, 0)]
    [InlineData(5, 6)]
    [InlineData(9, 10)]
    [InlineData(100, 101)]
    void BasicRule_ReturnsExpectedResult(int value, int expected)
    {
      Assert.Equal(expected, DepthPlot.BasicRiskRule(value));
    }

    [Theory]
    [MemberData(nameof(DepthsWithBasicRiskLevels))]
    void FindRiskLevels_GivenBasicRule_ReturnsExpectedResults(IEnumerable<string> readings, IEnumerable<int> expectedRiskLevels)
    {
      var sut = new DepthPlot(readings);
      Assert.Equal(expectedRiskLevels, sut.FindRiskLevels(DepthPlot.BasicRiskRule));
    }

    [Theory]
    [MemberData(nameof(DepthsWithMinimums))]
    void FindRiskLevels_GivenTrivialRule_ReturnsExpectedResults(IEnumerable<string> readings, IEnumerable<int> expectedRiskLevels)
    {
      var sut = new DepthPlot(readings);
      Assert.Equal(expectedRiskLevels, sut.FindRiskLevels(x => x));
    }

    [Theory]
    [MemberData(nameof(WellFormedDepthReadings))]
    void FindBasinSizes_DoesNotThrowException(IEnumerable<string> goodReadings)
    {
      var sut = new DepthPlot(goodReadings);
      var exception = Record.Exception(
          () => _ = sut.FindBasinSizes()
        );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(DepthsWithBasinSizes))]
    void FindBasinSizes_ReturnsExpectedResults(IEnumerable<string> readings, IEnumerable<int> expectedBasinSizes)
    {
      var sut = new DepthPlot(readings);
      Assert.Equal(expectedBasinSizes, sut.FindBasinSizes());
    }

    [Theory]
    [MemberData(nameof(BasinSizesWithAggregateRisk))]
    void BasinRiskRule_ReturnsExpectedRisk(IEnumerable<int> basinSizes, int expectedRisk)
    {
      Assert.Equal(expectedRisk, DepthPlot.BasinRiskRule(basinSizes));
    }

    [Theory]
    [MemberData(nameof(WellFormedDepthReadings))]
    void FindBasinAggregateRisk_DoesNotThrowException(IEnumerable<string> goodReadings)
    {
      var sut = new DepthPlot(goodReadings);
      var exception = Record.Exception(
          () => _ = sut.FindBasinAggregateRisk()
        );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(DepthsWithBasinAggregateRisk))]
    void FindBasinAggregateRisk_ReturnsExpectedRisk(IEnumerable<string> readings, int expectedRisk)
    {
      var sut = new DepthPlot(readings);
      Assert.Equal(expectedRisk, sut.FindBasinAggregateRisk());
    }

    [Theory]
    [InlineData(0, 5, 1)]
    [InlineData(1, 6, 1)]
    [InlineData(0, 6, 3)]
    [InlineData(0, 7, 4)]
    [InlineData(1, 8, 1)]
    [InlineData(0, 8, 6)]
    [InlineData(1, 9, 3)] // In the broader context, this would be 2 points not 3 but since we're starting there we get what we get
    [InlineData(2, 9, 1)]
    [InlineData(0, 9, 9)]
    void ScorePoint_ReturnsExpectedScore(int row, int column, int expectedScore)
    {
      var depths = new[] {
        "2199943210",
        "3987894921",
        "9856789892",
        "8767896789",
        "9899965678"
      };

      var sut = new DepthPlot(depths);

      Assert.Equal(expectedScore, sut.ScorePoint(row, column));
    }

    public static IEnumerable<object[]> MalformedDepthReadings
    {
      get
      {
        yield return new object[] {
          null
        };

        yield return new object[]
        {
          new List<string>()
        };

        yield return new object[]
        {
          new List<string>
          {
            "1",
            "12"
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "1",
            null
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "1",
            "a"
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "1",
            "\n"
          }
        };
      }
    }

    public static IEnumerable<object[]> WellFormedDepthReadings
    {
      get
      {
       yield return new object[]
        {
          new List<string>
          {
            "1",
            "2"
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678"
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921"
          }
        };
      }
    }

    public static IEnumerable<object[]> DepthsWithMinimums
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "1",
            "2"
          },
          new [] {
            1
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678"
          },
          new[] {
            1,
            0,
            5,
            5
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921"
          },
          new[]
          {
            1,
            0,
            7
          }
        };
      }
    }

    public static IEnumerable<object[]> DepthsWithBasicRiskLevels
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "1",
            "2"
          },
          new [] {
            2
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678"
          },
          new[] {
            2,
            1,
            6,
            6
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921"
          },
          new[]
          {
            2,
            1,
            8
          }
        };
      }
    }

    public static IEnumerable<object[]> DepthsWithBasinSizes
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "1",
            "2"
          },
          new [] {
            2
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678"
          },
          new[] {
            3,
            9,
            14,
            9
          }
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921"
          },
          new[]
          {
            3,
            8,
            3
          }
        };
      }
    }

    public static IEnumerable<object[]> BasinSizesWithAggregateRisk
    {
      get
      {
        yield return new object[]
        {
          new [] {
            2
          },
          2
        };

        yield return new object[]
        {
          new[] {
            3,
            9,
            14,
            9
          },
          1134
        };

        yield return new object[]
        {
          new[]
          {
            3,
            8,
            3
          },
          72
        };
      }
    }

    public static IEnumerable<object[]> DepthsWithBasinAggregateRisk
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "1",
            "2"
          },
          2
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678"
          },
          1134
        };

        yield return new object[]
        {
          new List<string>
          {
            "2199943210",
            "3987894921"
          },
          72
        };
      }
    }
  }
}
