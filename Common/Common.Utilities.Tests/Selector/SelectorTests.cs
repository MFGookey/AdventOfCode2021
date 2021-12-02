using System.Collections.Generic;
using System.Linq;
using Xunit;
using sut = Common.Utilities.Selector;

namespace Common.Utilities.Tests.Selector
{
  public class SelectorTests
  {
    [Fact]
    void SumFinder_TotalTermsIsZero_ReturnsNull()
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(new int[]{ 1, 2, 3 }, 1, 0);
      Assert.Null(result);
    }

    [Fact]
    void SumFinder_TotalTermsIsNegative_ReturnsNull()
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(new int[]{ 1, 2, 3 }, 1, int.MinValue);
      Assert.Null(result);
    }

    [Fact]
    void SumFinder_TotalTermsIsOneAndNoValidOptions_ReturnsNull()
    {
      var sut = new sut.Selector();
      var haystack = new int[] { 1, 2, 3 };
      var needle = -1;
      var totalTerms = 1;

      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Null(result);
    }

    [Theory]
    [InlineData(null, 1, 1)]
    [InlineData(null, 1, 2)]
    [InlineData(null, 5, 4)]
    void SumFinder_TotalTermsIsGreaterThanZeroAndHaystackIsNull_ReturnsNull(IEnumerable<int> haystack, int needle, int totalTerms)
    {
      var sut = new sut.Selector();

      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Null(result);
    }

    [Fact]
    void SumFinder_TotalTermsIsOneAndValidOption_ReturnsValidOption()
    {
      var sut = new sut.Selector();
      var haystack = new int[] { 1, 2, 3 };
      var needle = 2;
      var totalTerms = 1;
      var expectedResult = new int[] { 2 };

      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Equal(expectedResult, result);
    }

    [Theory]
    [MemberData(nameof(ValidOptions))]
    void SumFinder_TotalTermsMoreThanOneAndValidOptions_ReturnsValidOptions(IEnumerable<int> haystack, IEnumerable<int> needles, int totalTerms)
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(haystack, needles.Sum(), totalTerms);

      Assert.Equal(totalTerms, result.Count());
      Assert.Equal(needles.Count(), result.Count());
      Assert.Equal(needles.Sum(), result.Sum());
      Assert.Equal(needles.OrderBy(i => i), result.OrderBy(i => i));
    }

    [Theory]
    [MemberData(nameof(ValidXMASMessages))]
    void SumFinder_TotalTermsMoreThanOneAndValidXMASMessageOptions_ReturnsValidOptions(IEnumerable<int> haystack, int needle, int totalTerms, IEnumerable<int> expectedNeedles)
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Equal(totalTerms, result.Count());
      Assert.Equal(expectedNeedles.Sum(), result.Sum());
      Assert.Equal(needle, result.Sum());
      Assert.Equal(expectedNeedles.OrderBy(i => i), result.OrderBy(i => i));
    }

    [Theory]
    [MemberData(nameof(InvalidXMASMessages))]
    void SumFinder_TotalTermsMoreThanOneAndInvalidXMASMessageOptions_ReturnsNull(IEnumerable<int> haystack, int needle, int totalTerms)
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Null(result);
    }

    public static IEnumerable<object[]> ValidOptions {
      get
      {
        yield return new object[] {
          new int[] { 1, 2, 3 },
          new int[] { 1, 3 },
          2
        };

        // Same as above, needles in reversed order
        yield return new object[] {
          new int[] { 1, 2, 3 },
          new int[] { 3, 1 },
          2
        };

        yield return new object[] {
          new int[] { -1, 1, 5, 6 },
          new int[] { 5, 1 },
          2
        };

        yield return new object[] {
          new int[] { 1, 1, 1, 1, 1 },
          new int[]{ 1, 1, 1, 1 },
          4
        };
        
        // Provided by the Day 1 problem 1 description itself
        yield return new object[] {
          new int[] { 1721, 979, 366, 299, 675, 1456 },
          new int[] { 1721, 299 },
          2
        };
        
        yield return new object[] {
          new int[] { 1721, 979, 366, 299, 675, 1456 },
          new int[] { 299, 366 },
          2
        };

        yield return new object[] {
          new int[] { 1721, 979, 366, 299, -675, 1456 },
          new int[] { 979, -675, 366 },
          3
        };
        
        yield return new object[] {
          new int[] { 1721, 979, 366, -299, -675, 1456 },
          new int[] {-299, -675 },
          2
        };

        // Provided by the Day 2 problem 2 description itself
        yield return new object[] {
          new int[] { 1721, 979, 366, 299, 675, 1456 },
          new int[] { 979, 366, 675 },
          3
        };
      }
    }

    public static IEnumerable<object[]> ValidXMASMessages
    {
      get
      {
        yield return new object[]
        {
          new int[] { 35, 20, 15, 25, 47 },
          40,
          2,
          new int[] { 15, 25 }
        };

        yield return new object[]
        {
          new int[] { 20, 15, 25, 47, 40 },
          62,
          2,
          new int[] { 47, 15  }
        };

        yield return new object[]
        {
          new int[] { 15, 25, 47, 40, 62 },
          55,
          2,
          new int[] { 15, 40 }
        };

        yield return new object[]
        {
          new int[] { 25, 47, 40, 62, 55 },
          65,
          2,
          new int[] { 25, 40 }
        };

        yield return new object[]
        {
          new int[] { 47, 40, 62, 55, 65 },
          95,
          2,
          new int[] { 40, 55 }
        };

        yield return new object[]
        {
          new int[] { 40, 62, 55, 65, 95 },
          102,
          2,
          new int[] { 62, 40 }
        };

        yield return new object[]
        {
          new int[] { 62, 55, 65, 95, 102 },
          117,
          2,
          new int[] { 62, 55 }
        };

        yield return new object[]
        {
          new int[] { 55, 65, 95, 102, 117 },
          150,
          2,
          new int[] { 55, 95 }
        };

        yield return new object[]
        {
          new int[] { 65, 95, 102, 117, 150 },
          182,
          2,
          new int[] { 65, 117 }
        };

        yield return new object[]
        {
          new int[] { 102, 117, 150, 182, 127 },
          219,
          2,
          new int[] { 102, 117 }
        };

        yield return new object[]
        {
          new int[] { 117, 150, 182, 127, 219 },
          299,
          2,
          new int[] { 182, 117 }
        };

        yield return new object[]
        {
          new int[] { 150, 182, 127, 219, 299 },
          277,
          2,
          new int[] { 150, 127 }
        };

        yield return new object[]
        {
          new int[] { 182, 127, 219, 299, 277 },
          309,
          2,
          new int[] { 182, 127 }
        };

        yield return new object[]
        {
          new int[] { 127, 219, 299, 277, 309 },
          576,
          2,
          new int[] { 299, 277 }
        };
      }
    }

    public static IEnumerable<object[]> InvalidXMASMessages
    {
      get
      {
        yield return new object[]
        {
          new int[] { 95, 102, 117, 150, 182 },
          127,
          2
        };
      }
    }
  }
}
