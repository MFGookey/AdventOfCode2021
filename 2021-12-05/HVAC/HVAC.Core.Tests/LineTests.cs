using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace HVAC.Core.Tests
{
  public class LineTests
  {
    [Fact]
    void Line_GivenValidDefinition_DoesNotThrowException()
    {
      var exception = Record.Exception(() => _ = new Line("1,1 -> 2,3"));
      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(BadLineDefinitions))]
    void Line_GivenInvalidDefinition_ThrowsException(string definition)
    {
      Assert.Throws<ArgumentException>(() => _ = new Line(definition));
    }

    [Fact]
    void Line_GivenValidDefinition_ParsesDefinitionAsExpected()
    {
      var sut = new Line("-1,523 -> 2342,12351\n");
      Assert.Equal(-1, sut.Start.X);
      Assert.Equal(523, sut.Start.Y);
      Assert.Equal(2342, sut.End.X);
      Assert.Equal(12351, sut.End.Y);
    }

    [Theory]
    [MemberData(nameof(GoodLineDefinitions))]
    void Line_GivenValidDefinition_IsManhattanLineAsExpected(string definition, bool expectedManhattanLine)
    {
      var sut = new Line(definition);
      Assert.Equal(expectedManhattanLine, sut.IsLineWeCareAbout((start, end) => start.IsManhattanAligned(end)));
    }

    [Theory]
    [MemberData(nameof(LinesAndBoundingBoxes))]
    void Line_GivenValidDefinition_ReportsBoundingCornersCorrectly(string definition, int expectedLowX, int expectedHighX, int expectedLowY, int expectedHighY)
    {
      var sut = new Line(definition);
      Assert.Equal(expectedLowX, sut.LowX());
      Assert.Equal(expectedLowY, sut.LowY());
      Assert.Equal(expectedHighX, sut.HighX());
      Assert.Equal(expectedHighY, sut.HighY());
      Assert.True(sut.LowX() <= sut.HighX());
      Assert.True(sut.LowY() <= sut.HighY());
    }

    [Theory]
    [MemberData(nameof(ManhattanLineDefinitions))]
    void Line_GivenManhattanLineDefinition_WhenGeneratingLinePoints_DoesNotThrowExceptions(string definition)
    {
      var sut = new Line(definition);
      var exception = Record.Exception(() => sut.GenerateLinePoints((start, end) => start.IsManhattanAligned(end) || start.IsDiagonallyAligned(end)));
      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(DontCareLineDefinitions))]
    void Line_GivenNonManhattanLineDefinition_WhenGeneratingLinePoints_ThrowsExceptions(string definition)
    {
      var sut = new Line(definition);
      Assert.Throws<Exception>(() => sut.GenerateLinePoints((start, end) => start.IsManhattanAligned(end) || start.IsDiagonallyAligned(end)));
    }

    [Fact]
    void Line_GivenHorizontalDefinition_GeneratesExpectedLinePoints()
    {
      var sut = new Line("1,0 -> 4,0");
      Assert.Equal(0, sut.LowY());
      Assert.Equal(0, sut.HighY());
      Assert.Equal(1, sut.LowX());
      Assert.Equal(4, sut.HighX());

      var linePoints = sut.GenerateLinePoints((start, end) => start.IsManhattanAligned(end));
      Assert.Single(linePoints.Select(p => p.Y).Distinct());
      Assert.Equal(new[] { 1, 2, 3, 4 }, linePoints.Select(p => p.X).OrderBy(i => i));
    }

    [Fact]
    void Line_GivenVerticalDefinition_GeneratesExpectedLinePoints()
    {
      var sut = new Line("1,4 -> 1,9");
      Assert.Equal(4, sut.LowY());
      Assert.Equal(9, sut.HighY());
      Assert.Equal(1, sut.LowX());
      Assert.Equal(1, sut.HighX());

      var linePoints = sut.GenerateLinePoints((start, end) => start.IsManhattanAligned(end));
      Assert.Single(linePoints.Select(p => p.X).Distinct());
      Assert.Equal(new[] { 4, 5, 6, 7, 8, 9 }, linePoints.Select(p => p.Y).OrderBy(i => i));
    }

    [Theory]
    [MemberData(nameof(DiagonalLinePoints))]
    void Line_GivenDiagonalDefinition_GeneratesExpectedLinePoints(string definition, IEnumerable<Point> expectedPoints)
    {
      var sut = new Line(definition);
      var linePoints = sut.GenerateLinePoints((start, end) => start.IsDiagonallyAligned(end));
      Assert.Equal(expectedPoints.OrderBy(p => p.X), linePoints.OrderBy(p=> p.X));
    }

    [Theory]
    [MemberData(nameof(DiagonalAndManhattanLinePoints))]
    void Line_GivenDiagonalAndManhattanDefinition_GeneratesExpectedLinePoints(string definition, IEnumerable<Point> expectedPoints)
    {
      var sut = new Line(definition);
      var linePoints = sut.GenerateLinePoints((start, end) => start.IsManhattanAligned(end) || start.IsDiagonallyAligned(end)).OrderBy(p => p.X).ThenBy(p => p.Y);

      Assert.Equal(expectedPoints.OrderBy(p => p.X).ThenBy(p => p.Y), linePoints);
    }

    [Theory]
    [MemberData(nameof(DiagonalLineDefinitions))]
    void Line_GivenDiagonalDefinitions_IsLineWeCareAbout(string definition)
    {
      var sut = new Line(definition);
      Assert.True(sut.IsLineWeCareAbout((start, end) => (start.IsDiagonallyAligned(end))));
    }

    public static IEnumerable<object[]> BadLineDefinitions
    {
      get
      {
        yield return new[] { string.Empty };
        yield return new[] { (string) null };
        yield return new[] { ",1 -> 1,1" };
        yield return new[] { "1, -> 1,1" };
        yield return new[] { "1,1 -> ,1" };
        yield return new[] { "1,1 -> 1," };
        yield return new[] { "asdfsdafs" };
      }
    }

    public static IEnumerable<object[]> GoodLineDefinitions
    {
      get
      {
        yield return new object[] { "10,10 -> 10,10", true };
        yield return new object[] { "2,6 -> 2,235423", true };
        yield return new object[] { "98,23 -> 0,23", true };
        yield return new object[] { "-10,10 -> 10,235423", false };
      }
    }

    public static IEnumerable<object[]> LinesAndBoundingBoxes
    {
      get
      {
        yield return new object[] { "10,10 -> 10,10", 10, 10, 10, 10 };
        yield return new object[] { "2,6 -> 2,235423", 2, 2, 6, 235423 };
        yield return new object[] { "98,23 -> 0,23", 0, 98, 23, 23 };
        yield return new object[] { "-10,10 -> 10,235423", -10, 10, 10, 235423 };
        yield return new object[] { "2,3425 -> 6,4", 2, 6, 4, 3425 };
        yield return new object[] { "98,9789 -> 0,23", 0, 98, 23, 9789 };
        yield return new object[] { "1210,10 -> 10,235423", 10, 1210, 10, 235423 };
      }
    }

    public static IEnumerable<object[]> ManhattanLineDefinitions
    {
      get
      {
        yield return new object[] { "10,10 -> 10,10" };
        yield return new object[] { "2,6 -> 2,235423" };
        yield return new object[] { "98,23 -> 0,23" };
      }
    }

    public static IEnumerable<object[]> DiagonalLineDefinitions
    {
      get
      {
        yield return new object[] { "1,1 -> 3,3" };
        yield return new object[] { "9,7 -> 7,9" };
      }
    }

    public static IEnumerable<object[]> DontCareLineDefinitions
    {
      get
      {
        yield return new object[] { "120,104 -> 10,10" };
        yield return new object[] { "12412,6 -> 2,235423" };
        yield return new object[] { "98,2323 -> 0,23" };
      }
    }

    public static IEnumerable<object[]> DiagonalLinePoints
    {
      get
      {
        yield return new object[] {
          "1,1 -> 3,3",
          new[]
          {
            new Point(1,1),
            new Point(2,2),
            new Point(3,3)
          }
        };
        yield return new object[] {
          "9,7 -> 7,9",
          new[]
          {
            new Point(9,7),
            new Point(8,8),
            new Point(7,9)
          }
        };
      }
    }

    public static IEnumerable<object[]> DiagonalAndManhattanLinePoints
    {
      get
      {
        yield return new object[] {
          "1,1 -> 3,3",
          new[]
          {
            new Point(1,1),
            new Point(2,2),
            new Point(3,3)
          }
        };

        yield return new object[] {
          "9,7 -> 7,9",
          new[]
          {
            new Point(9,7),
            new Point(8,8),
            new Point(7,9)
          }
        };

        yield return new object[] {
          "1,1 -> 1,3",
          new[]
          {
            new Point(1,1),
            new Point(1,2),
            new Point(1,3)
          }
        };

        yield return new object[] {
          "9,7 -> 7,7",
          new[]
          {
            new Point(9,7),
            new Point(8,7),
            new Point(7,7)
          }
        };

      }
    }
  }
}
