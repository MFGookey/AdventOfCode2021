using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Common.Utilities.TwoD;

namespace Common.Utilities.Tests.TwoD
{
  public class PointTests
  {
    [Fact]
    void Point_GivenValidCoordinates_SetsRowCorrectly()
    {
      var sut = new Point(12, 6346);
      Assert.Equal(12, sut.Row);
      Assert.Equal(12, sut.Y);
    }

    [Fact]
    void Point_GivenValidCoordinates_SetsColumnCorrectly()
    {
      var sut = new Point(-38, 894);
      Assert.Equal(894, sut.Column);
      Assert.Equal(894, sut.X);
    }

    [Theory]
    [MemberData(nameof(PointComparisonData))]
    void Point_WhenComparingAlignments_ReturnsExpectedAlignmentValue(
      int row,
      int column,
      Point other,
      bool expectedManhattanAlignment,
      bool expectedDiagonalAlignment
    )
    {
      var sut = new Point(row, column);
      Assert.Equal(expectedManhattanAlignment, sut.IsManhattanAligned(other));
      Assert.Equal(expectedDiagonalAlignment, sut.IsDiagonallyAligned(other));
    }

    [Theory]
    [MemberData(nameof(PointEqualityData))]
    void Point_WhenComparedToAnotherPoint_ReturnsExpectedEquality(
      int row,
      int column,
      Point other,
      bool expectedEquality
    )
    {
      var sut = new Point(row, column);
      Assert.Equal(expectedEquality, sut.Equals(other));

      Assert.Equal(expectedEquality, sut.Equals((object)other));

      if (other != null)
      {
        Assert.Equal(expectedEquality, other.Equals(sut));
        Assert.Equal(expectedEquality, other.Equals((object)sut));
        Assert.Equal(expectedEquality, sut.GetHashCode() == other.GetHashCode());
      }
    }

    [Theory]
    [MemberData(nameof(PointToStringData))]
    void Point_WhenToStringIsCalled_ReturnsExpectedString(int row, int column, string expectedToString)
    {
      var sut = new Point(row, column);
      Assert.Equal(expectedToString, sut.ToString());
    }

    [Theory]
    [MemberData(nameof(PointToXYStringData))]
    void Point_WhenToStringXYIsCalled_ReturnsExpectedString(int X, int Y, string expectedToString)
    {
      var sut = new Point(Y, X);
      Assert.Equal(expectedToString, sut.ToXYString());
    }

    public static IEnumerable<object[]> PointComparisonData
    {
      get
      {
        yield return new object[]
        {
          10,
          10,
          new Point(10, 10),
          true,
          true
        };

        yield return new object[]
        {
          2,
          6,
          new Point(2, 235423),
          true,
          false
        };

        yield return new object[]
        {
          98,
          23,
          new Point(0, 23),
          true,
          false
        };

        yield return new object[]
        {
          -10,
          10,
          new Point(10, 235423),
          false,
          false
        };

        yield return new object[]
        {
          1,
          1,
          new Point(3, 3),
          false,
          true
        };

        yield return new object[]
        {
          9,
          7,
          new Point(7, 9),
          false,
          true
        };
      }
    }

    public static IEnumerable<object[]> PointEqualityData
    {
      get
      {
        yield return new object[]
        {
          10,
          20,
          new Point(10, 20),
          true
        };

        yield return new object[]
        {
          -10,
          20,
          new Point(10, 20),
          false
        };

        yield return new object[]
        {
          10,
          -20,
          new Point(10, 20),
          false
        };

        yield return new object[]
        {
          10,
          20,
          null,
          false
        };
      }
    }

    public static IEnumerable<object[]> PointToStringData
    {
      get
      {
        yield return new object[]
        {
          10,
          10,
          "10,10"
        };

        yield return new object[]
        {
          2,
          6,
          "2,6"
        };

        yield return new object[]
        {
          98,
          23,
          "98,23"
        };

        yield return new object[]
        {
          -10,
          10,
          "-10,10"
        };

        yield return new object[]
        {
          2,
          235423,
          "2,235423"
        };

        yield return new object[]
        {
          0,
          23,
          "0,23"
        };

        yield return new object[]
        {
          10,
          235423,
          "10,235423"
        };
      }
    }

    public static IEnumerable<object[]> PointToXYStringData
    {
      get
      {
        yield return new object[]
        {
          10,
          10,
          "10,10"
        };

        yield return new object[]
        {
          2,
          6,
          "2,6"
        };

        yield return new object[]
        {
          98,
          23,
          "98,23"
        };

        yield return new object[]
        {
          -10,
          10,
          "-10,10"
        };

        yield return new object[]
        {
          2,
          235423,
          "2,235423"
        };

        yield return new object[]
        {
          0,
          23,
          "0,23"
        };

        yield return new object[]
        {
          10,
          235423,
          "10,235423"
        };
      }
    }
  }
}
