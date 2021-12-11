using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace LiterallyMyThesis.Core.Tests
{
  public class SyntaxScorerTests
  {
    [Fact]
    public void SyntaxScorer_GivenNullLines_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(
        () => _ = new SyntaxScorer(null)
      );
    }

    [Theory]
    [MemberData(nameof(GoodLineCollections))]
    public void SyntaxScorer_GivenLines_DoesNotThrowException(IEnumerable<string> lines)
    {
      var exception = Record.Exception(
        () => _ = new SyntaxScorer(lines)
      );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(GoodLineCollections))]
    public void ScoreLines_GivenLines_DoesNotThrowException(IEnumerable<string> lines)
    {
      var sut = new SyntaxScorer(lines);

      var exception = Record.Exception(
        () => _ = sut.ScoreLines()
      );

      Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(GoodLineCollectionsWithSyntaxErrorScores))]
    public void ScoreLines_GivenLines_ReturnsExpectedSyntaxErrorScore(IEnumerable<string> lines, int expectedScore)
    {
      var sut = new SyntaxScorer(lines);

      Assert.Equal(expectedScore, sut.ScoreLines());
    }

    [Theory]
    [MemberData(nameof(GoodIncompleteLineCollectionsWithCompletionScores))]
    public void ScoreLines_GivenIncompleteLines_ReturnsExpectedCompletionScore(IEnumerable<string> lines, int expectedScore)
    {
      var sut = new SyntaxScorer(lines);
      Assert.Equal(expectedScore, sut.ScoreLineCompletion());
    }

    public static IEnumerable<object[]> GoodLineCollections
    {
      get
      {
        yield return new object[]
        {
          new List<string>()
        };

        yield return new object[]
        {
          new List<string>
          {
            "[({(<(())[]>[[{[]{<()<>>",
            "[(()[<>])]({[<{<<[]>>(",
            "{([(<{}[<>[]}>{[]{[(<()>",
            "(((({<>}<{<{<>}{[]{[]{}",
            "[[<[([]))<([[{}[[()]]]",
            "[{[{({}]{}}([{[{{{}}([]",
            "{<[[]]>}<{[{[{[]{()[[[]",
            "[<(<(<(<{}))><([]([]()",
            "<{([([[(<>()){}]>(<<{{",
            "<{([{{}}[<[[[<>{}]]]>[]]"
          }
        };

        yield return new object[]
        {
          new List<string> {
            string.Empty,
            "()",
            "[]",
            "([])",
            "{()()()}",
            "<([{}])>",
            "[<>({}){}[([])<>]]",
            "(((((((((())))))))))",
            "(",
            "[",
            "([]",
            "{()()(",
            "<([{",
            "[<>({}){}[([])<",
            "(((((((((()))))"
          }
        };
      }
    }

    public static IEnumerable<object[]> GoodLineCollectionsWithSyntaxErrorScores
    {
      get
      {
        yield return new object[]
        {
          new List<string>(),
          0
        };

        yield return new object[]
        {
          new List<string>
          {
            "[({(<(())[]>[[{[]{<()<>>",
            "[(()[<>])]({[<{<<[]>>(",
            "{([(<{}[<>[]}>{[]{[(<()>",
            "(((({<>}<{<{<>}{[]{[]{}",
            "[[<[([]))<([[{}[[()]]]",
            "[{[{({}]{}}([{[{{{}}([]",
            "{<[[]]>}<{[{[{[]{()[[[]",
            "[<(<(<(<{}))><([]([]()",
            "<{([([[(<>()){}]>(<<{{",
            "<{([{{}}[<[[[<>{}]]]>[]]"
          },
          26397
        };

        yield return new object[]
        {
          new List<string> {
            string.Empty,
            "()",
            "[]",
            "([])",
            "{()()()}",
            "<([{}])>",
            "[<>({}){}[([])<>]]",
            "(((((((((())))))))))",
            "(",
            "[",
            "([]",
            "{()()(",
            "<([{",
            "[<>({}){}[([])<",
            "(((((((((()))))"
          },
          0
        };
      }
    }

    public static IEnumerable<object[]> GoodIncompleteLineCollectionsWithCompletionScores
    {
      get
      {
        yield return new object[]
        {
          new List<string>
          {
            "[({(<(())[]>[[{[]{<()<>>",
            "[(()[<>])]({[<{<<[]>>(",
            "{([(<{}[<>[]}>{[]{[(<()>",
            "(((({<>}<{<{<>}{[]{[]{}",
            "[[<[([]))<([[{}[[()]]]",
            "[{[{({}]{}}([{[{{{}}([]",
            "{<[[]]>}<{[{[{[]{()[[[]",
            "[<(<(<(<{}))><([]([]()",
            "<{([([[(<>()){}]>(<<{{",
            "<{([{{}}[<[[[<>{}]]]>[]]"
          },
          288957
        };
      }
    }
  }
}
