using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneralElectric.Core
{
  public class ConsumptionReading : IConsumptionReading
  {
    public IList<bool> Readings
    {
      get; private set;
    }

    public ConsumptionReading(string rawReading)
    {
      Readings = rawReading.ToCharArray().Select((c) =>
      {
        if (c == '1')
        {
          return true;
        }
        else if (c == '0')
        {
          return false;
        }
        else
        {
          throw new ArgumentException(
            $"{rawReading} is an invalid reading.  Reading values must be comprised only 0 or 1.",
            nameof(rawReading)
          );
        }
      }
      ).ToList();
    }
  }
}
