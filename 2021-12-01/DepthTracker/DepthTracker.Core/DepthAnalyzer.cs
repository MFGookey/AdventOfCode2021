using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepthTracker.Core
{
  public class DepthAnalyzer
  {
    private readonly IEnumerable<int> _depthLog;

    public DepthAnalyzer(IEnumerable<int> depthLog)
    {
      _depthLog = depthLog;
    }

    public int CountIncreases()
    {
      return CountIncreases(1);
    }

    public int CountIncreases(int windowSize)
    {
      if (windowSize <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(windowSize), "Window Size must be greater than zero");
      }
      int currentIncreases = 0;
      var readingWindow = new Queue<int>();
      int? previousDepth = null;
      foreach (var reading in _depthLog)
      {
        readingWindow.Enqueue(reading);
        
        while (readingWindow.Count > windowSize)
        {
          _ = readingWindow.Dequeue();
        }

        if (readingWindow.Count == windowSize)
        {
          var sum = readingWindow.Sum();

          if (previousDepth.HasValue)
          {
            if (sum > previousDepth.Value)
            {
              currentIncreases++;
            }
          }

          previousDepth = sum;
        }
      }

      return currentIncreases;
    }
  }
}
