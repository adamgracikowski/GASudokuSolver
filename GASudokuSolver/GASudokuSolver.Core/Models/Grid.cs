using GASudokuSolver.Core.Configurations;

namespace GASudokuSolver.Core.Models;

public sealed class Grid
{
	public readonly int[,] Data;

	public Grid(int[,] data)
	{
		Data = data;
	}

	public int[] GetRow(int row)
	{
		var result = new int[SudokuConstants.Grid.NumberOfColumns];
		for (var col = 0; col < SudokuConstants.Grid.NumberOfColumns; col++)
		{
			result[col] = Data[row, col];
		}

		return result;
	}

	public int[] GetColumn(int col)
	{
		var result = new int[SudokuConstants.Grid.NumberOfRows];
		for (var row = 0; row < SudokuConstants.Grid.NumberOfRows; row++)
		{
			result[row] = Data[row, col];
		}

		return result;
	}

	public int[] GetSubgrid(int subgrid)
	{
		var result = new int[SudokuConstants.Subgrid.NumberOfRows * SudokuConstants.Subgrid.NumberOfColumns];
		
		var startRow = (subgrid / SudokuConstants.Subgrid.NumberOfRows) * SudokuConstants.Subgrid.NumberOfRows;
		var startCol = (subgrid % SudokuConstants.Subgrid.NumberOfColumns) * SudokuConstants.Subgrid.NumberOfColumns;
		
		var i = 0;

		for (var row = startRow; row < startRow + SudokuConstants.Subgrid.NumberOfRows; row++)
		{
			for (var col = startCol; col < startCol + SudokuConstants.Subgrid.NumberOfColumns; col++)
			{
				result[i++] = Data[row, col];
			}
		}

		return result;
	}
}