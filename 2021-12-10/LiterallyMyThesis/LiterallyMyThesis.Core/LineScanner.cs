using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LiterallyMyThesis.Core
{
  public class LineScanner
  {
    public static readonly Func<char, int> BasicScoreMapping = new Func<char, int>(
      (character) =>
      {
        switch (character)
        {
          case ')':
            return 3;
          case ']':
            return 57;
          case '}':
            return 1197;
          case '>':
            return 25137;
          default:
            throw new ArgumentOutOfRangeException("Allowable scorable characters are ), ], }, and >.", character, nameof(character));
        }
      }
    );

    public static readonly Func<char, int> CompletionScoreMapping = new Func<char, int>(
      (character) =>
      {
        switch (character)
        {
          case ')':
            return 1;
          case ']':
            return 2;
          case '}':
            return 3;
          case '>':
            return 4;
          default:
            throw new ArgumentOutOfRangeException("Allowable scorable characters are ), ], }, and >.", character, nameof(character));
        }
      }
    );

    private static readonly Func<char, bool> _shouldPush = new Func<char, bool>(
      (character) =>
      {
        switch (character)
        {
          case '(':
          case '[':
          case '{':
          case '<':
            return true;
          default:
            return false;
        }
      }
    );

    private static readonly Func<char, bool> _shouldPop = new Func<char, bool>(
      (character) =>
      {
        switch (character)
        {
          case ')':
          case ']':
          case '}':
          case '>':
            return true;
          default:
            return false;
        }
      }
    );

    private static readonly Func<char, char> _mapClosingMatch = new Func<char, char>(
      (opening) =>
      {
        switch (opening)
        {
          case '(':
            return ')';
          case '[':
            return ']';
          case '{':
            return '}';
          case '<':
            return '>';
          default:
            throw new ArgumentException($"Could not match up {opening} with a closing character");
        }
      }
    );

    private static readonly Func<char, char, bool> _checkMatch = new Func<char, char, bool>(
      (opening, closing) =>
      {
        switch (closing)
        {
          case ')':
            return opening == '(';
          case ']':
            return opening == '[';
          case '}':
            return opening == '{';
          case '>':
            return opening == '<';
          default:
            throw new ArgumentException($"Could not match up {opening} with {closing}");
        }
      }
    );

    public string Line
    {
      get;
      private set;
    }

    public int? SyntaxErrorScore
    {
      get;
      private set;
    }

    public bool? IsComplete
    {
      get;
      private set;
    }

    public bool? SyntaxError
    {
      get
      {
        return SyntaxErrorScore == null ? (bool?)null : SyntaxErrorScore > 0;
      }
    }

    public Int64? CompletionScore
    {
      get;
      private set;
    }

    public LineScanner(string line)
    {
      if (line == null)
      {
        throw new ArgumentNullException(nameof(line), "Line to scan may not be null");
      }

      if (line == null || Regex.IsMatch(line, @"[^()[\]{}<>]"))
      {
        throw new ArgumentException("Lines may only be made up of (, ), [, ], {, }, <, or > characters.", nameof(line));
      }

      SyntaxErrorScore = null;
      CompletionScore = null;
      IsComplete = null;
      Line = line;
    }

    public void ScanLine()
    {
      var splitLine = Line.ToCharArray();
      var openingCharacters = new Stack<char>(splitLine.Length);

      foreach (var c in splitLine)
      {
        if (_shouldPush(c))
        {
          openingCharacters.Push(c);
        }
        else if (_shouldPop(c))
        {
          if (openingCharacters.TryPop(out var opening) == false || _checkMatch(opening, c) == false)
          {
            SyntaxErrorScore = LineScanner.BasicScoreMapping(c);
            IsComplete = false;
            return;
          }
        }
        else
        {
          throw new Exception($"Could not figure out what to do with '{c}'");
        }
      }

      SyntaxErrorScore = 0;
      IsComplete = openingCharacters.Count == 0;
      CompleteLine(openingCharacters);
    }

    private void CompleteLine(Stack<char> openingCharacters)
    {
      Int64 runningScore = 0;

      while (openingCharacters.TryPop(out var opening))
      {
        runningScore = runningScore * 5 + CompletionScoreMapping(_mapClosingMatch(opening));
      }

      CompletionScore = runningScore;
    }
  }
}
