using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmokeInTheWater.Core
{
  public class DepthPlot
  {
    public IReadOnlyList<IReadOnlyList<int>> Plot
    {
      get;
      private set;
    }

		public static readonly Func<int, int> BasicRule = new Func<int, int>(
				x => x+1
			);

    public DepthPlot(IEnumerable<string> depthReadings)
    {
      // depthReadings may not be null
      if (depthReadings == null)
      {
        throw new ArgumentException("Depth readings may not be null", nameof(depthReadings));
      }

      // depthReadings must be rectangular
      if (
        depthReadings.Any() == false
        || depthReadings.Select(row => string.IsNullOrEmpty(row) ? 0 : row.Length).Distinct().Count() != 1)
      {
        throw new ArgumentException("Each row of depth readings must be the same length", nameof(depthReadings));
      }

      // Depth readings must be all numeric
      if (depthReadings.Any(r => Regex.IsMatch(r, @"[^\d]")))
      {
        throw new ArgumentException("Only integers are allowed in depth readings", nameof(depthReadings));
      }

      Plot = depthReadings
        .Select(
          row => row
            .Select(
              c => int.Parse(c.ToString())
            )
            .ToList()
        )
        .ToList();
    }

    public IEnumerable<int> FindLocalMinimums()
    {
			// for every depth reading in the plot
			// check the neighbors (for all that exist)
			// and if all of them are greater than the
			// current reading then return the reading
			return Plot.Select(
				(row, rowIndex) =>
					row.Select(
						(c, columnIndex) =>
							new
							{
								// Here we create ranges of cells to check
								rowIndex = rowIndex,
								columnIndex = columnIndex,
								value = c,
								columnChecks = Enumerable
									.Range(
										Math.Max(0, columnIndex - 1),
										3
									)
									.Where(
										r =>
											r < row.Count()
											&& r <= columnIndex + 1
									),
								rowChecks = Enumerable
									.Range(
										Math.Max(0, rowIndex - 1),
										3
									)
									.Where(
										r =>
											r < Plot.Count()
											&& r <= rowIndex + 1
									)
							}
					)
					.Select(
						// Here we actually check those ranges against the current contextual cell's value
						o => new
						{
							localMinimum = o.rowChecks
								.SelectMany(
									r => o.columnChecks,
									(r, c) => new
									{
										row = r,
										column = c,
										plotValue = Plot[r][c]
									}
								)
								.Where(
									// exclude the cell we are checking from the list of its neighbors
									cell =>
										(
											cell.row != o.rowIndex
											|| cell.column != o.columnIndex
										)
										// We only care about if any neighbors are less than the cell we are checking
										&& cell.plotValue < o.value
								)
								.Any() == false,
							o.rowIndex,
							o.columnIndex,
							o.value
						}
					)
					// We only care about if the cell is a local minimum
					.Where(o => o.localMinimum)
			)
			// Flatten into a single enumerable of just the values
			.SelectMany(
				x =>
					x.Select(o => o.value)
			);
		}

		public IEnumerable<int> FindRiskLevels(Func<int, int> rule)
		{
			return FindLocalMinimums().Select(value => rule(value));
		}
  }
}
