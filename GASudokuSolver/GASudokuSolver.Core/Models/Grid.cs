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
		var result = new int[Constants.Grid.Columns];
		for (var col = 0; col < Constants.Grid.Columns; col++)
		{
			result[col] = Data[row, col];
		}

		return result;
	}

	public int[] GetColumn(int col)
	{
		var result = new int[Constants.Grid.Rows];
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			result[row] = Data[row, col];
		}

		return result;
	}

	public int[] GetSubgrid(int subgrid)
	{
		var result = new int[Constants.Subgrid.Rows * Constants.Subgrid.Columns];
		
		var startRow = (subgrid / Constants.Subgrid.Rows) * Constants.Subgrid.Rows;
		var startCol = (subgrid % Constants.Subgrid.Columns) * Constants.Subgrid.Columns;
		
		var counter = 0;

		for (var row = startRow; row < startRow + Constants.Subgrid.Rows; row++)
		{
			for (var col = startCol; col < startCol + Constants.Subgrid.Columns; col++)
			{
				result[counter++] = Data[row, col];
			}
		}

		return result;
	}
}