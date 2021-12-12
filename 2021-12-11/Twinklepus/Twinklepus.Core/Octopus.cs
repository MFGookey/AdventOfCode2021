using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Twinklepus.Core
{
  public class Octopus
  {
    public int Energy
    {
      get;
      private set;
    }

    public bool FlashedThisTick {
      get;
      private set;
    }

    private IReadOnlyList<Octopus> _neighbors;

    public Octopus(int initialEnergy)
    {
      if (initialEnergy < 0 || initialEnergy > 9)
      {
        throw new ArgumentOutOfRangeException(nameof(initialEnergy), "Initial energy levels must be between 0 and 9 inclusive.");
      }

      Energy = initialEnergy;
      FlashedThisTick = false;
      _neighbors = null;
    }

    public void SetNeighbors(IEnumerable<Octopus> neighbors)
    {
      if (_neighbors == null)
      {
        _neighbors = neighbors.ToList();
      }
    }

    public void Tick()
    {
      if (FlashedThisTick == false)
      {
        Energy++;

        if (Energy > 9)
        {
          FlashedThisTick = true;
          Energy = 0;

          if (_neighbors != null)
          {
            // Yes these will all tick back at us
            // but it won't do anything because we've already set FlashedThisTick
            foreach (var neighbor in _neighbors)
            {
              neighbor.Tick();
            }
          }
        }
      }
    }

    public void CompleteTick()
    {
      FlashedThisTick = false;
    }

    public override string ToString()
    {
      return Energy.ToString();
    }
  }
}
