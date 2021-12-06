using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace KettlesOfFish.Core
{
  public class FishKettles
  {
    private readonly CircularIndex _adultCurrentDayPointer;
    private readonly CircularIndex _childhoodCurrentDayPointer;
    private Int64[] _adultKettles;
    private Int64[] _childKettles;

    public FishKettles(IEnumerable<string> fish, int adultReproductionInterval, int childhoodInterval)
    {
      if (fish == null || fish.Where(f => Regex.IsMatch(f, @"\d+") == false).Any())
      {
        throw new ArgumentException("All fish inputs must be integers", nameof(fish));
      }

      if (adultReproductionInterval <= 0)
      {
        throw new ArgumentException("There must be more than 0 days in the adult reproduction interval!", nameof(adultReproductionInterval));
      }

      // Theoretically if childhoodInterval is 0, we can put new fish straight into the adult population
      if (childhoodInterval < 0)
      {
        throw new ArgumentException("Childhood may not be negative!");
      }

      var unkettledFish = fish
        .Select(i => Int64.Parse(i)) // Turn our string ints into real ints
        .GroupBy(i => i) // We want to count the fish with each distinct value
        .Select(
          g => new
          {
            timeToReproduce = g.Key,
            populationSize = g.LongCount()
          }
        );

      // unkettledFish is now an IEnumerable of anonymous objects which represents
      // the number of fish who have the given number of days until they reproduce.

      // Now we need to ensure that none of the fish we have have a longer reproduction interval than we can handle
      if (unkettledFish.Where(k => k.timeToReproduce >= adultReproductionInterval).Any())
      {
        // There's a chance we could add children into the population from the beginning
        // but I'm already making enough work for myself
        throw new ArgumentException($"Based on taking {adultReproductionInterval} days to reproduce, no fish may be equal to or more than that many days from reproduction!", nameof(fish));
      }



      _adultCurrentDayPointer = new CircularIndex(adultReproductionInterval);
      _childhoodCurrentDayPointer = new CircularIndex(childhoodInterval);
      _adultKettles = new Int64[adultReproductionInterval];
      _childKettles = new Int64[childhoodInterval];

      foreach (var k in unkettledFish)
      {
        // If we allow children in the input population, be aware of that here
        _adultKettles[k.timeToReproduce] = k.populationSize;
      }
    }

    public Int64 CurrentPopulation
    {
      get
      {
        return _adultKettles.Sum() + _childKettles.Sum();
      }
    }

    public void Tick()
    {
      Tick(1);
    }
    

    public void Tick(int timesToTick)
    {
      if (timesToTick < 0)
      {
        throw new ArgumentException("Times to tick must be positive!");
      }

      // And now the fun part starts

      Int64 maturingChildren;

      for (int i = 0; i < timesToTick; i++)
      {
        // save any children in _childKettles[_childhoodCurrentDayPointer.CurrentValue]
        maturingChildren = _childKettles[_childhoodCurrentDayPointer.CurrentValue];

        // set _childKettles[_childhoodCurrentDayPointer.CurrentValue] = _adultKettles[_adultCurrentDayPointer.CurrentValue]
        // to simulate the current day's time-to-reproduce fish reproducing
        _childKettles[_childhoodCurrentDayPointer.CurrentValue] = _adultKettles[_adultCurrentDayPointer.CurrentValue];

        // add the saved original value from _childKettles[_childhoodCurrentDayPointer.CurrentValue] to _adultKettles[_adultCurrentDayPointer.CurrentValue]
        // to simulate the current batch of children who matured into adults
        _adultKettles[_adultCurrentDayPointer.CurrentValue] += maturingChildren;

        // call _childhoodCurrentDayPointer.Step()
        _childhoodCurrentDayPointer.Step();
        // call _adultCurrentDayPointer.Step()
        _adultCurrentDayPointer.Step();
      }
    }
  }
}
