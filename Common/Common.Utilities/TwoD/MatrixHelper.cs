using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Common.Utilities.TwoD
{
  public class MatrixHelper
  {
    public static IReadOnlyDictionary<Point, IReadOnlyList<Point>> GenerateNeighborMaps(int rows, int columns)
    {
      // rows and columns must be greater than 0
      if (rows <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(rows), "Rows must be greater than 0");
      }

      if (columns <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(columns), "Columns must be greater than 0");
      }

			// Given a matrix's dimensions, for each point p, list each point adjacent to p

			var rowRange = Enumerable.Range(0, rows);
			var columnRange = Enumerable.Range(0, columns);

			return rowRange.SelectMany(
				row => columnRange
					.Select(
						column => new KeyValuePair<Point, IReadOnlyList<Point>>(
							new Point(row, column),
							Enumerable
								.Range(
									Math.Max(0, row - 1),
									3
								)
								.Where(
									r =>
										r < rowRange.Count()
										&& r <= row + 1
								)
								.SelectMany(
									r => Enumerable
												.Range(
													Math.Max(0, column - 1),
													3
												)
												.Where(
													c =>
														c < columnRange.Count()
														&& c <= column + 1
												)
												.Select(
													c => new Point(r, c)
												)

								)
								.Where(coordinate => coordinate.Row != row || coordinate.Column != column)
								.ToList()
						)
					)
			)
			.ToDictionary(
				kvp => kvp.Key,
				kvp => kvp.Value
			);
		}
  }
}
