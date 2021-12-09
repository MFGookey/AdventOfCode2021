using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SmokeInTheWater.Core.Tests
{
  public class PointValueTests
  {
    [Fact]
    void PointValue_WhenConstructed_SetsValuesCorrectly()
    {
      var row = 42;
      var column = 23;
      var value = 1337;
      var sut = new PointValue(row, column, value);
      Assert.Equal(row, sut.Row);
      Assert.Equal(column, sut.Column);
      Assert.Equal(value, sut.Value);
    }
  }
}
