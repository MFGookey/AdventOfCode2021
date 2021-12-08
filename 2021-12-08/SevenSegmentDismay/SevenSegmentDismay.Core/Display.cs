using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace SevenSegmentDismay.Core
{
  public class Display : IEquatable<Display>
  {
    public IReadOnlyCollection<string> Segments
    {
      get; private set;
    }

    public readonly int LitSegmentCount;

    public static readonly string CanonicalMapping = @" aaaa 
b    c
b    c
 dddd 
e    f
e    f
 gggg ".Replace("\r\n", "\n");

    public static readonly Display ZERO = new Display("abcefg");
    public static readonly Display ONE = new Display("cf");
    public static readonly Display TWO = new Display("acdeg");
    public static readonly Display THREE = new Display("acdfg");
    public static readonly Display FOUR = new Display("bcdf");
    public static readonly Display FIVE = new Display("abdfg");
    public static readonly Display SIX = new Display("abdefg");
    public static readonly Display SEVEN = new Display("acf");
    public static readonly Display EIGHT = new Display("abcdefg");
    public static readonly Display NINE = new Display("abcdfg");

    public Display(string segments)
    {
      if (segments == null)
      {
        throw new ArgumentNullException(nameof(segments));
      }

      if (Regex.IsMatch(segments, @"[^a-g\s]", RegexOptions.IgnoreCase))
      {
        throw new ArgumentException("Segments may only be values of a, b, c, d, e, f, or g", nameof(segments));
      }

      var splitSegments = Regex.Replace(segments, @"\s", string.Empty).ToCharArray().Select(c => char.ToLowerInvariant(c).ToString());

      if (splitSegments.Count() != splitSegments.Distinct().Count())
      {
        throw new ArgumentException("Segments may only appear once in a reading", nameof(segments));
      }

      Segments = splitSegments.ToList();
      LitSegmentCount = Segments.Count();
    }

    public override string ToString()
    {
      var template = new string(CanonicalMapping.ToUpperInvariant());
      foreach (var segment in Segments)
      {
        template = template.Replace(segment.ToUpperInvariant(), segment.ToLowerInvariant());
      }

      return Regex.Replace(template, @"[A-G]", ".");
    }

    public IEnumerable<string> DisplayByRows()
    {
      return ToString().Split("\n").ToList();
    }

    public IEnumerable<string> AppendByRows(IEnumerable<string> toAppend)
    {
      return AppendByRows(toAppend, " ");
    }

    public IEnumerable<string> AppendByRows(IEnumerable<string> toBeAppended, string interstitial)
    {
      var toAppend = DisplayByRows().ToList();
      if (toBeAppended.Count() != toAppend.Count())
      {
        throw new ArgumentException($"AppendByRows expects the common format:\n{Display.CanonicalMapping}", nameof(toBeAppended));
      }

      return toBeAppended
        .Select(
          (rowText, rowNumber) =>
          {
            var sb = new StringBuilder();
            sb.Append(rowText);
            sb.Append(interstitial);
            sb.Append(toAppend[rowNumber]);
            return sb.ToString();
          }
        );
    }

    public bool Equals([AllowNull] Display other)
    {
      if (other == null)
      {
        return false;
      }

      return Enumerable.SequenceEqual(
        this.Segments.OrderBy(s => s),
        other.Segments.OrderBy(s => s)
      );
    }
  }
}
