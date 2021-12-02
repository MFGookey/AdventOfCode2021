using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SubPlot.Core.Tests
{
  public class NavigationStepTests
  {
    [Theory]
    [InlineData("Forward 0", NavigationDirections.Forward, 0)]
    [InlineData("forward 10", NavigationDirections.Forward, 10)]
    [InlineData("Up 50", NavigationDirections.Up, 50)]
    [InlineData("Down 231", NavigationDirections.Down, 231)]
    [InlineData("up -10", NavigationDirections.Up, -10)]
    [InlineData("down -5", NavigationDirections.Down, -5)]
    void NavigationStep_GivenValidCommands_ParsesCorrectly(
      string command,
      NavigationDirections expectedDirection,
      int expectedMagnitude
    )
    {
      var sut = new NavigationStep(command);
      Assert.Equal(expectedDirection, sut.Direction);
      Assert.Equal(expectedMagnitude, sut.Magnitude);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Back 1")]
    [InlineData("Forward")]
    [InlineData("Up")]
    [InlineData("Down")]
    [InlineData("Left 10")]
    [InlineData("Right 10")]
    [InlineData("10 Up")]
    [InlineData("1 1")]

    void NavigationStep_GivenInvalidCommands_ThrowsFormatException(string command)
    {
      Assert.Throws<FormatException>(
        () => { 
          new NavigationStep(command);
        }
      );
    }

    [Theory]
    [InlineData("Up 10", 0, -10)]
    [InlineData("Forward 22", 22, 0)]
    [InlineData("Down 5", 0, 5)]
    [InlineData("Forward -2", -2, 0)]
    [InlineData("Down -76", 0, -76)]
    [InlineData("Up -835", 0, 835)]
    void NavigationStep_GivenValidCommand_CalculatesChangeCorrectly(string command, int expectedHorizontalChange, int expectedDepthChange)
    {
      var sut = new NavigationStep(command);
      Assert.Equal(expectedHorizontalChange, sut.ChangeInHorizontal());
      Assert.Equal(expectedDepthChange, sut.ChangeInDepth());
    }
  }
}
