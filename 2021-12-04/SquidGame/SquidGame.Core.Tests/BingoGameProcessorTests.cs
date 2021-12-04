using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SquidGame.Core.Tests
{
  public class BingoGameProcessorTests
  {
    [Fact]
    void BingoGameProcessor_GivenValidDrawsAndBoard_DoesNotThrowException()
    {
      var exception = Record
        .Exception(
          () =>
          {
            var sut = new BingoGameProcessor(Draws, Boards);
          }
      );

      Assert.Null(exception);
    }

    [Fact]
    void BingoGameProcessor_GivenValidDrawsAndBoards_WhenPlayed_DoesNotThrowException()
    {
      var sut = new BingoGameProcessor(Draws, Boards);
      var exception = Record.Exception(
        () => sut.PlayUntilBingo()
      );

      Assert.Null(exception);
    }

    [Fact]
    void BingoGameProcessor_GivenValidDrawsAndBoards_WhenPlayed_ReturnsExpectedBoardScoring()
    {
      var sut = new BingoGameProcessor(Draws, Boards);
      var winningBoard = sut.PlayUntilBingo();

      Assert.True(winningBoard.HasWon);
      Assert.Equal(188, winningBoard.UnmarkedSum);
      Assert.Equal(4512, winningBoard.Score);
    }

    [Fact]
    
    void BingoGameProcessor_GivenValidDrawsAndBoards_WhenPlayedToLastWinningBoard_DoesNotThrowException()
    {
      var sut = new BingoGameProcessor(Draws, Boards);
      var exception = Record.Exception(
        () => sut.PlayUntilLastBoardWins()
      );

      Assert.Null(exception);
    }

    [Fact]
    void BingoGameProcessor_GivenValidDrawsAndBoards_WhenPlayedToLastWinningBoard_ReturnsExpectedBoardScoring()
    {
      var sut = new BingoGameProcessor(Draws, Boards);
      var winningBoard = sut.PlayUntilLastBoardWins();

      Assert.True(winningBoard.HasWon);
      Assert.Equal(148, winningBoard.UnmarkedSum);
      Assert.Equal(1924, winningBoard.Score);
    }

    public static readonly string Draws = "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1";

    public static readonly IReadOnlyList<string> Boards = new[]
    {
      @"22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19",

      @" 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6",

      @"14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7"
    };
  }
}
