using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace GeneralElectric.Core
{
  public class ConsumptionCalculator
  {
    private readonly IEnumerable<IConsumptionReading> _readings;

    public ConsumptionCalculator(IEnumerable<IConsumptionReading> readings)
    {
      if (readings.Select(reading => reading.Readings.Count).Distinct().Count() != 1)
      {
        throw new ArgumentException("Readings for calculation must all have the same number of bits", nameof(readings));
      }

      _readings = readings;
    }

    public int CalculateGamma()
    {
      // Gamma is comprised of the most commonly occurring bit in each column of readings converted to decimal.
      var ones = new int[_readings.First().Readings.Count];
      var zeroes = new int[ones.Length];
      var result = new BitArray(ones.Length);

      foreach (var reading in _readings)
      {
        for (var i = 0; i < reading.Readings.Count; i++)
        {
          if (reading.Readings[i])
          {
            ones[i]++;
          }
          else
          {
            zeroes[i]++;
          }
        }
      }

      for (var i = 0; i < ones.Length; i++)
      {
        if (ones[i] > zeroes[i])
        {
          // This is dumb but the order is reversed vs what we are expecting
          result[ones.Length - i - 1] = true;
        }
        else{
          // This is dumb but the order is reversed vs what we are expecting
          result[ones.Length - i - 1] = false;
        }
      }

      int[] array = new int[1];
      result.CopyTo(array, 0);
      return array[0];
    }

    public int CalculateEpsilon()
    {
      // Epsilon is comprised of the least commonly occurring bit in each column of readings converted to decimal.
      var ones = new int[_readings.First().Readings.Count];
      var zeroes = new int[ones.Length];
      var result = new BitArray(ones.Length);

      foreach (var reading in _readings)
      {
        for (var i = 0; i < reading.Readings.Count; i++)
        {
          if (reading.Readings[i])
          {
            ones[i]++;
          }
          else
          {
            zeroes[i]++;
          }
        }
      }

      for (var i = 0; i < ones.Length; i++)
      {
        if (ones[i] <= zeroes[i])
        {
          // This is dumb but the order is reversed vs what we are expecting
          result[ones.Length - i - 1] = true;
        }
        else
        {
          // This is dumb but the order is reversed vs what we are expecting
          result[ones.Length - i - 1] = false;
        }
      }

      int[] array = new int[1];
      result.CopyTo(array, 0);
      return array[0];
    }
  }
}
