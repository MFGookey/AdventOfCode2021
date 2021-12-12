using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Utilities.TwoD;

namespace Twinklepus.Core
{
  public class OctoGrid
  {
    public readonly IReadOnlyList<IReadOnlyList<Octopus>> Octopi;

    public Int64 FlashCount
    {
      get;
      private set;
    }

    private int _tickCount;

    public OctoGrid(IEnumerable<string> initialEnergyLevels)
    {
      // Disallow null collection
      if (initialEnergyLevels == null || initialEnergyLevels.Any(s => s == null))
      {
        throw new ArgumentNullException(nameof(initialEnergyLevels), "Energy levels may not be null");
      }

      // For now only allow rectangular energy levels
      if (initialEnergyLevels.Select(r => r.Length).Distinct().Count() > 1)
      {
        throw new ArgumentException("Initial energy levels must be rectangular", nameof(initialEnergyLevels));
      }

      // All levels must be ints
      if (initialEnergyLevels.Where(r => Regex.IsMatch(r, @"[^\d]")).Any())
      {
        throw new ArgumentException("Initial energy levels must all be integers", nameof(initialEnergyLevels));
      }

      FlashCount = 0;
      _tickCount = 0;

      // Set up our grid of octopi
      Octopi = initialEnergyLevels
        .Select(
          // For each row, split the characters and make Octopi
          r => r
            .ToCharArray()
            .Select(
              c => new Octopus(
                int.Parse(
                  c.ToString()
                )
              )
            )
            // Materialize the IEnumerable of Octopi as a list
            .ToList()
        )
        // Materialize the IEnumerable of Lists of Octopi as a list
        .ToList();

      var neighborPointMap = MatrixHelper.GenerateNeighborMaps(Octopi.Count(), Octopi.First().Count());

      // Need to wire up the neighbor collections next
      for (var r = 0; r < Octopi.Count(); r++)
      {
        for (var c = 0; c < Octopi[r].Count(); c++)
        {
          Octopi[r][c]
            .SetNeighbors(
              neighborPointMap[new Point(r, c)]
              .Select(p => Octopi[p.Row][p.Column])
            );
        }
      }
    }

    public void Tick()
    {
      _tickCount++;
      // This happens in two phases so we can complete the ticks later.
      foreach (var octopus in Octopi.SelectMany(o => o))
      {
        octopus.Tick();
      }

      FlashCount += Octopi.SelectMany(o => o).LongCount(o => o.FlashedThisTick);

      // Now complete the tick process
      foreach (var octopus in Octopi.SelectMany(o => o))
      {
        octopus.CompleteTick();
      }
    }

    public void Tick(int timesToTick)
    {
      if (timesToTick < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(timesToTick), "Tick must be given 0 or more times to tick.");
      }

      for (var i = 0; i < timesToTick; i++)
      {
        Tick();
      }
    }

    public void TickUntil(int endingTickCount)
    {
      while (_tickCount < endingTickCount)
      {
        Tick();
      }
    }

    public override string ToString()
    {
      return string.Join(
        "\n",
        Octopi
          .Select(
            r => string
              .Join(
                string.Empty,
                r.Select(o => o.ToString())
              )
          )
      );
    }
  }
}
