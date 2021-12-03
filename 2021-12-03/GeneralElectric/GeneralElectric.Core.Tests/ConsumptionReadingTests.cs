using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeneralElectric.Core.Tests
{
  public class ConsumptionReadingTests
  {
    [Fact]
    void ConsumptionReading_GivenEmptyString_HasEmptyReadings()
    {
      var sut = new ConsumptionReading("");
      Assert.Empty(sut.Readings);
    }

    [Theory]
    [InlineData("0", new[] { false })]
    [InlineData("1", new[] { true })]
    [InlineData("10", new[] { true, false })]
    [InlineData("01", new[] { false, true })]
    [InlineData("11", new[] { true, true })]
    [InlineData("00", new[] { false, false })]
    [InlineData("00100", new[] { false, false, true, false, false })]
    [InlineData("11110", new[] { true, true, true, true, false })]
    [InlineData("10110", new[] { true, false, true, true, false })]
    [InlineData("10111", new[] { true, false, true, true, true })]
    [InlineData("10101", new[] { true, false, true, false, true })]
    [InlineData("01111", new[] { false, true, true, true, true })]
    [InlineData("00111", new[] { false, false, true, true, true })]
    [InlineData("11100", new[] { true, true, true, false, false })]
    [InlineData("10000", new[] { true, false, false, false, false })]
    [InlineData("11001", new[] { true, true, false, false, true })]
    [InlineData("00010", new[] { false, false, false, true, false })]
    [InlineData("01010", new[] { false, true, false, true, false })]
    void ConsumptionReading_GivenValues_ParsesCorrectly(
      string rawReading,
      IEnumerable<bool> expectedReadings
    )
    {
      var sut = new ConsumptionReading(rawReading);
      Assert.Equal(expectedReadings, sut.Readings);
    }
  }
}
