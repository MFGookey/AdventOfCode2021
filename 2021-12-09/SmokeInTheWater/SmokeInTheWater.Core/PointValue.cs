using System;
using System.Collections.Generic;
using System.Text;

namespace SmokeInTheWater.Core
{
  public class PointValue
  {
    public int Row
    {
      get;
      private set;
    }

    public int Column
    {
      get; private set;
    }

    public int Value
    {
      get; private set;
    }

    public PointValue(int row, int column, int value)
    {
      Row = row;
      Column = column;
      Value = value;
    }
  }
}
