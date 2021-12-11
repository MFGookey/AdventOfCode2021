using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LiterallyMyThesis.Core
{
  public class SyntaxScorer
  {
    IEnumerable<LineScanner> Lines;
    private bool _scored;

    public SyntaxScorer(IEnumerable<string> lines)
    {
      if (lines == null)
      {
        throw new ArgumentNullException(nameof(lines));
      }

      Lines = lines.Select(l => new LineScanner(l)).ToList();
      _scored = false;
    }

    public int ScoreLines()
    {
      _scored = true;
      foreach (var l in Lines)
      {
        l.ScanLine();
      }

      if (Lines.Any(l => l.SyntaxErrorScore.HasValue == false))
      {
        throw new InvalidOperationException("Found scanned lines without score");
      }

      return Lines
        .Where(l => l.SyntaxErrorScore.HasValue)
        .GroupBy(l => l.SyntaxErrorScore.Value)
        .Select(g => g.Key * g.Count())
        .Sum();
    }

    public Int64 ScoreLineCompletion()
    {
      if (_scored == false)
      {
        _ = ScoreLines();
      }

      // Given scored lines, for all lines that were incomplete, sort by completion score and return the median score
      var completionScores = Lines
        .Where(l => l.IsComplete == false && l.CompletionScore.HasValue)
        .Select(l => (Int64)l.CompletionScore.Value)
        .OrderBy(score => score);

      var scoreCount = completionScores.Count();

      if (scoreCount == 0)
      {
        return 0;
      }

      if (scoreCount % 2 == 0)
      {
        return completionScores.Skip(scoreCount / 2).First();
      }
      else
      {
        return completionScores.Skip((scoreCount - 1) / 2).First();
      }
    }
  }
}
