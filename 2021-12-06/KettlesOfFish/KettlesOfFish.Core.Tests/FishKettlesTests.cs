using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace KettlesOfFish.Core.Tests
{
  public class FishKettlesTests
  {
    [Fact]
    void FishKettles_GivenNullFishEnumerable_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new FishKettles(null, 1, 1)
      );
    }

    [Fact]
    void FishKettles_GivenNonNumericValuesInFishEnumerable_ThrowsException()
    {
      var initialState = new[]
      {
        "1",
        "2",
        "fds"
      };

      Assert.Throws<ArgumentException>(
        () => _ = new FishKettles(null, 1, 1)
      );
    }

    [Fact]
    void FishKettles_GivenZeroAdultInterval_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new FishKettles(new string[] { }, 0, 1)
      );
    }

    [Fact]
    void FishKettles_GivenNegativeAdultInterval_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new FishKettles(InitialState, -1, 1)
      );
    }

    [Fact]
    void FishKettles_GivenNegativeChildInterval_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new FishKettles(InitialState, 7, -1)
      );
    }

    [Fact]
    void FishKettles_GivenPopulationWithLargerValuesThanAdultInterval_ThrowsException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new FishKettles(InitialState, 2, 2)
      );
    }

    [Fact]
    void FishKettles_GivenValidConstructorValues_CalculatesPopulationCorrectlyEachDay()
    {
      var sut = new FishKettles(InitialState, 7, 2);
      for (var i = 0; i < PopulationByDay.Length; i++)
      {
        Assert.Equal(PopulationByDay[i], sut.CurrentPopulation);
        sut.Tick();
      }
    }

    [Fact]
    void Tick_GivenZeroTicks_DoesNotChangePopulationSize()
    {
      var adultInterval = 7;
      var childInterval = 2;
      var sut = new FishKettles(InitialState, adultInterval, childInterval);
      var initialPopulation = sut.CurrentPopulation;
      
      // Assert we HAVE a population
      Assert.True(sut.CurrentPopulation > 0);

      for (var i = 0; i < 2 * (adultInterval + childInterval); i++)
      {
        sut.Tick(0);
      }

      Assert.Equal(initialPopulation, sut.CurrentPopulation);

      // Prove we can grow the population as expected after ticking zeroes
      for (var i = 0; i < PopulationByDay.Length; i++)
      {
        Assert.Equal(PopulationByDay[i], sut.CurrentPopulation);
        sut.Tick();
      }

      // Assert the population really did grow.
      Assert.True(initialPopulation < sut.CurrentPopulation);
    }

    [Fact]
    void Tick_GivenNegativeTicks_ThrowsException()
    {
      var adultInterval = 7;
      var childInterval = 2;
      var sut = new FishKettles(InitialState, adultInterval, childInterval);

      Assert.Throws<ArgumentException>(
        () => sut.Tick(-1)
      );
    }

    [Fact]
    void Tick_GivenLargeTicks_CalculatesCorrectPopulation()
    {
      var adultInterval = 7;
      var childInterval = 2;
      var sut = new FishKettles(InitialState, adultInterval, childInterval);
      sut.Tick(256);
      Assert.Equal(26984457539, sut.CurrentPopulation);
    }

    private static IEnumerable<string> InitialState = new[]
    {
      "3",
      "4",
      "3",
      "1",
      "2"
    };

    // Calculated by hand from the problem's sample data
    /*
        Initial state: 3,4,3,1,2
        After  1 day:  2,3,2,0,1
        After  2 days: 1,2,1,6,0,8
        After  3 days: 0,1,0,5,6,7,8
        After  4 days: 6,0,6,4,5,6,7,8,8
        After  5 days: 5,6,5,3,4,5,6,7,7,8
        After  6 days: 4,5,4,2,3,4,5,6,6,7
        After  7 days: 3,4,3,1,2,3,4,5,5,6
        After  8 days: 2,3,2,0,1,2,3,4,4,5
        After  9 days: 1,2,1,6,0,1,2,3,3,4,8
        After 10 days: 0,1,0,5,6,0,1,2,2,3,7,8
        After 11 days: 6,0,6,4,5,6,0,1,1,2,6,7,8,8,8
        After 12 days: 5,6,5,3,4,5,6,0,0,1,5,6,7,7,7,8,8
        After 13 days: 4,5,4,2,3,4,5,6,6,0,4,5,6,6,6,7,7,8,8
        After 14 days: 3,4,3,1,2,3,4,5,5,6,3,4,5,5,5,6,6,7,7,8
        After 15 days: 2,3,2,0,1,2,3,4,4,5,2,3,4,4,4,5,5,6,6,7
        After 16 days: 1,2,1,6,0,1,2,3,3,4,1,2,3,3,3,4,4,5,5,6,8
        After 17 days: 0,1,0,5,6,0,1,2,2,3,0,1,2,2,2,3,3,4,4,5,7,8
        After 18 days: 6,0,6,4,5,6,0,1,1,2,6,0,1,1,1,2,2,3,3,4,6,7,8,8,8,8
    */
    private static int[] PopulationByDay = new[]
    {
      5,
      5,
      6,
      7,
      9,
      10,
      10,
      10,
      10,
      11,
      12,
      15,
      17,
      19,
      20,
      20,
      21,
      22,
      26
    };
  }
}
