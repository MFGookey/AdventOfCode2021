using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Carcanizer.Core
{
  public class CrabCommander
  {

    private readonly IEnumerable<int> _armada;

    public CrabCommander(IEnumerable<string> crabs)
    {
      if (crabs.Any(c => Regex.IsMatch(c, @"[^\d]")))
      {
        throw new ArgumentException("Crabs must all be numbers", nameof(crabs));
      }

      _armada = crabs.Select(c => int.Parse(c));
    }

    public int CalculateCrabsOfTheLinePositionCost()
    {
      // Given a list of positions, find the ONE position that minimizes the aggregate difference between each position and the position you want.
      // I mean, uh, fuel efficiency.

      // The line MUST be between the highest and lowest values of crab position.
      var range = Enumerable.Range(_armada.Min(), (_armada.Max()
      - _armada.Min() + 1));

      // Calculate the fuel costs for every option in the range across the whole armada
      return range.Select(
        r => new
        {
          // the position for which we are calculating
          position = r,
          cost = _armada
            .Select(
              // for each crab in the armada, calculate the cost
              // to get to the position we are considering
              c => Math.Abs(c - r)
            )
            // Total up the costs aross the armada
            // to move to the position we are considering
            .Sum()
        }
      )
      // Sort our options by their cost, ascending
      .OrderBy(option => option.cost)
      // Take the first element
      .First()
      // and return the cost
      .cost;

    }
  }
}
