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

		for (var i = 0; i < SudokuConstants.Grid.NumberOfRows; i++)
		{
			for(var j = 0; j < SudokuConstants.Grid.NumberOfColumns; j++)
			{
				if (solved.Data[i, j] != unsolved.Data[i, j] && 
					unsolved.Data[i, j] != SudokuConstants.Cell.EmptyValue)
				{
					return false;
				}
			}
		}

		for (var i = 0; i < SudokuConstants.Grid.NumberOfRows; i++)
		{
			if (!IsValidGroup(solved.GetRow(i)) ||
				!IsValidGroup(solved.GetColumn(i)) ||
				!IsValidGroup(solved.GetSubgrid(i)))
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

		for (var i = 0; i < SudokuConstants.Grid.NumberOfRows; i++)
		{
			if (!IsValidGroup(unsolved.GetRow(i), skipEmptyCells: true) ||
				!IsValidGroup(unsolved.GetColumn(i), skipEmptyCells: true) ||
				!IsValidGroup(unsolved.GetSubgrid(i), skipEmptyCells: true))
			{
				return false;
			}
		}

		return true;
	}

	private static bool HasValidDimensions(Grid grid)
	{
		return grid.Data.GetLength(0) == SudokuConstants.Grid.NumberOfRows &&
			grid.Data.GetLength(1) == SudokuConstants.Grid.NumberOfColumns;
	}

	private static bool IsValidGroup(int[] group, bool skipEmptyCells = false)
	{
		var seen = new HashSet<int>();

		foreach (var number in group)
		{
			if (skipEmptyCells && number == SudokuConstants.Cell.EmptyValue) 
				continue;

			if (number < SudokuConstants.Cell.MinValue || 
				number > SudokuConstants.Cell.MaxValue ||
				!seen.Add(number))
			{
				return false;
			}
		}

		return true;
	}
}
