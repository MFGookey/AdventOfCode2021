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

    public int? Score
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
        return Score == null ? (bool?)null : Score > 0;
      }
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

      Score = null;
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
            Score = LineScanner.BasicScoreMapping(c);
            IsComplete = false;
            return;
          }
        }
        else
        {
          throw new Exception($"Could not figure out what to do with '{c}'");
        }
      }

      Score = 0;
      IsComplete = openingCharacters.Count == 0;
    }
  }
}
