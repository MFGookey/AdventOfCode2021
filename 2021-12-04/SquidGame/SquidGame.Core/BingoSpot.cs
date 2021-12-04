using System;
using System.Collections.Generic;
using System.Text;

namespace SquidGame.Core
{
  public class BingoSpot
  {
    public int Value {
      get;
      private set;
    }
    public bool Marked
    {
      get; private set;
    }

    public BingoSpot(int value)
    {
      Marked = false;
      Value = value;
    }

    public void Mark()
    {
      Marked = true;
    }
  }
}
