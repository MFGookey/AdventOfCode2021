using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace Twinklepus.Core.Tests
{
  public class OctopusTests
  {
    [Theory]
    [InlineData(-1)]
    [InlineData(10)]
    public void Octopus_GivenInvalidInitialEnergy_ThrowsArgumentOutOfRangeException(int initialEnergy)
    {
      Assert.Throws<ArgumentOutOfRangeException>(
        () => _ = new Octopus(initialEnergy)
      );
    }

    [Theory]
    [MemberData(nameof(ValidInitialEnergies))]
    public void Octopus_GivenValidInitialEnergy_DoesNotThrowException(int initialEnergy)
    {
      var exception = Record.Exception(
        () => _ = new Octopus(initialEnergy)
      );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(ValidInitialEnergies))]
    public void Octopus_GivenValidInitialEnergy_SetsPropertiesAsExpected(int initialEnergy)
    {
      var sut = new Octopus(initialEnergy);
      Assert.Equal(initialEnergy, sut.Energy);
      Assert.Equal(initialEnergy.ToString(), sut.ToString());
      Assert.False(sut.FlashedThisTick);
    }

    [Theory]
    [MemberData(nameof(ValidInitialEnergies))]
    public void Tick_CalledEnoughTimes_SetsPropertiesAsExpected(int initialEnergy)
    {
      var sut = new Octopus(initialEnergy);
      var trackEnergy = initialEnergy;
      
      // Tick right up to the threshold of a flash
      for (var i = 9; i > initialEnergy; i--)
      {
        sut.Tick();
        trackEnergy++;
        Assert.False(sut.FlashedThisTick);
        Assert.Equal(trackEnergy, sut.Energy);
        Assert.Equal(trackEnergy.ToString(), sut.ToString());
      }

      // The first iteration should cause a flash
      // Every subsequent iteration should do effectively nothing
      for (var i = 0; i < 10; i++)
      {
        sut.Tick();
        Assert.True(sut.FlashedThisTick);
        Assert.Equal(0, sut.Energy);
        Assert.Equal(0.ToString(), sut.ToString());
      }
    }

    [Theory]
    [MemberData(nameof(ValidInitialEnergies))]
    public void TickComplete_CalledAfterTick_SetsPropertiesAsExpected(int initialEnergy)
    {
      var sut = new Octopus(initialEnergy);

      // Tick right up to the threshold of a flash
      for (var i = 9; i > initialEnergy; i--)
      {
        sut.Tick();
      }

      Assert.False(sut.FlashedThisTick);
      Assert.Equal(9, sut.Energy);
      Assert.Equal(9.ToString(), sut.ToString());

      sut.CompleteTick();

      Assert.False(sut.FlashedThisTick);
      Assert.Equal(9, sut.Energy);
      Assert.Equal(9.ToString(), sut.ToString());

      for (var i = 0; i < 10; i++)
      {
        sut.Tick();
        Assert.True(sut.FlashedThisTick);
        Assert.Equal(0, sut.Energy);
        Assert.Equal(0.ToString(), sut.ToString());
      }

      sut.CompleteTick();
      Assert.False(sut.FlashedThisTick);
      Assert.Equal(0, sut.Energy);
      Assert.Equal(0.ToString(), sut.ToString());
    }

    [Fact]
    public void Tick_WhenCausesFlash_TicksNeighbors()
    {
      var sut = new Octopus(9);
      Assert.False(sut.FlashedThisTick);
      Assert.Equal(9, sut.Energy);

      var lowNeighbor = new Octopus(0);
      var highNeighbor = new Octopus(9);

      sut.SetNeighbors(new List<Octopus> { lowNeighbor, highNeighbor });

      sut.Tick();

      Assert.True(sut.FlashedThisTick);
      Assert.Equal(0, sut.Energy);

      Assert.Equal(1, lowNeighbor.Energy);
      Assert.False(lowNeighbor.FlashedThisTick);

      Assert.Equal(0, highNeighbor.Energy);
      Assert.True(highNeighbor.FlashedThisTick);
    }

    public static IEnumerable<object[]> ValidInitialEnergies
    {
      get
      {
        return Enumerable
          .Range(0, 10)
          .Select(v => new object[] { v });
      }
    }
  }
}
