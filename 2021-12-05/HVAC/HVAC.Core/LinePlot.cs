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
      _heatMap = new Dictionary<Point, int>();
      var plottedLines = new List<Line>();
      foreach (var definition in lineDefinitions)
      {
        plottedLines.Add(new Line(definition));
      }

      _lines = plottedLines;
    }

    public void GenerateHeatMap()
    {
      foreach (var line in _lines.Where(l => l.IsManhattanLine()))
      {
        foreach (var point in line.GenerateLinePoints())
        {
          if (_heatMap.ContainsKey(point))
          {
            var currentValue = _heatMap[point];
            currentValue++;
            _heatMap[point] = currentValue;
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
