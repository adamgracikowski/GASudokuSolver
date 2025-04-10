using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Validators.Contracts;

namespace GASudokuSolver.Core.Validators.Implementations;

public sealed class GridValidator : IGridValidator
{
	public bool IsValidSolved(Grid solved, Grid unsolved)
	{
		if (!HasValidDimensions(unsolved))
		{
			return false;
		}

		for (var row = 0; row < Constants.Grid.Rows; ++row)
		{
			for(var col = 0; col < Constants.Grid.Columns; ++col)
			{
				if (solved.Data[row, col] != unsolved.Data[row, col] && 
					unsolved.Data[row, col] != Constants.Cell.EmptyValue)
				{
					return false;
				}
			}
		}

		for (var row = 0; row < Constants.Grid.Rows; ++row)
		{
			if (!IsValidGroup(solved.GetRow(row)))
			{
				return false;
			}
		}

		for (var col = 0; col < Constants.Grid.Columns; ++col)
		{
			if (!IsValidGroup(solved.GetColumn(col)))
			{
				return false;
			}
		}

		for (var subgrid = 0; subgrid < Constants.Grid.Subgrids; ++subgrid)
		{
			if (!IsValidGroup(solved.GetSubgrid(subgrid)))
			{
				return false;
			}
		}

		return true;
	}

	public bool IsValidUnsolved(Grid unsolved)
	{
		if (!HasValidDimensions(unsolved))
		{
			return false;
		}

		for (var row = 0; row < Constants.Grid.Rows; ++row)
		{
			if (!IsValidGroup(unsolved.GetRow(row), skipEmptyCells: true))
			{
				return false;
			}
		}

		for (var col = 0; col < Constants.Grid.Columns; ++col)
		{
			if (!IsValidGroup(unsolved.GetColumn(col), skipEmptyCells: true))
			{
				return false;
			}
		}

		for (var subgrid = 0; subgrid < Constants.Grid.Subgrids; ++subgrid)
		{
			if (!IsValidGroup(unsolved.GetSubgrid(subgrid), skipEmptyCells: true))
			{
				return false;
			}
		}

		return true;
	}

	private static bool HasValidDimensions(Grid grid)
	{
		return grid.Data.GetLength(0) == Constants.Grid.Rows &&
			grid.Data.GetLength(1) == Constants.Grid.Columns;
	}

	private static bool IsValidGroup(int[] group, bool skipEmptyCells = false)
	{
		var seen = new HashSet<int>();

		foreach (var number in group)
		{
			if (skipEmptyCells && number == Constants.Cell.EmptyValue) 
				continue;

			if (number < Constants.Cell.MinValue || 
				number > Constants.Cell.MaxValue ||
				!seen.Add(number))
			{
				return false;
			}
		}

		return true;
	}
}
