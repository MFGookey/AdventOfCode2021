using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Utilities.TwoD;

namespace IRigami.Core
{
  public class FoldableSheet
  {
    private Queue<Action> _folds;
    private IEnumerable<Point> _points;

    public FoldableSheet(IEnumerable<string> points, IEnumerable<string> folds)
    {
      if (points == null || points.Any(p => p == null))
      {
        throw new ArgumentNullException("Points may not be null.", nameof(points));
      }

      if (folds == null || folds.Any(f => f == null))
      {
        throw new ArgumentNullException("Fold instructions may not be null.", nameof(folds));
      }

      if (points.Any(p => Regex.IsMatch(p, @"^-?\d+,-?\d+$") == false))
      {
        throw new ArgumentException("All points must be of the form \"{x},{y}\" where x and y are integers", nameof(points));
      }

      if (folds.Any(f => Regex.IsMatch(f, @"^fold along [xy]=-?\d+$") == false))
      {
        throw new ArgumentException("All fold instructions must be of the form \"fold along {x or y}={number}\"", nameof(folds));
      }

      _points = points.Select(
          s => {
            var split = s.Split(",").Select(n => int.Parse(n)).ToList();
            return new Point(split[1], split[0]);
          }
        );

      _folds = new Queue<Action>(
          folds.Select(
            f => {
              var instruction = f.Split("=").ToList();
              if (instruction[0].Equals("fold along x"))
              {
                return new Action(() => this.FoldX(int.Parse(instruction[1])));
              }
              else
              {
                return new Action(() => this.FoldY(int.Parse(instruction[1])));
              }
            }
          )
        );
    }

    public void Tick()
    {
      _folds.Dequeue()();
    }

    public int CountVisiblePoints()
    {
      return _points.Distinct().Count();
    }

    private void FoldX(int foldingColumn)
    {
      var pointsToMove = _points.Where(p => p.X > foldingColumn);
      _points = _points.Concat(
        pointsToMove.Select(
          p => new Point(p.Y, foldingColumn - (p.X - foldingColumn))
        )
      )
      .ToList();

      _points = _points.Except(_points.Where(p => p.X > foldingColumn)).Distinct().ToList();
    }

    private void FoldY(int foldingRow)
    {
      var pointsToMove = _points.Where(p => p.Y > foldingRow);
      _points = _points.Concat(
        pointsToMove.Select(
          p => new Point(foldingRow - (p.Y - foldingRow), p.X)
        )
      )
      .ToList();

      _points = _points.Except(_points.Where(p => p.Y > foldingRow)).Distinct().ToList();
    }

    // it would be useful to have a method to plot the points
    public string Plot()
    {
      var grid = new char[_points.Max(p => p.Y) + 1,_points.Max(p => p.X) + 1];
      var sb = new StringBuilder();
      for (var y = 0; y < grid.GetLength(0); y++)
      {
        for (var x = 0; x < grid.GetLength(1); x++)
        {
          if (_points.Contains(new Point(y, x)))
          {
            grid[y, x] = '#';
          }
          else
          {
            grid[y, x] = '.';
          }
          sb.Append(grid[y, x]);
        }
        sb.AppendLine();
      }

      return sb.ToString();
    }
  }
}
