using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Common.Utilities.TwoD;
using System.Linq;

namespace Common.Utilities.Tests.TwoD
{
  public class MatrixHelperTests
  {
    [Theory]
    [MemberData(nameof(InvalidNeighborMapParameters))]
    public void GenerateNeighborMaps_GivenInvalidParameters_ThrowsArgumentException(int rows, int columns)
    {
      Assert.Throws<ArgumentOutOfRangeException>(
        () => _ = MatrixHelper.GenerateNeighborMaps(rows, columns)
      );
    }

    [Theory]
    [MemberData(nameof(NeighborParametersWithExpectedMap))]
    public void GenerateNeighborMaps_GivenValidParameters_GeneratesExpectedMap(
      int rows,
      int columns,
      IReadOnlyDictionary<Point, IReadOnlyList<Point>> expectedMap
    )
    {
      // It would be nice if I could do this all with one assert but Xunit seems to struggle.
      var map = MatrixHelper.GenerateNeighborMaps(rows, columns);
      
      Assert.Equal(
        expectedMap.Keys.OrderBy(p => p),
        map.Keys.OrderBy(p => p)
      );

      foreach (var point in expectedMap.Keys)
      {
        Assert.Equal(
          expectedMap[point].OrderBy(p=>p),
          map[point].OrderBy(p=>p)
        );
      }
    }


    public static IEnumerable<object[]> InvalidNeighborMapParameters
    {
      get
      {
        yield return new object[]
        {
          0,
          1
        };

        yield return new object[]
        {
          1,
          0
        };

        yield return new object[]
        {
          -1,
          1
        };

        yield return new object[]
        {
          1,
          -1
        };
      }
    }

    public static IEnumerable<object[]> NeighborParametersWithExpectedMap
    {
      get {
        yield return new object[]
        {
          1,
          1,
          new Dictionary<Point, IReadOnlyList<Point>>
          {
            {
              new Point(0,0),
              new List<Point>()
            }
          }
        };

        yield return new object[]
        {
          2,
          2,
          new Dictionary<Point, IReadOnlyList<Point>>
          {
            {
              new Point(0,0),
              new List<Point>
              {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0)
              }
            },
            {
              new Point(0,1),
              new List<Point>
              {
                new Point(0, 0),
                new Point(1, 1),
                new Point(1, 0)
              }
            },
            {
              new Point(1,0),
              new List<Point>
              {
                new Point(0, 0),
                new Point(1, 1),
                new Point(0, 1)
              }
            },
            {
              new Point(1,1),
              new List<Point>
              {
                new Point(0, 0),
                new Point(1, 0),
                new Point(0, 1)
              }
            }
          }
        };

        yield return new object[]
        {
          3,
          5,
          new Dictionary<Point, IReadOnlyList<Point>>
          {
            // Begin row 0
            {
              new Point(0,0),
              new List<Point>
              {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0)
              }
            },
            {
              new Point(0,1),
              new List<Point>
              {
                new Point(0, 0),
                new Point(0, 2),
                new Point(1, 0),
                new Point(1, 1),
                new Point(1, 2)
              }
            },
            {
              new Point(0,2),
              new List<Point>
              {
                new Point(0, 1),
                new Point(0, 3),
                new Point(1, 1),
                new Point(1, 2),
                new Point(1, 3)
              }
            },
            {
              new Point(0,3),
              new List<Point>
              {
                new Point(0, 2),
                new Point(0, 4),
                new Point(1, 2),
                new Point(1, 3),
                new Point(1, 4)
              }
            },
            {
              new Point(0,4),
              new List<Point>
              {
                new Point(0, 3),
                new Point(1, 3),
                new Point(1, 4)
              }
            },
            // End row 0
            // Begin row 1
            {
              new Point(1,0),
              new List<Point>
              {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 0),
                new Point(2, 1),
              }
            },
            {
              new Point(1,1),
              new List<Point>
              {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2),
                new Point(1, 0),
                new Point(1, 2),
                new Point(2, 0),
                new Point(2, 1),
                new Point(2, 2)
              }
            },
            {
              new Point(1,2),
              new List<Point>
              {
                new Point(0, 1),
                new Point(0, 2),
                new Point(0, 3),
                new Point(1, 1),
                new Point(1, 3),
                new Point(2, 1),
                new Point(2, 2),
                new Point(2, 3)
              }
            },
            {
              new Point(1,3),
              new List<Point>
              {
                new Point(0, 2),
                new Point(0, 3),
                new Point(0, 4),
                new Point(1, 2),
                new Point(1, 4),
                new Point(2, 2),
                new Point(2, 3),
                new Point(2, 4)
              }
            },
            {
              new Point(1,4),
              new List<Point>
              {
                new Point(0, 3),
                new Point(0, 4),
                new Point(1, 3),
                new Point(2, 3),
                new Point(2, 4)
              }
            },
            // End row 1
            // Begin row 2
            {
              new Point(2,0),
              new List<Point>
              {
                new Point(1, 0),
                new Point(1, 1),
                new Point(2, 1)
              }
            },
            {
              new Point(2,1),
              new List<Point>
              {
                new Point(1, 0),
                new Point(1, 1),
                new Point(1, 2),
                new Point(2, 0),
                new Point(2, 2)
              }
            },
            {
              new Point(2,2),
              new List<Point>
              {
                new Point(1, 1),
                new Point(1, 2),
                new Point(1, 3),
                new Point(2, 1),
                new Point(2, 3)
              }
            },
            {
              new Point(2,3),
              new List<Point>
              {
                new Point(1, 2),
                new Point(1, 3),
                new Point(1, 4),
                new Point(2, 2),
                new Point(2, 4)
              }
            },
            {
              new Point(2,4),
              new List<Point>
              {
                new Point(1, 3),
                new Point(1, 4),
                new Point(2, 3)
              }
            },
          }
          // End row 2
        };
      }
    }
  }
}
