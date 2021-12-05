using System;
using System.Diagnostics.CodeAnalysis;

namespace HVAC.Core
{
  public class Point : IEquatable<Point>
  {
    public int X
    {
      get; private set;
    }

    public int Y
    {
      get; private set;
    }

    public Point(int x, int y)
    {
      X = x;
      Y = y;
    }

    public bool IsManhattanAligned(Point other)
    {
      return (this.X == other.X || this.Y == other.Y);
    }

    public bool IsDiagonallyAligned(Point other)
    {
      // we are diagonally aligned if the change in X == the change in Y between the two points
      return (
        Math.Max(this.X, other.X) - Math.Min(this.X, other.X)
      )
      ==
      (
        Math.Max(this.Y, other.Y) - Math.Min(this.Y, other.Y)
      );
    }

    public bool Equals([AllowNull] Point other)
    {
      if (other is null)
      {
        return false;
      }

      return this.X == other.X && this.Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is Point))
      {
        return false;
      }
      else
      {
        return this.Equals((Point)obj);
      }
    }

    public override int GetHashCode()
    {
      return this.ToString().GetHashCode();
    }

    public override string ToString()
    {
      return $"{this.X},{this.Y}";
    }
  }
}
