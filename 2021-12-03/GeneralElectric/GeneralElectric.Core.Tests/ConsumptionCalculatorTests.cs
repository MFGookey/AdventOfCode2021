using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeneralElectric.Core.Tests
{
  public class ConsumptionCalculatorTests
  {
    private static readonly int GammaIndex = 0;
    private static readonly int EpsilonIndex = 1;
    private static readonly int OxygenRatingIndex = 2;
    private static readonly int ScrubberRatingIndex = 3;

    [Fact]
    void ConsumptionCalculator_GivenNonRectangularReadings_ThrowsArgumentException()
    {
      var readings = new[]
      {
        new ConsumptionReading(""),
        new ConsumptionReading("0")
      };

      Assert.Throws<ArgumentException>(() => new ConsumptionCalculator(readings));
    }

    [Theory]
    [MemberData(nameof(Readings))]

    void ConsumptionCalculator_GivenReadings_CalculatesGammaCorrectly(
      IEnumerable<IConsumptionReading> readings,
      int[] expectedValues
      )
    {
      var sut = new ConsumptionCalculator(readings);
      Assert.Equal(expectedValues[GammaIndex], sut.CalculateGamma());
    }

    [Theory]
    [MemberData(nameof(Readings))]
    void ConsumptionCalculator_GivenReadings_CalculatesEpsilonCorrectly(
      IEnumerable<IConsumptionReading> readings,
      int[] expectedValues
      )
    {
      var sut = new ConsumptionCalculator(readings);

      Assert.Equal(expectedValues[EpsilonIndex], sut.CalculateEpsilon());
    }

    [Theory]
    [MemberData(nameof(Readings))]
    void ConsumptionCalculator_GivenReadings_CalculatesOxygenGeneratorRatingCorrectly(
      IEnumerable<IConsumptionReading> readings,
      int[] expectedValues
      )
    {
      var sut = new ConsumptionCalculator(readings);

      Assert.Equal(
        expectedValues[OxygenRatingIndex],
        sut.CalculateOxygenGeneratorRating()
      );
    }

    [Theory]
    [MemberData(nameof(Readings))]
    void ConsumptionCalculator_GivenReadings_CalculatesCO2ScrubberRatingCorrectly(
      IEnumerable<IConsumptionReading> readings,
      int[] expectedValues
      )
    {
      var sut = new ConsumptionCalculator(readings);

      Assert.Equal(
        expectedValues[ScrubberRatingIndex],
        sut.CalculateCO2ScrubberRating()
      );
    }


    public static IEnumerable<object[]> Readings
    {
      get
      {
        /*yield return new object[]
        {
          new[] {new ConsumptionReading("0")},
          new[] {
            0,
            1,
            0,
            1
          }
        };

        yield return new object[]
        {
          new[] {new ConsumptionReading("1")},
          new[] {
            1,
            0,
            1,
            0
          }
        };*/

        yield return new object[]
        {
          new[] {
            new ConsumptionReading("00100"),
            new ConsumptionReading("11110"),
            new ConsumptionReading("10110"),
            new ConsumptionReading("10111"),
            new ConsumptionReading("10101"),
            new ConsumptionReading("01111"),
            new ConsumptionReading("00111"),
            new ConsumptionReading("11100"),
            new ConsumptionReading("10000"),
            new ConsumptionReading("11001"),
            new ConsumptionReading("00010"),
            new ConsumptionReading("01010")
          },
          new[] {
            22,
            9,
            23,
            10
          }
        };
      }
    }
  }
}
