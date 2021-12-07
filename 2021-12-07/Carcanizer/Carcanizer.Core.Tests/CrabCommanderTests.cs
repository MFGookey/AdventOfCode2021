using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Carcanizer.Core.Tests
{
  public class CrabCommanderTests
  {
    [Fact]
    void CrabCommander_GivenNonNumericCrabs_ThrowsException()
    {
      var badCrabs = new[] { "1", "2", "3X" };
      Assert.Throws<ArgumentException>(() => _ = new CrabCommander(badCrabs));
    }

    [Fact]
    void CrabCommander_GivenSingleton_WhenCalculatingCost_ReturnsSingleton()
    {
      var sut = new CrabCommander(new[] { "42" });
      Assert.Equal(0, sut.CalculateCrabsOfTheLinePositionCost());
    }



    [Fact]
    void CrabCommander_GivenKnownList_WhenCalculatingCost_ReturnsExpectedValue()
    {
      var demoList = new[] {
        "16",
        "1",
        "2",
        "0",
        "4",
        "2",
        "7",
        "1",
        "2",
        "14"
      };

      var sut = new CrabCommander(demoList);
      Assert.Equal(37, sut.CalculateCrabsOfTheLinePositionCost());
    }
  }
}
