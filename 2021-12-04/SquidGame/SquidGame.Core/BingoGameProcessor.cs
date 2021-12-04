using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace SquidGame.Core
{
  public class BingoGameProcessor
  {
    private readonly IEnumerable<int> _draws;
    private readonly IEnumerable<BingoBoard> _boards;

    public int? LastDraw
    {
      get; private set;
    }

    public BingoGameProcessor(string draws, IEnumerable<string> boards)
    {
      LastDraw = null;

      // draws ought to be a comma delimited list of ints
      if (Regex.IsMatch(draws, "(?:\\d+,?)+") == false)
      {
        throw new ArgumentException("draws must be a comma delimited list of ints", nameof(draws));
      }

      _draws = draws.Split(",").Select(s => int.Parse(s));

      // boards is a collection of strings where each string is a grid of ints
      var boardList = new List<BingoBoard>();

      foreach (var boardString in boards)
      {
        boardList.Add(new BingoBoard(boardString));
      }

      _boards = boardList;
    }

    public BingoBoard PlayUntilBingo()
    {
      foreach (var draw in _draws)
      {
        LastDraw = draw;
        foreach (var board in _boards)
        {
          board.Mark(draw);
        }

        if (_boards.Any(board => board.HasWon))
        {
          return _boards.First(board => board.HasWon);
        }
      }

      throw new Exception("Ran out of draws and nobody has won!");
    }
  }
}
