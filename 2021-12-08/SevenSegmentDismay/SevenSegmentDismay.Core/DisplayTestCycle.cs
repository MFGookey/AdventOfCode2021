using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace SevenSegmentDismay.Core
{
  public class DisplayTestCycle
  {
    public IEnumerable<Display> TestResults
    {
      get;
      private set;
    }

    public IReadOnlyList<Display> DisplayReading
    {
      get;
      private set;
    } 

    public DisplayTestCycle(string results)
    {
      if (results == null)
      {
        throw new ArgumentNullException(nameof(results));
      }

      if (Regex.IsMatch(results, @"^(?:[a-g]{1,7} ){10}\|(?: [a-g]{1,7}){4}$") == false)
      {
        throw new ArgumentException("Results must match the format of \"{DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} {DisplayString} | {DisplayString} {DisplayString} {DisplayString} {DisplayString}", nameof(results));
      }

      var splitResults = results.Split("|");

      TestResults = splitResults[0].Trim().Split(" ").Select(result => new Display(result));

      DisplayReading = splitResults[1].Trim().Split(" ").Select(reading => new Display(reading)).ToList();
    }
    
    private Dictionary<string, string> DetermineMapping()
    {
      var results = new Dictionary<string, string>();
      // First make sure we CAN determine a mapping
      var possibles = new IEnumerable<Display>[10];
      possibles[1] = TestResults.Where(result => result.LitSegmentCount == Display.ONE.LitSegmentCount);

      possibles[4] = TestResults.Where(result => result.LitSegmentCount == Display.FOUR.LitSegmentCount);

      possibles[7] = TestResults.Where(result => result.LitSegmentCount == Display.SEVEN.LitSegmentCount);

      possibles[8] = TestResults.Where(result => result.LitSegmentCount == Display.EIGHT.LitSegmentCount);

      if (
        possibles[1].Count() != 1
        || possibles[4].Count() != 1
        || possibles[7].Count() != 1
        || possibles[8].Count() != 1
      )
      {
        throw new InvalidOperationException("Could not find exactly one possible display with the same number of lit segments as canonical 1, 4, 7, or 8");
      }

      var unmappedDisplays = new Display[10];
      unmappedDisplays[1] = possibles[1].First();
      unmappedDisplays[4] = possibles[4].First();
      unmappedDisplays[7] = possibles[7].First();
      unmappedDisplays[8] = possibles[8].First();

      // 7 is 1 with an additional top segment lit, so the difference between the two is the TOP segment

      var possibleSegments = unmappedDisplays[7].Segments.Except(unmappedDisplays[1].Segments);

      if (possibleSegments.Count() != 1)
      {
        throw new InvalidOperationException("Could not determine TOP segment from difference betwen predetermined 7 and 1");
      }

      results.Add(possibleSegments.First(), Display.TOP);

      // bottom right is present in 1, and also in every other number but 2
      var possiblePossibleTwos = unmappedDisplays[1]
        .Segments
        .Select(
          s => new {
              segment = s,
              displaysWithSegment = TestResults
                .Where(
                  display => display.Segments.Contains(s)
                )
            }
        )
        .Where(k => k.displaysWithSegment.Count() == 9)
        .Select(
          k => TestResults
            .Where(
              display => display.Segments.Contains(k.segment) == false
            )
        );

      // Only one of the two should have anyting in it.
      if (
        possiblePossibleTwos.Count(p => p.Any()) != 1
        || possiblePossibleTwos.Where(p => p.Any()).First().Count() != 1
      )
      {
        throw new InvalidOperationException("Could not determine two from the absence of the bottom right segment");
      }

      // Not strictly necessary but let's be completionist.
      possibles[2] = possiblePossibleTwos.Where(p => p.Any()).First();

      unmappedDisplays[2] = possibles[2].First();

      possibleSegments = unmappedDisplays[1].Segments.Except(unmappedDisplays[2].Segments);

      if (possibleSegments.Count() != 1)
      {
        throw new InvalidOperationException("Could not determine BOTTOM_RIGHT segment from difference betwen predetermined 1 and 2");
      }

      results.Add(possibleSegments.First(), Display.BOTTOM_RIGHT);

      // Now we know that the remaining segment in unmappedOne is TOP_RIGHT
      possibleSegments = unmappedDisplays[1].Segments.Except(possibleSegments);

      if (possibleSegments.Count() != 1)
      {
        throw new InvalidOperationException("Could not determine TOP_RIGHT segment by removing discovered BOTTOM_RIGHT segment from 1");
      }

      results.Add(possibleSegments.First(), Display.TOP_RIGHT);

      possibles[6] = TestResults
        .Where(
          r => r.LitSegmentCount == 6
            && r.Segments.Contains(
                ReverseDictionaryKeyLookup(results, Display.TOP_RIGHT)
              ) == false
        );

      if (possibles[6].Count() != 1)
      {
        throw new InvalidOperationException("Could not determine six from the 6-lit-segment set with an unlit absence of the top right segment");
      }

      unmappedDisplays[6] = possibles[6].First();

      // We now know TOP, TOP_RIGHT, and BOTTOM_RIGHT
      // Along with 1, 2, 4, 6, 7, and 8

      // Of the remaining segments, 5 has 5 segments with no top right lit
      possibles[5] = TestResults
          .Except(unmappedDisplays.Where(d => d != null))
          .Where(r =>
            r.LitSegmentCount == 5
            && r
              .Segments
              .Contains(
                ReverseDictionaryKeyLookup(results, Display.TOP_RIGHT)
              ) == false
          );

      if (possibles[5].Count() != 1)
      {
        throw new InvalidOperationException("Could not determine five from the undetermined 5-lit-segment set with an unlit absence of the top right segment");
      }

      unmappedDisplays[5] = possibles[5].First();

      // 3 is the only 5 segment left undetermined.
      possibles[3] = TestResults
        .Except(unmappedDisplays.Where(d => d != null))
        .Where(r => r.LitSegmentCount == 5);
      
      if (possibles[3].Count() != 1)
      {
        throw new InvalidOperationException("Could not determine three from the undetermined 5-lit-segment set.");
      }

      unmappedDisplays[3] = possibles[3].First();

      // We now know TOP, TOP_RIGHT, and BOTTOM_RIGHT
      // Along with 1, 2, 3, 4, 5, 6, 7, and 8

      // 6 except 5 is BOTTOM_LEFT
      possibleSegments = unmappedDisplays[6].Segments.Except(unmappedDisplays[5].Segments);

      if (possibleSegments.Count() != 1)
      {
        throw new InvalidOperationException("Could not determine BOTTOM_LEFT by the difference in segments between 6 and 5");
      }

      results.Add(possibleSegments.First(), Display.BOTTOM_LEFT);

      // We now know TOP, TOP_RIGHT, BOTTOM_LEFT, and BOTTOM_RIGHT
      // Along with 1, 2, 3, 4, 5, 6, 7, and 8
      // 9 is 8 except BOTTOM_LEFT
      possibles[9] = TestResults
        .Where(
          r => Enumerable
            .SequenceEqual(
                r
                  .Segments
                  .OrderBy(s => s),
                unmappedDisplays[8]
                  .Segments
                  .Where(
                    s => s.Equals(
                      ReverseDictionaryKeyLookup(results, Display.BOTTOM_LEFT)
                    ) == false
                  )
                  .OrderBy(s => s)
            )
        );

      if (possibles[9].Count() != 1)
      {
        throw new InvalidOperationException("Could not determine 9 from remaining test results.");
      }

      unmappedDisplays[9] = possibles[9].First();

      // 0 is what's left
      possibles[0] = TestResults.Except(unmappedDisplays.Where(d => d != null));

      if (possibles[0].Count() != 1)
      {
        throw new InvalidOperationException("Could not determine 0 from remaining test results.");
      }

      unmappedDisplays[0] = possibles[0].First();

      // We now know all the numbers and 4 of the 7 segments.

      // MIDDLE is 8 - 0
      possibleSegments = unmappedDisplays[8].Segments.Except(unmappedDisplays[0].Segments);

      if (possibleSegments.Count() != 1)
      {
        throw new InvalidOperationException("Could not determine MIDDLE by the difference in segments between 8 and 0");
      }

      results.Add(possibleSegments.First(), Display.MIDDLE);

      // We now know TOP, TOP_RIGHT, MIDDLE, BOTTOM_LEFT, and BOTTOM_RIGHT

      // TOP_LEFT is 6 - 2 - BOTTOM_RIGHT
      possibleSegments = unmappedDisplays[6]
        .Segments
        .Except(unmappedDisplays[2].Segments)
        .Where(s => s.Equals(ReverseDictionaryKeyLookup(results, Display.BOTTOM_RIGHT)) == false);

      if (possibleSegments.Count() != 1)
      {
        throw new InvalidOperationException("Could not determine TOP_LEFT by the difference in segments between 6, 2, and BOTTOM_RIGHT");
      }

      results.Add(possibleSegments.First(), Display.TOP_LEFT);

      // We now know TOP, TOP_LEFT, TOP_RIGHT, MIDDLE, BOTTOM_LEFT, and BOTTOM_RIGHT


      // All we need now is BOTTOM which is the only unmapped segment
      possibleSegments = unmappedDisplays[8].Segments.Except(results.Keys);

      if (possibleSegments.Count() != 1)
      {
        throw new InvalidOperationException("Could not determine BOTTOM by looking for the missing value in results.Keys");
      }

      results.Add(possibleSegments.First(), Display.BOTTOM);



      return results;
    }

    private string ReverseDictionaryKeyLookup(Dictionary<string, string> haystack, string needle)
    {
      if (
        haystack
        .Values
        .Where(v => v.Equals(needle))
        .Count() != 1
      )
      {
        throw new InvalidOperationException($"Could not reverse lookup {needle} in provided dictionary");
      }

      return haystack
        .Where(kvp => kvp.Value == needle)
        .Select(kvp => kvp.Key)
        .First();
    }

    public int DetermineReading()
    {
      var mapping = DetermineMapping();
      return DisplayReading
        .Select(
          (d, index) => (int)Math.Pow(10, 3 - index) * d.Remap(mapping).ReadDisplay()
        )
        .Sum();
    }
  }
}
