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

    private BitArray CalculateByBitColumns(Func<int, int, bool> bumpOnes)
    {
      return CalculateByBitColumns(bumpOnes, _readings);
    }

    private BitArray CalculateByBitColumns(Func<int, int, bool> bumpOnes, IEnumerable<IConsumptionReading> readings)
    {
      var readingLength = 1;

      if (readings.Any())
      {
        readingLength = readings.FirstOrDefault().Readings.Count;
      }

      var ones = new int[readingLength];
      var zeroes = new int[readingLength];
      var result = new BitArray(readingLength);

      foreach (var reading in readings)
      {
        for (var i = 0; i < readingLength; i++)
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
        if (bumpOnes(ones[i], zeroes[i]))
        {
          // This is dumb but the order is reversed vs what we are expecting
          result[readingLength - i - 1] = true;
        }
        else
        {
          // This is dumb but the order is reversed vs what we are expecting
          result[readingLength - i - 1] = false;
        }
      }

      return result;
    }

    private int ConvertToInt(BitArray toConvert)
    {
      int[] array = new int[1];
      toConvert.CopyTo(array, 0);
      return array[0];
    }

    public int CalculateGamma()
    {
      var result = CalculateByBitColumns((one, zero) => one > zero);

      return ConvertToInt(result);
    }

    public int CalculateEpsilon()
    {
      // Epsilon is comprised of the least commonly occurring bit in each column of readings converted to decimal.
      var result = CalculateByBitColumns((one, zero) => one <= zero);
      return ConvertToInt(result);
    }

    public int CalculateRatingByCriteria(Func<int, int, bool> bumpOnes)
    {
      var filteredResults = _readings;

      for (int i = filteredResults.First().Readings.Count() - 1; i >= 0; i--)
      {

        if (filteredResults.Count() == 1)
        {
          // convert the filtered result reading into an int and return it.
          var finalResult = new BitArray(filteredResults.First().Readings.Reverse().ToArray());
          return ConvertToInt(finalResult);
        }
        var precalculatedResults = CalculateByBitColumns(bumpOnes, filteredResults);
        filteredResults = filteredResults.Where(result => result.Readings[result.Readings.Count - i - 1] == precalculatedResults[i]).ToList();
        ;
      }

      if (filteredResults.Count() == 1)
      {
        // convert the filtered result reading into an int and return it.
        var finalResult = new BitArray(filteredResults.First().Readings.Reverse().ToArray());
        return ConvertToInt(finalResult);
      }

      throw new Exception("Could not narrow down the filtered results to a single record");
    }

    public int CalculateOxygenGeneratorRating()
    {
      return CalculateRatingByCriteria((one, zero) => one >= zero);
    }

    public int CalculateCO2ScrubberRating()
    {
      return CalculateRatingByCriteria((one, zero) => one < zero);
    }
  }
}
