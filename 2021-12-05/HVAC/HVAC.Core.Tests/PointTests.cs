using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HVAC.Core.Tests
{
  public class PointTests
  {
    [Fact]
    void Point_GivenValidCoordinates_SetsXCorrectly()
    {
      var sut = new Point(12, 6346);
      Assert.Equal(12, sut.X);
    }

    [Fact]
    void Point_GivenValidCoordinates_SetsYCorrectly()
    {
      var sut = new Point(-38, 894);
      Assert.Equal(894, sut.Y);
    }

    [Theory]
    [MemberData(nameof(PointComparisonData))]
    void Point_WhenComparingManhattanAlignment_ReturnsExpectedAlignmentValue(int x, int y, Point other, bool expectedAlignment)
    {
      var sut = new Point(x, y);
      Assert.Equal(expectedAlignment, sut.IsManhattanAligned(other));
    }

    [Theory]
    [MemberData(nameof(PointEqualityData))]
    void Point_WhenComparedToAnotherPoint_ReturnsExpectedEquality(int x, int y, Point other, bool expectedEquality)
    {
      var sut = new Point(x, y);
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
    void Point_WhenToStringIsCalled_ReturnsExpectedString(int x, int y, string expectedToString)
    {
      var sut = new Point(x, y);
      Assert.Equal(expectedToString, sut.ToString());
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
          true
        };

        yield return new object[]
        {
          2,
          6,
          new Point(2, 235423),
          true
        };

        yield return new object[]
        {
          98,
          23,
          new Point(0, 23),
          true
        };

        yield return new object[]
        {
          -10,
          10,
          new Point(10, 235423),
          false
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
  }
}
