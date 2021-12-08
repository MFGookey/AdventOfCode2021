using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace SevenSegmentDismay.Core.Tests
{
  public class DisplayTests
  {
    [Theory]
    [InlineData("h")]
    [InlineData(";")]
    [InlineData("!")]
    [InlineData("asdfaw3")]
    [InlineData("1")]
    public void Display_GivenIncorrectSegments_ThrowsArgumentException(string badSegments)
    {
      Assert.Throws<ArgumentException>(
        () => _ = new Display(badSegments)
      );
    }

    [Fact]
    public void Display_GivenNullSegments_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new Display(null)
      );
    }

    [Fact]
    public void Display_GivenDoubledSegments_ThrowsArgumentException()
    {
      Assert.Throws<ArgumentException>(
        () => _ = new Display("aa")
      );
    }

    [Theory]
    [InlineData("abcefg")]  // Zero
    [InlineData("cf")]      // One
    [InlineData("acdeg")]   // Two
    [InlineData("acdfg")]   // Three
    [InlineData("bcdf")]    // Four
    [InlineData("abdfg")]   // Five
    [InlineData("abdefg")]  // Six
    [InlineData("acf")]     // Seven
    [InlineData("abcdefg")] // Eight
    [InlineData("abcdfg")]  // Nine
    public void Display_GivenLegitimateSegments_DoesNotThrowException(string segments)
    {
      var exception = Record.Exception(() => _ = new Display(segments));
      Assert.Null(exception);
    }

    [Theory]
    [InlineData("abcefg", " aaaa \nb    c\nb    c\n .... \ne    f\ne    f\n gggg ")]  // Zero
    [InlineData("cf", " .... \n.    c\n.    c\n .... \n.    f\n.    f\n .... ")]      // One
    [InlineData("acdeg", " aaaa \n.    c\n.    c\n dddd \ne    .\ne    .\n gggg ")]   // Two
    [InlineData("acdfg", " aaaa \n.    c\n.    c\n dddd \n.    f\n.    f\n gggg ")]   // Three
    [InlineData("bcdf", " .... \nb    c\nb    c\n dddd \n.    f\n.    f\n .... ")]    // Four
    [InlineData("abdfg", " aaaa \nb    .\nb    .\n dddd \n.    f\n.    f\n gggg ")]   // Five
    [InlineData("abdefg", " aaaa \nb    .\nb    .\n dddd \ne    f\ne    f\n gggg ")]  // Six
    [InlineData("acf", " aaaa \n.    c\n.    c\n .... \n.    f\n.    f\n .... ")]     // Seven
    [InlineData("abcdefg", " aaaa \nb    c\nb    c\n dddd \ne    f\ne    f\n gggg ")] // Eight
    [InlineData("abcdfg", " aaaa \nb    c\nb    c\n dddd \n.    f\n.    f\n gggg ")]  // Nine
    public void ToString_ReturnsExpectedString(string segments, string expectedToString)
    {
      var sut = new Display(segments);
      Assert.Equal(expectedToString, sut.ToString());
    }

    [Theory]
    [MemberData(nameof(DisplayByRowsData))]
    public void DisplayByRows_ReturnsExpectedStringList(string segments, IEnumerable<string> expectedRows)
    {
      var sut = new Display(segments);
      Assert.Equal(expectedRows, sut.DisplayByRows());
    }

    [Theory]
    [MemberData(nameof(DisplayByRowsData))]
    public void DisplayByRows_WhenReversed_DoesNotMatchNormalStringList(string segments, IEnumerable<string> expectedRows)
    {
      var sut = new Display(segments);
      Assert.NotEqual(expectedRows, sut.DisplayByRows().Reverse());
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("qqq")]
    public void AppendByRows_GivenInterstitial_ReturnsExpectedStrings(string interstitial)
    {
      var left = Display.FOUR.DisplayByRows().ToList();
      var right = Display.TWO.DisplayByRows().ToList();
      var appended = new List<string>();
      var sb = new StringBuilder();
      for (var i = 0; i < left.Count(); i++)
      {
        sb.Clear();
        sb.Append(left[i]);
        sb.Append(interstitial);
        sb.Append(right[i]);
        appended.Add(sb.ToString());
      }

      var leftDisplayRows = new Display("cbfd").DisplayByRows(); // Equivalent to FOUR
      var sut = new Display("egcad"); // Equivalent to TWO
      Assert.Equal(appended, sut.AppendByRows(leftDisplayRows, interstitial));
    }



    [Fact]
    public void AppendByRows_ReturnsExpectedStrings()
    {
      var left = Display.TWO.DisplayByRows().ToList();
      var right = Display.THREE.DisplayByRows().ToList();
      var appended = new List<string>();
      var sb = new StringBuilder();
      for (var i = 0; i < left.Count(); i++)
      {
        sb.Clear();
        sb.Append(left[i]);
        sb.Append(" ");
        sb.Append(right[i]);
        appended.Add(sb.ToString());
      }

      var leftDisplayRows = new Display("adgec").DisplayByRows(); // Equivalent to TWO
      var sut = new Display("dgacf"); // Equivalent to THREE
      Assert.Equal(appended, sut.AppendByRows(leftDisplayRows));
    }

    [Fact]
    public void Equals_GivenEquivalentDisplays_ReturnsTrue()
    {
      var sut = new Display("badfeg"); // Equivalent to SIX
      Assert.True(sut.Equals(Display.SIX));
    }

    [Fact]
    public void Equals_GivenDifferentDisplays_ReturnsFalse()
    {
      var sut = new Display("fc"); // Equivalent to ONE
      Assert.False(sut.Equals(Display.FOUR));
    }

    public static IEnumerable<object[]> DisplayByRowsData
    {
      get
      {
        yield return new object[]
        {
          // Zero
          "abcefg",
          new [] {
            " aaaa ",
            "b    c",
            "b    c",
            " .... ",
            "e    f",
            "e    f",
            " gggg "
          }
        };

        yield return new object[]
        {
          // One
          "cf",
          new [] {
            " .... ",
            ".    c",
            ".    c",
            " .... ",
            ".    f",
            ".    f",
            " .... "
          }
        };

        yield return new object[]
        {
          // Two
          "acdeg",
          new [] {
            " aaaa ",
            ".    c",
            ".    c",
            " dddd ",
            "e    .",
            "e    .",
            " gggg "
          }
        };

        yield return new object[]
        {
          // Three
          "acdfg",
          new [] {
            " aaaa ",
            ".    c",
            ".    c",
            " dddd ",
            ".    f",
            ".    f",
            " gggg "
          }
        };

        yield return new object[]
        {
          // Four
          "bcdf",
          new [] {
            " .... ",
            "b    c",
            "b    c",
            " dddd ",
            ".    f",
            ".    f",
            " .... "
          }
        };

        yield return new object[]
        {
          // Five
          "abdfg",
          new [] {
            " aaaa ",
            "b    .",
            "b    .",
            " dddd ",
            ".    f",
            ".    f",
            " gggg "
          }
        };

        yield return new object[]
        {
          // Six
          "abdefg",
          new [] {
            " aaaa ",
            "b    .",
            "b    .",
            " dddd ",
            "e    f",
            "e    f",
            " gggg "
          }
        };

        yield return new object[]
        {
          // Seven
          "acf",
          new [] {
            " aaaa ",
            ".    c",
            ".    c",
            " .... ",
            ".    f",
            ".    f",
            " .... "
          }
        };

        yield return new object[]
        {
          // Eight
          "abcdefg",
          new [] {
            " aaaa ",
            "b    c",
            "b    c",
            " dddd ",
            "e    f",
            "e    f",
            " gggg "
          }
        };

        yield return new object[]
        {
          // Nine
          "abcdfg",
          new [] {
            " aaaa ",
            "b    c",
            "b    c",
            " dddd ",
            ".    f",
            ".    f",
            " gggg "
          }
        };
      }
    }
  }
}
