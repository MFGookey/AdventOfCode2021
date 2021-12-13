using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace IRigami.Core.Tests
{
  public class FoldableSheetTests
  {
    [Fact]
    public void FoldableSheet_GivenNullPoints_ThrowsArgumentNullException()
    {
      var folds = new List<string>();
      Assert.Throws<ArgumentNullException>(
        () => _ = new FoldableSheet(null, folds)
      );
    }

    [Fact]
    public void FoldableSheet_GivenNullPoint_ThrowsArgumentNullException()
    {
      var folds = new List<string>();
      var points = new List<string>
      {
        "1,1",
        "2,2",
        null,
        "4,4"
      };

      Assert.Throws<ArgumentNullException>(
        () => _ = new FoldableSheet(points, folds)
      );
    }

    [Fact]
    public void FoldableSheet_GivenNullFolds_ThrowsArgumentNullException()
    {
      var points = new List<string>();
      Assert.Throws<ArgumentNullException>(
        () => _ = new FoldableSheet(points, null)
      );
    }

    [Fact]
    public void FoldableSheet_GivenNullFold_ThrowsArgumentNullException()
    {
      var folds = new List<string>
      {
        "fold along y=7",
        null,
        "fold along x=5"
      };

      var points = new List<string>
      {
        "1,1",
        "2,2",
        "4,4"
      };

      Assert.Throws<ArgumentNullException>(
        () => _ = new FoldableSheet(points, folds)
      );
    }

    [Fact]
    public void FoldableSheet_GivenInvalidPoint_ThrowsArgumentException()
    {
      var folds = new List<string>
      {
        "fold along y=7",
        "fold along x=5"
      };

      var points = new List<string>
      {
        "1,1",
        "asdf",
        "4,4"
      };

      Assert.Throws<ArgumentException>(
        () => _ = new FoldableSheet(points, folds)
      );
    }



    [Fact]
    public void FoldableSheet_GivenInvalidFold_ThrowsArgumentException()
    {
      var folds = new List<string>
      {
        "fold along y=7",
        "asdf",
        "fold along x=5"
      };

      var points = new List<string>
      {
        "1,1",
        "4,4"
      };

      Assert.Throws<ArgumentException>(
        () => _ = new FoldableSheet(points, folds)
      );
    }

    [Theory]
    [MemberData(nameof(ValidParameters))]

    public void FoldableSheet_GivenValidParameters_DoesNotThrowException(IEnumerable<string> points, IEnumerable<string> folds)
    {
      var exception = Record.Exception(
        () => _ = new FoldableSheet(points, folds)
      );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(ValidParametersWithExpectedPointCount))]

    public void Tick_SetsVisiblePointsAsExpected(
        IEnumerable<string> points,
        IEnumerable<string> folds,
        int timesToTick,
        int expectedPoints
    )
    {
      var sut = new FoldableSheet(points, folds);

      for (var i = 0; i < timesToTick; i++)
      {
        sut.Tick();
      }

      Assert.Equal(expectedPoints, sut.CountVisiblePoints());
    }

    public static IEnumerable<object[]> ValidParameters
    {
      get
      {
        yield return new object[]
        {
          new string[] {
            "6,10",
            "0,14",
            "9,10",
            "0,3",
            "10,4",
            "4,11",
            "6,0",
            "6,12",
            "4,1",
            "0,13",
            "10,12",
            "3,4",
            "3,0",
            "8,4",
            "1,10",
            "2,14",
            "8,10",
            "9,0"
          },
          new string[] {
            "fold along y=7",
            "fold along x=5"
          }
        };
      }
    }

    public static IEnumerable<object[]> ValidParametersWithExpectedPointCount
    {
      get
      {
        yield return new object[]
        {
          new string[] {
            "6,10",
            "0,14",
            "9,10",
            "0,3",
            "10,4",
            "4,11",
            "6,0",
            "6,12",
            "4,1",
            "0,13",
            "10,12",
            "3,4",
            "3,0",
            "8,4",
            "1,10",
            "2,14",
            "8,10",
            "9,0"
          },
          new string[] {
            "fold along y=7",
            "fold along x=5"
          },
          1,
          17
        };
      }
    }
  }
}
