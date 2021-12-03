using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeneralElectric.Core.Tests
{
  public class ConsumptionCalculatorTests
  {
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

    void ConsumptionCalculator_GivenReadings_CalculatesCorrectly(
      IEnumerable<IConsumptionReading> readings,
      int expectedGamma,
      int expectedEpsilon
      )
    {
      var sut = new ConsumptionCalculator(readings);

      Assert.Equal(expectedGamma, sut.CalculateGamma());
      Assert.Equal(expectedEpsilon, sut.CalculateEpsilon());
    }

    public static IEnumerable<object[]> Readings
    {
      get
      {
        yield return new object[]
        {
          new[] {new ConsumptionReading("0")},
          0,
          1
        };

        yield return new object[]
        {
          new[] {new ConsumptionReading("1")},
          1,
          0
        };

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
          22,
          9
        };
      }
    }
  }
}
