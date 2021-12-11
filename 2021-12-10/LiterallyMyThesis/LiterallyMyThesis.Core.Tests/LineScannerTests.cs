using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace LiterallyMyThesis.Core.Tests
{
  public class LineScannerTests
  {
    [Theory]
    [MemberData(nameof(GoodCharactersWithSyntaxErrorScores))]
    public void BasicScoreMapping_GivenValidCharacter_ReturnsExpectedScore(char character, int expectedScore)
    {
      Assert.Equal(expectedScore, LineScanner.BasicScoreMapping(character));
    }

    [Theory]
    [MemberData(nameof(BadCharacters))]
    public void BasicScoreMapping_GivenInvalidCharacter_ThrowsArgumentOutOfRangeException(char character)
    {
      Assert.Throws<ArgumentOutOfRangeException>(
        () => _ = LineScanner.BasicScoreMapping(character)
      );
    }

    [Theory]
    [MemberData(nameof(GoodCharactersWithCompletionScores))]
    public void CompletionScoreMapping_GivenValidCharacter_ReturnsExpectedScore(char character, int expectedScore)
    {
      Assert.Equal(expectedScore, LineScanner.CompletionScoreMapping(character));
    }

    [Theory]
    [MemberData(nameof(BadCharacters))]
    public void CompletionScoreMapping_GivenInvalidCharacter_ThrowsArgumentOutOfRangeException(char character)
    {
      Assert.Throws<ArgumentOutOfRangeException>(
        () => _ = LineScanner.CompletionScoreMapping(character)
      );
    }

    [Fact]
    public void LineScanner_GivenNullLine_ThrowsArgumentNullExcepiton()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new LineScanner(null)
      );
    }

    [Theory]
    [InlineData("a")]
    [InlineData("\n")]
    [InlineData("[][][][]{}{}{}{}{}{}}}}}{{{{{}{}{}{}{()()()()()()())(<><><><><><>w><><><><><")]
    public void LineScanner_GivenMalformedLine_ThrowsArgumentException(string line)
    {
      Assert.Throws<ArgumentException>(() => _ = new LineScanner(line));
    }

    [Theory]
    [MemberData(nameof(GoodLines))]
    public void LineScanner_GivenGoodLine_DoesNotThrowException(string line)
    {
      var exception = Record.Exception(() => _ = new LineScanner(line));
      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(GoodLines))]
    public void LineScanner_GivenGoodLine_SetsPropertiesAsExpected(string line)
    {
      var sut = new LineScanner(line);
      Assert.Null(sut.SyntaxErrorScore);
      Assert.Equal(line, sut.Line);
      Assert.Null(sut.SyntaxError);
      Assert.Null(sut.IsComplete);
      Assert.Null(sut.CompletionScore);
    }

    [Theory]
    [MemberData(nameof(GoodLines))]
    public void ScanLine_GivenGoodLine_DoesNotThrowException(string toScan)
    {
      var sut = new LineScanner(toScan);
      var exception = Record.Exception(() => sut.ScanLine());
      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(GoodLines))]
    public void ScanLine_GivenGoodLine_SetsPropertiesAsExpected(string toScan)
    {
      var sut = new LineScanner(toScan);
      sut.ScanLine();
      Assert.False(sut.SyntaxError);
      Assert.NotNull(sut.SyntaxErrorScore);
      Assert.Equal(0, sut.SyntaxErrorScore);
      Assert.True(sut.CompletionScore.HasValue);
    }

    [Theory]
    [MemberData(nameof(BadLines))]
    public void ScanLine_GiveBadLine_DoesNotThrowException(string toScan)
    {
      var sut = new LineScanner(toScan);
      var exception = Record.Exception(() => sut.ScanLine());
      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(BadLinesWithScores))]
    public void ScanLine_GivenBadLine_SetsPropertiesAsExpected(string toScan, int expectedScore)
    {
      var sut = new LineScanner(toScan);
      sut.ScanLine();
      Assert.True(sut.SyntaxError);
      Assert.False(sut.IsComplete);
      Assert.NotNull(sut.SyntaxErrorScore);
      Assert.Equal(expectedScore, sut.SyntaxErrorScore);
      Assert.False(sut.CompletionScore.HasValue);
    }

    [Theory]
    [MemberData(nameof(GoodCompleteLines))]
    public void ScanLine_GivenGoodCompleteLines_SetsCompleteTrue(string toScan)
    {
      var sut = new LineScanner(toScan);
      sut.ScanLine();
      Assert.False(sut.SyntaxError);
      Assert.True(sut.IsComplete);
      Assert.NotNull(sut.SyntaxErrorScore);
      Assert.Equal(0, sut.SyntaxErrorScore);
      Assert.True(sut.CompletionScore.HasValue);
      Assert.Equal(0, sut.CompletionScore.Value);
    }

    [Theory]
    [MemberData(nameof(GoodIncompleteLines))]
    public void ScanLine_GivenGoodIncompleteLines_SetsCompleteFalse(string toScan)
    {
      var sut = new LineScanner(toScan);
      sut.ScanLine();
      Assert.False(sut.SyntaxError);
      Assert.False(sut.IsComplete);
      Assert.NotNull(sut.SyntaxErrorScore);
      Assert.Equal(0, sut.SyntaxErrorScore);
      Assert.True(sut.CompletionScore.HasValue);
      Assert.True(sut.CompletionScore > 0);
    }

    [Theory]
    [MemberData(nameof(GoodIncompleteLinesWithCompletionScores))]
    public void ScanLine_GivenGoodIncompleteLines_SetsCompletionScoreAsExpected(string toScan, int completionScore)
    {
      var sut = new LineScanner(toScan);
      sut.ScanLine();
      Assert.False(sut.SyntaxError);
      Assert.False(sut.IsComplete);
      Assert.NotNull(sut.SyntaxErrorScore);
      Assert.Equal(0, sut.SyntaxErrorScore);
      Assert.True(sut.CompletionScore.HasValue);
      Assert.Equal(completionScore, sut.CompletionScore);
    }

    // This is really dumb but it isn't every day I can cover the
    // ENTIRE PROBLEM SPACE in unit tests and darn it, I'm going to take this one.
    public static IEnumerable<object[]> BadCharacters
    {
      get
      {
        return Enumerable
            // C# chars go from 0 to 0xFFFF or 65535
            .Range(0, 65536)
            .Except(
              new[]
                {
                  (int)')',
                  (int)']',
                  (int)'}',
                  (int)'>'
                }
            )
            .Select(i => new object[] { (char)i });
      }
    }

    public static IEnumerable<object[]> GoodCharactersWithSyntaxErrorScores
    {
      get
      {
        return new List<object[]>
        {
          new object[] { ')', 3 },
          new object[] { ']', 57 },
          new object[] { '}', 1197 },
          new object[] { '>', 25137 },
        };
      }
    }

    public static IEnumerable<object[]> GoodCharactersWithCompletionScores
    {
      get
      {
        return new List<object[]>
        {
          new object[] { ')', 1 },
          new object[] { ']', 2 },
          new object[] { '}', 3 },
          new object[] { '>', 4 },
        };
      }
    }

    public static IEnumerable<object[]> GoodCompleteLines
    {
      get
      {
        yield return new[] { string.Empty };
        yield return new[] { "()" };
        yield return new[] { "[]" };
        yield return new[] { "([])" };
        yield return new[] { "{()()()}" };
        yield return new[] { "<([{}])>" };
        yield return new[] { "[<>({}){}[([])<>]]" };
        yield return new[] { "(((((((((())))))))))" };
      }
    }

    public static IEnumerable<object[]> GoodIncompleteLines
    {
      get
      {
        yield return new[] { "(" };
        yield return new[] { "[" };
        yield return new[] { "([]" };
        yield return new[] { "{()()(" };
        yield return new[] { "<([{" };
        yield return new[] { "[<>({}){}[([])<" };
        yield return new[] { "(((((((((()))))" };
      }
    }

    public static IEnumerable<object[]> GoodIncompleteLinesWithCompletionScores
    {
      get
      {
        yield return new object[] { "(", 1};
        yield return new object[] { "[", 2 };
        yield return new object[] { "{", 3 };
        yield return new object[] { "<", 4 };
        yield return new object[] { "[({(<(())[]>[[{[]{<()<>>", 288957 };
        yield return new object[] { "[(()[<>])]({[<{<<[]>>(", 5566};
        yield return new object[] { "(((({<>}<{<{<>}{[]{[]{}", 1480781 };
        yield return new object[] { "{<[[]]>}<{[{[{[]{()[[[]", 995444 };
        yield return new object[] { "<{([{{}}[<[[[<>{}]]]>[]]", 294 };

      }
    }

    public static IEnumerable<object[]> GoodLines
    {
      get
      {
        return GoodCompleteLines.Concat(GoodIncompleteLines);
      }
    }
    public static IEnumerable<object[]> BadLines
    {
      get
      {
        yield return new[] { "{([(<{}[<>[]}>{[]{[(<()>" };
        yield return new[] { "[[<[([]))<([[{}[[()]]]" };
        yield return new[] { "[{[{({}]{}}([{[{{{}}([]" };
        yield return new[] { "[<(<(<(<{}))><([]([]()" };
        yield return new[] { "<{([([[(<>()){}]>(<<{{" };
      }
    }

    public static IEnumerable<object[]> BadLinesWithScores
    {
      get
      {
        yield return new object[] { "{([(<{}[<>[]}>{[]{[(<()>", 1197 };
        yield return new object[] { "[[<[([]))<([[{}[[()]]]", 3 };
        yield return new object[] { "[{[{({}]{}}([{[{{{}}([]", 57 };
        yield return new object[] { "[<(<(<(<{}))><([]([]()", 3 };
        yield return new object[] { "<{([([[(<>()){}]>(<<{{", 25137 };
      }
    }
  }
}
