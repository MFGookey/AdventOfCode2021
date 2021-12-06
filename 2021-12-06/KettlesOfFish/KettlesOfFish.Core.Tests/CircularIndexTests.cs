using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KettlesOfFish.Core.Tests
{
  public class CircularIndexTests
  {
    [Fact]
    void CircularIndex_GivenNegativeRadix_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new CircularIndex(-1)
      );
    }

    [Fact]
    void CircularIndex_GivenZeroRadix_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new CircularIndex(0)
      );
    }

    [Fact]
    void CircularIndex_GivenPositiveRadix_DoesNotThrowException()
    {
      var exception = Record.Exception(() => _ = new CircularIndex(1));
      Assert.Null(exception);
    }

    [Fact]
    void CircularIndex_GivenPositiveRadix_SetsRadixCorrectly()
    {
      var radix = 5;
      var sut = new CircularIndex(radix);
      Assert.Equal(radix, sut.Radix);
      Assert.Equal(0, sut.CurrentValue);
    }

    [Fact]
    void CircularIndex_GivenNegativeInitialValue_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new CircularIndex(5, -1)
      );
    }

    [Fact]
    void CircularIndex_GivenInitialValueEqualToRadix_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new CircularIndex(5, 5)
      );
    }

    [Fact]
    void CircularIndex_GivenInitialValueGreaterThanRadix_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new CircularIndex(5, 6)
      );
    }

    void CircularIndex_GivenValidInitialValue_DoesNotThrowException()
    {
      var exception = Record.Exception(
        () => _ = new CircularIndex(5, 1)
      );
      Assert.Null(exception);
    }

    void CircularIndex_GivenValidInitialValue_SetsCurrentValueCorrectly()
    {
      var radix = 5;
      var initialValue = 3;

      var sut = new CircularIndex(radix, initialValue);

      Assert.Equal(radix, sut.Radix);
      Assert.Equal(initialValue, sut.CurrentValue);

    }

    void CircularIndex_GivenValidInitialValue_WhenSteppingToLessThanRadix_GoesUpByOne()
    {
      var radix = 5;
      var initialValue = 3;

      var sut = new CircularIndex(radix, initialValue);

      Assert.Equal(initialValue, sut.CurrentValue);

      sut.Step();

      Assert.Equal((initialValue + 1), sut.CurrentValue);
    }

    void CircularIndex_GivenValidInitialValue_WhenSteppingToRadix_GoesToZero()
    {
      var radix = 7;
      var initialValue = 6;

      var sut = new CircularIndex(radix, initialValue);

      Assert.Equal(initialValue, sut.CurrentValue);
      Assert.Equal(radix, sut.Radix);

      sut.Step();

      Assert.Equal(0, sut.CurrentValue);

      // Checking again just to show Radix doesn't change.
      Assert.Equal(radix, sut.Radix);
    }
  }
}
