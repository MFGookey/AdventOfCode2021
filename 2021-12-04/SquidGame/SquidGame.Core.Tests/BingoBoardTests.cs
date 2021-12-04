using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace SquidGame.Core.Tests
{
  public class BingoBoardTests
  {
    [Theory]
    [MemberData(nameof(BadBoards))]
    void BingoBoard_GivenBadBoardState_ThrowsArgumentException(string boardState)
    {
      Assert.Throws<ArgumentException>(() => new BingoBoard(boardState));
    }

    [Theory]
    [MemberData(nameof(GoodBoards))]
    void BingoBoard_GivenGoodBoardState_DoesNotThrow(string boardState)
    {
      var exception = Record.Exception(() => new BingoBoard(boardState));

      Assert.Null(exception);
    }

    [Fact]
    void BingoBoard_GivenGoodBoardStateAndWinningMoves_Wins()
    {
      var sut = new BingoBoard(@"14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7");

      foreach (var draw in SampleGame)
      {
        Assert.False(sut.HasWon);
        sut.Mark(draw);
      }

      Assert.True(sut.HasWon);

    }

    [Fact]
    void BingoBoard_GivenGoodBoardStateAndWinningMoves_ScoresCorrectly()
    {
      var sut = new BingoBoard(@"14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7");

      foreach (var draw in SampleGame)
      {
        Assert.Null(sut.UnmarkedSum);
        Assert.Null(sut.Score);
        sut.Mark(draw);
      }

      Assert.Equal(188, sut.UnmarkedSum);
      Assert.Equal(4512, sut.Score);
    }

    [Theory]
    [MemberData(nameof(LosingBoards))]
    void BingoBoard_GivenGoodBoardStateAndLosingMoves_DoesNotWin(string losingBoardState)
    {
      var sut = new BingoBoard(losingBoardState);
      foreach (var draw in SampleGame)
      {
        Assert.False(sut.HasWon);
        sut.Mark(draw);
      }

      Assert.False(sut.HasWon);
    }

    [Theory]
    [MemberData(nameof(LosingBoards))]
    void BingoBoard_GivenGoodBoardStateAndLosingMoves_ScoresCorrectly(string losingBoardState)
    {
      var sut = new BingoBoard(losingBoardState);
      foreach (var draw in SampleGame)
      {
        Assert.Null(sut.UnmarkedSum);
        Assert.Null(sut.Score);
        sut.Mark(draw);
      }

      Assert.Null(sut.UnmarkedSum);
      Assert.Null(sut.Score);
    }

    public static IEnumerable<object[]> BadBoards
    {
      get
      {
        yield return new[] { "" };
        
        yield return new[] { @"22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10 18  5
 1 12 20 15 19" };

        yield return new[] { @" 3 15  0  2 22
 9 18 13 17  5
20 11 10 24  4
14 21 16 12  6" };

        yield return new[] { @"14 21 17 24  4
10 16 15  9 19
18  8 bad 26 20
22 11 13  6  5
 2  0 12  3  7" };

        yield return new[] { @"a" };
      }
    }

    public static IEnumerable<object[]> GoodBoards
    {
      get
      {
        yield return new[] { @"22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19" };

        yield return new[] { @" 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6" };

        yield return new[] { @"14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7" };
      }
    }

    public static IEnumerable<object[]> LosingBoards
    {
      get
      {
        return GoodBoards.SkipLast(1);
      }
    }

    public static int[] SampleGame = new[] { 7, 4, 9, 5, 11, 17, 23, 2, 0, 14, 21, 24 };
  }
}
