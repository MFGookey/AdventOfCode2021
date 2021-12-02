using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DepthTracker.Core.Tests
{
  public class DepthAnalyzerTests
  {
    [Fact]
    public void DepthAnalyzer_GivenEmptyLog_CountsZeroIncreases()
    {
      var sut = new DepthAnalyzer(new int[] { });
      Assert.Equal(0, sut.CountIncreases());
    }

    [Fact]
    public void DepthAnalyzer_GivenLogWithNoIncreases_CountsZeroIncreases()
    {
      var sut = new DepthAnalyzer(new int[] { 5, 4, 3, 2, 1 });
      Assert.Equal(0, sut.CountIncreases());
    }

    [Fact]
    public void DepthAnalyzer_GivenLogWithIncreases_CountsFourIncreases()
    {
      var sut = new DepthAnalyzer(new int[] { 10, 20, 30, 40, 50 });
      Assert.Equal(4, sut.CountIncreases());
    }

    [Fact]
    public void DepthAnalyzer_GivenSampleData_CountsCorrectIncreases()
    {
      var sut = new DepthAnalyzer(new int[] {
        199,
        200,
        208,
        210,
        200,
        207,
        240,
        269,
        260,
        263
      });

      Assert.Equal(7, sut.CountIncreases());
    }

    [Fact]
    public void DepthAnalyzer_GivenSampleWindowData_CountsCorrectIncreases()
    {
      var sut = new DepthAnalyzer(new int[] {
        199,
        200,
        208,
        210,
        200,
        207,
        240,
        269,
        260,
        263
      });

      Assert.Equal(5, sut.CountIncreases(3));
    }
  }
}
