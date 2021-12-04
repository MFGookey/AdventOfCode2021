using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SquidGame.Core
{
  public class BingoBoard
  {
    public bool HasWon
    {
      get;
      private set;
    }

    public int? UnmarkedSum
    {
      get;
      private set;
    }

    public int? Score
    {
      get;
      private set;
    }

    private int? _lastDrawn;

    private readonly IReadOnlyList<IReadOnlyList<BingoSpot>> _boardState;

    public BingoBoard(string board)
    {
      // Normalize line endings
      board = board.Replace("\r\n", "\n");

      // the board state must be made up only of integers, spaces, and newlines
      if (Regex.IsMatch(board, @"^[\d \n]+$", RegexOptions.None) == false)
      {
        throw new ArgumentException("Board state must be one or more lines of integers and spaces", nameof(board));
      }

      IEnumerable<IEnumerable<int>> parsedBoard = board
        .Split('\n') // Split the board into rows
        .Where(row => string.IsNullOrWhiteSpace(row) == false) // Remove empty rows
        .Select(
          s => s
          .Split(' ') // split the rows into individual spaces
          .Where( // Remove empty spots where there was extra whitespace
            cell => string.IsNullOrWhiteSpace(cell) == false
          )
          .Select( // Turn the strings into ints.
            value => int.Parse(value)
          )
        );

      // Ensure the board is not ragged
      if (parsedBoard.Select(row => row.Count()).Distinct().Count() != 1)
      {
        throw new ArgumentException("Each row of the board must have the same number of values", nameof(board));
      }

      var columns = parsedBoard.Select(row => row.Count()).Distinct().First();

      if (parsedBoard.Count() != columns)
      {
        throw new ArgumentException("The board must have the same number of rows and columns", nameof(board));
      }

      var boardValues = new List<List<BingoSpot>>(columns);

      int rowNumber = 0;

      foreach (var row in parsedBoard)
      {
        boardValues.Add(new List<BingoSpot>(columns));
        
        foreach (var cell in row)
        {
          boardValues[rowNumber].Add(new BingoSpot(cell));
        }

        rowNumber++;
      }

      HasWon = false;
      UnmarkedSum = null;
      Score = null;
      _lastDrawn = null;

      _boardState = boardValues;
    }

    private void CheckForWin()
    {
      // If we have already won, bail out early.
      if (HasWon)
      {
        return;
      }

      // Any row with all marked columns, or any column with each row marked is a winner.

      // First check if there are even enough marked cells to possibly win
      var markedCount = _boardState.Select(row => row.Count(cell => cell.Marked)).Sum();

      if (markedCount < _boardState.Count())
      {
        return;
      }

      // If we get here, it is possible for a win based on the number of marked cells.

      // First check for if a whole row is marked
      if (_boardState.Any(row => row.Count(cell => cell.Marked) == row.Count()))
      {
        HasWon = true;
        return;
      }

      // If no whole row is marked, check by columns instead.
      for (var i = 0; i < _boardState.Count(); i++)
      {
        if (_boardState.Select(row => row[i]).Count(cell => cell.Marked) == _boardState.Count())
        {
          HasWon = true;
          return;
        }
      }
    }

    private void CalculateUnmarkedSum()
    {
      if (HasWon == false)
      {
        return;
      }

      // The score of the board is the sum of all unmarked spots on the board.
      UnmarkedSum = _boardState
        .Select(
          row => row // For each row
            .Where(cell => cell.Marked == false) // For each unmarked cel
            .Select(cell => cell.Value) // Select the value
            .Sum() // Sum across the rows
        )
      .Sum(); // Sum the value of each row's sum for a grand total
    }

    private void ScoreBoard()
    {
      if (HasWon == false || Score.HasValue || UnmarkedSum.HasValue == false)
      {
        return;
      }

      Score = UnmarkedSum * _lastDrawn;
    }

    public void Mark(int value)
    {
      // Don't continue marking winning boards
      if (HasWon)
      {
        return;
      }

      _lastDrawn = value;
      var validCellsByRow = _boardState.Select(row => row.Where(cell => cell.Value == value));

      foreach (var row in validCellsByRow)
      {
        foreach (var cell in row)
        {
          cell.Mark();
        }
      }

      CheckForWin();
      CalculateUnmarkedSum();
      ScoreBoard();
    }
  }
}
