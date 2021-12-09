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

    public static readonly string TOP = "a";
    public static readonly string TOP_LEFT = "b";
    public static readonly string TOP_RIGHT = "c";
    public static readonly string MIDDLE = "d";
    public static readonly string BOTTOM_LEFT = "e";
    public static readonly string BOTTOM_RIGHT = "f";
    public static readonly string BOTTOM = "g";

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

    public override bool Equals(object obj)
    {
      return Equals(obj as Display);
    }

    public override int GetHashCode()
    {
      return ToString().GetHashCode();
    }

    public Display Remap(Dictionary<string, string> mapping)
    {
      if (
        mapping == null
        ||mapping.Keys.Count() != 7
        || mapping.Keys.Any(k => k.Length != 1)
        || mapping.Keys.Any(
          k =>
            k.CompareTo(Display.TOP) < 0
            || k.CompareTo(Display.BOTTOM) > 0
        )
        || mapping.Values.Distinct().Count() != 7
        || mapping.Values.Any(v => v.Length != 1)
        || mapping.Values.Any(
          v =>
            v.CompareTo(Display.TOP) < 0
            || v.CompareTo(Display.BOTTOM) > 0
        )
      )
      {
        throw new ArgumentException("Mapping must be a complete unique mapping from a-g through a-g where all values are unique.", nameof(mapping));
      }

      // mapping's keys are the FROM and the values are the TO
      var sb = new StringBuilder();
      foreach (var segment in Segments)
      {
        sb.Append(mapping[segment]);
      }

      return new Display(sb.ToString());
    }

    public int ReadDisplay()
    {
      if (this.Equals(Display.ZERO))
      {
        return 0;
      }

      if (this.Equals(Display.ONE))
      {
        return 1;
      }

      if (this.Equals(Display.TWO))
      {
        return 2;
      }

      if (this.Equals(Display.THREE))
      {
        return 3;
      }

      if (this.Equals(Display.FOUR))
      {
        return 4;
      }

      if (this.Equals(Display.FIVE))
      {
        return 5;
      }

      if (this.Equals(Display.SIX))
      {
        return 6;
      }

      if (this.Equals(Display.SEVEN))
      {
        return 7;
      }

      if (this.Equals(Display.EIGHT))
      {
        return 8;
      }

      if (this.Equals(Display.NINE))
      {
        return 9;
      }

      throw new InvalidOperationException("This display is not properly mapped!");
    }
  }
}
