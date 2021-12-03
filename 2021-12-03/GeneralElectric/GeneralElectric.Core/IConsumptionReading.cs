using System.Collections.Generic;

namespace GeneralElectric.Core
{
  public interface IConsumptionReading
  {
    IList<bool> Readings { get; }
  }
}