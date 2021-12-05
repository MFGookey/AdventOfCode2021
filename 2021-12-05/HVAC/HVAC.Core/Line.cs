using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace HVAC.Core
{
  public class Line
  {
    public Point Start
    {
      get;
      private set;
    }

    public Point End
    {
      get;
      private set;
    }

    public Line(string definition)
    {
      // Definition is of the form "x1,y1 -> x2,y2"
      if (string.IsNullOrEmpty(definition) || Regex.IsMatch(definition, @"^-?\d+,-?\d+ -> -?\d+,-?\d+\n?$") == false)
      {
        throw new ArgumentException($"{definition} is not a valid line definition.  Expected format is \"x1,y1 -> x2,y2\".", nameof(definition));
      }

      var points = definition.Replace("\n", "").Split(" -> ");
      var coordinatePairs = points
        .Select(
          point => point
            .Split(",")
            .Select(
              number => int.Parse(number)
            )
            .ToArray()
        )
        .ToArray();

      Start = new Point(coordinatePairs[0][0], coordinatePairs[0][1]);
      End = new Point(coordinatePairs[1][0], coordinatePairs[1][1]);
    }

    public bool IsManhattanLine()
    {
      return Start.IsManhattanAligned(End);
    }

    public int LowX()
    {
      return Start.X <= End.X ? Start.X : End.X;
    }

    public int LowY()
    {
      return Start.Y <= End.Y ? Start.Y : End.Y;
    }

    public int HighX()
    {
      return Start.X > End.X ? Start.X : End.X;
    }

    public int HighY()
    {
      return Start.Y > End.Y ? Start.Y : End.Y;
    }

    public IEnumerable<Point> GenerateLinePoints()
    {
      if (this.IsManhattanLine() == false)
      {
        throw new Exception("Currently we will only process vertical or horizontal lines");
      }

      if (this.LowX() == this.HighX())
      {
        // This is a vertical line with a changing Y, or two identical points.
        if (this.LowY() == this.HighY())
        {
          return new[] { this.Start };
        }

        // Return in ascending Y order
        return Enumerable.Range(
            this.LowY(),
            this.HighY() - this.LowY() + 1
          )
          .Select(y => new Point(Start.X, y))
          .OrderBy(p => p.Y)
          .ToList();
      }
      else
      {
        // This is a horizontal line with a changing X.
        return Enumerable.Range(
            this.LowX(),
            this.HighX() - this.LowX() + 1
          )
          .Select(x => new Point(x, Start.Y))
          .OrderBy(p => p.X)
          .ToList();
      }
    }
  }
}
