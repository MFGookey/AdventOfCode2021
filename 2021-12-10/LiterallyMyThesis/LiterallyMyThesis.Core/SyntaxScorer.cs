using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LiterallyMyThesis.Core
{
  public class SyntaxScorer
  {
    IEnumerable<LineScanner> Lines;

    public SyntaxScorer(IEnumerable<string> lines)
    {
      if (lines == null)
      {
        throw new ArgumentNullException(nameof(lines));
      }

      Lines = lines.Select(l => new LineScanner(l)).ToList();
    }

    public int ScoreLines()
    {
      foreach (var l in Lines)
      {
        l.ScanLine();
      }

      if (Lines.Any(l => l.Score.HasValue == false))
      {
        throw new InvalidOperationException("Found scanned lines without score");
      }

      return Lines
        .Where(l => l.Score.HasValue)
        .GroupBy(l => l.Score.Value)
        .Select(g => g.Key * g.Count())
        .Sum();
    }
  }
}
