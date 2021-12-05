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

    public bool IsLineWeCareAbout(Func<Point, Point, bool> rule)
    {
      return rule(Start, End);
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

    public IEnumerable<Point> GenerateLinePoints(Func<Point, Point, bool> rule)
    {
      if (this.IsLineWeCareAbout(rule) == false)
      {
        throw new Exception("Currently we will only process lines which follow the provided rule.");
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
        if (this.LowY() == this.HighY())
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
        else
        {
          // This is a diagonal line with X and Y changing by the same steps.

          // Figure out which point has a lower X value
          var lowerX = new[] { Start, End }.OrderBy(p => p.X).First();
          var higherX = new[] { Start, End }.OrderBy(p => p.X).Last();

          var xValues = Enumerable.Range(lowerX.X, (higherX.X - lowerX.X + 1)).ToList();
          var yValues = Enumerable.Range(Math.Min(higherX.Y, lowerX.Y), (Math.Max(higherX.Y, lowerX.Y) - Math.Min(higherX.Y, lowerX.Y) + 1)).OrderBy(y => lowerX.Y < higherX.Y ? y : -y).ToList();

          List<Point> results = new List<Point>();
          for (var i = 0; i < xValues.Count(); i++)
          {
            results.Add(new Point(xValues[i], yValues[i]));
          }

          return results;
        }
      }
    }
  }
}
