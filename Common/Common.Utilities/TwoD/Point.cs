using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Common.Utilities.TwoD
{
  public class Point : IEquatable<Point>, IComparable<Point>, IComparable
  {
    public int Row
    {
      get;
      private set;
    }

    public int Column
    {
      get;
      private set;
    }

    public int X
    {
      get
      {
        return Column;
      }
    }

    public int Y
    {
      get
      {
        return Row;
      }
    }

    public Point(int row, int column)
    {
      Row = row;
      Column = column;
    }

    public bool Equals([AllowNull] Point other)
    {
      if (other == null)
      {
        return false;
      }

      return this.Row == other.Row && this.Column == other.Column;
    }

    public override string ToString()
    {
      return $"{Row},{Column}";
    }

    public string ToXYString()
    {
      return $"{X},{Y}";
    }

    public override int GetHashCode()
    {
      return this.ToString().GetHashCode();
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

    public bool IsManhattanAligned(Point other)
    {
      return (this.Row == other.Row || this.Column == other.Column);
    }

    public bool IsDiagonallyAligned(Point other)
    {
      // we are diagonally aligned if the change in Row == the change in Column between the two points
      return (
        Math.Max(this.Row, other.Row) - Math.Min(this.Row, other.Row)
      )
      ==
      (
        Math.Max(this.Column, other.Column) - Math.Min(this.Column, other.Column)
      );
    }

    public int CompareTo([AllowNull] Point other)
    {
      if (other == null)
      {
        return this.ToString().CompareTo((string)null);
      }

      return this.ToString().CompareTo(other.ToString());
    }

    public int CompareTo(object obj)
    {
      if (!(obj is Point))
      {
        return this.ToString().CompareTo((string)null);
      }

      return this.CompareTo((Point)obj);
    }
  }
}
