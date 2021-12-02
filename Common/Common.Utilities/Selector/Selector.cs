using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Utilities.Selector
{
  /// <inheritdoc cref="ISelector"/>
  public class Selector : ISelector
  {
    /// <inheritdoc/>
    public IEnumerable<long> SumFinder(IEnumerable<long> haystack, long needle, int totalTerms)
    {
      // Given a haystack, and a needle we can recursively work to find the sum using slices of the input haystack.
      if (totalTerms > 0 && (haystack is null) == false)
      {
        IEnumerable<long> result = new List<long>();
        // Iterate over the haystack once.  Let i be the current index in haystack
        for (var i = 0; i < haystack.Count(); i++)
        {
          // If totalTerms == 1 && the current index in haystack == needle, we're done here!  Return a collection containing the current selected item as a collection
          if (totalTerms == 1)
          {
            if (haystack.Skip(i).First().Equals(needle))
            {
              result = result.Append(haystack.Skip(i).First());
              return result;
            }
          }
          else
          {
            // If totalTerms > 1: call SumFinder(haystack[i+1..end], needle-haystack[i], totalTerms-1);
            // Actually skipping i+1 because haystack is zero indexed
            result = SumFinder(haystack.Skip(i + 1), needle- haystack.Skip(i).First(), totalTerms - 1);

            // If that recursive call returns null, move on to the next i, else: append haystack[i] to the returned list and return that list.
            if (result is null == false)
            {
              result = result.Append(haystack.Skip(i).First());
              return result;
            }
          }
        }
      }

      // If we are at the end of the haystack, return null
      return null;
    }

    /// <inheritdoc/>
    public IEnumerable<int> SumFinder(IEnumerable<int> haystack, int needle, int totalTerms)
    {
      if (haystack == null)
      {
        return null;
      }

      var foundSums = SumFinder(haystack.Select(i => (long)i), (long)needle, totalTerms);
      
      if (foundSums == null)
      {
        return null;
      }

      return foundSums.Select(l => (int)l);
    }
  }
}
