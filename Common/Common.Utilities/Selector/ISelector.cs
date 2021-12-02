using System;
using System.Collections.Generic;

namespace Common.Utilities.Selector
{
  /// <summary>
  /// Given a list of integers, select some which satisfy a given property
  /// </summary>
  public interface ISelector
  {
    /// <summary>
    /// Given a list of ints, a target sum, and the number of terms to include in the final list, return an enumerable of those ints
    /// </summary>
    /// <param name="haystack">The list of ints to search for a sum equalling the target value</param>
    /// <param name="needle">The target value to which the returned list must sum</param>
    /// <param name="totalTerms">The number of ints that ought to be in the final listing</param>
    /// <returns>An enumerable of integers that sum to a target value</returns>
    IEnumerable<long> SumFinder(IEnumerable<long> haystack, long needle, int totalTerms);
  
    /// <summary>
    /// Given a list of ints, a target sum, and the number of terms to include in the final list, return an enumerable of those ints
    /// </summary>
    /// <param name="haystack">The list of ints to search for a sum equalling the target value</param>
    /// <param name="needle">The target value to which the returned list must sum</param>
    /// <param name="totalTerms">The number of ints that ought to be in the final listing</param>
    /// <returns>An enumerable of integers that sum to a target value</returns>
    IEnumerable<int> SumFinder(IEnumerable<int> haystack, int needle, int totalTerms);
  }
}
