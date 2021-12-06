using System;
using System.Collections.Generic;
using System.Text;

namespace KettlesOfFish.Core
{
  public class CircularIndex
  {
    public int CurrentValue
    {
      get;
      private set;
    }

    public readonly int Radix;

    public CircularIndex(int radix) : this(radix, 0)
    {
    }

    public CircularIndex(int radix, int initialValue)
    {
      if (radix <= 0)
      {
        throw new ArgumentException("Radix must be a value greater than 0", nameof(radix));
      }

      if (initialValue < 0 || initialValue >= radix)
      {
        throw new ArgumentException($"initial value must be >= 0 and < Radix ({radix}", nameof(initialValue));
      }

      Radix = radix;
      CurrentValue = initialValue;
    }

    public void Step()
    {
      CurrentValue = (CurrentValue + 1) % Radix;
    }
  }
}
