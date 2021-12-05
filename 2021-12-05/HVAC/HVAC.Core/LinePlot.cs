using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HVAC.Core
{
  public class LinePlot
  {
    private IEnumerable<Line> _lines;
    private Dictionary<Point, int> _heatMap;

    public LinePlot(IEnumerable<string> lineDefinitions)
    {
      var plottedLines = new List<Line>();
      foreach (var definition in lineDefinitions)
      {
        plottedLines.Add(new Line(definition));
      }

      _lines = plottedLines;
    }

    public void GenerateHeatMap(Func<Point, Point, bool> rule)
    {
      _heatMap = new Dictionary<Point, int>();
      foreach (var line in _lines.Where(l => l.IsLineWeCareAbout(rule)))
      {
        foreach (var point in line.GenerateLinePoints(rule))
        {
          if (_heatMap.ContainsKey(point))
          {
            _heatMap[point]++;
          }
          else
          {
            _heatMap.Add(point, 1);
          }
        }
      }
    }

    public int CountHeatAboveThreshold(int threshold)
    {
      return _heatMap.Values.Where(v => v >= threshold).Count();
    }
  }
}
