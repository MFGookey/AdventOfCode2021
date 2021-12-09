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

		private bool[,] _visited;
		private readonly object _visitedLock = new object();

		public static readonly Func<int, int> BasicRiskRule = new Func<int, int>(
				x => x+1
			);

		public static readonly Func<IEnumerable<int>, int> BasinRiskRule = new Func<IEnumerable<int>, int>(
				basinSizes =>
					basinSizes
					.OrderByDescending(s => s)
					.Take(3)
					.Aggregate(
						1,
						(left, right) => left * right
					)
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

		private IEnumerable<PointValue> FindMinimumPointValues()
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
					.Select(o => new PointValue(o.rowIndex, o.columnIndex, o.value))
			)
			.SelectMany(p => p);
		}

    public IEnumerable<int> FindLocalMinimums()
    {
			return FindMinimumPointValues()
			// Return just the values
			.Select(
				p => p.Value
			);
		}

		public IEnumerable<int> FindRiskLevels(Func<int, int> rule)
		{
			return FindLocalMinimums().Select(value => rule(value));
		}

		public IEnumerable<int> FindBasinSizes()
		{
			var minimums = FindMinimumPointValues();

			// for each minimum, look at adjacent points on the plot
			// if any are greater than the current spoit and less than 9, include them.
			// only orthogonal spots need to be checked
			// recursively do this for every spot you add.
			// include spots of equal value if they are above or to the left of you
			return minimums.Select(m => ScorePoint(m.Row, m.Column));
		}

		public int ScorePoint(int row, int column)
		{
			lock (_visitedLock)
			{
				_visited = new bool[Plot.Count(), Plot.First().Count()];
				return ScorePointRecursively(row, column);
			}
		}

		private int ScorePointRecursively(int row, int column)
		{
			var value = Plot[row][column];
			

			// Value of 9 or greater is considered not part of any basin
			if (value >= 9 || _visited[row, column])
			{
				return 0;
			}

			_visited[row, column] = true;

			var score = 1; // count myself
			
			// Score any points "above" this one if they are of higher or equal value
			if (row > 0 && Plot[row - 1][column] >= value)
			{
				score += ScorePointRecursively(row - 1, column);
			}

			// Score any points "to the left" of this one if they are of higher or equal value
			if (column > 0 && Plot[row][column - 1] >= value)
			{
				score += ScorePointRecursively(row, column - 1);
			}

			// Score any points "below" this one if they are of higher value
			if (row + 1 < Plot.Count() && Plot[row + 1][column] > value)
			{
				score += ScorePointRecursively(row + 1, column);
			}

			// Score any points "to the right" of this one if they are of higher value
			if (column + 1 < Plot[row].Count() && Plot[row][column + 1] > value)
			{
				score += ScorePointRecursively(row, column + 1);
			}

			return score;
		}

		public int FindBasinAggregateRisk()
		{
			return DepthPlot.BasinRiskRule(FindBasinSizes());
		}
  }
}
