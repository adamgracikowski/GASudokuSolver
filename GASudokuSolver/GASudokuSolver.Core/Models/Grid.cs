using GASudokuSolver.Core.Configurations;
using System.Windows.Controls;

namespace GASudokuSolver.Core.Models;

public sealed class Grid
{
	public int[,] Data;
	public bool[,] Mutable;

	public Grid(int[,] data)
	{
		Data = data;
		SetMutables();
	}

	public Grid(Grid grid)
	{
		Data = new int[Constants.Grid.Rows, Constants.Grid.Columns];
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for(var col = 0; col < Constants.Grid.Columns; col++)
			{
				Data[row, col] = grid.Data[row, col];
			}
		}
		SetMutables();
	}

	public void SetMutables()
	{
		Mutable = new bool[Data.GetLength(0), Data.GetLength(1)];
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				Mutable[row, col] = Data[row, col] == Constants.Cell.EmptyValue;
			}
		}
	}

	public int[] GetRow(int row)
	{
		var result = new int[Constants.Grid.Columns];
		for (var col = 0; col < Constants.Grid.Columns; ++col)
		{
			result[col] = Data[row, col];
		}

		return result;
	}

	public int[] GetColumn(int col)
	{
		var result = new int[Constants.Grid.Rows];
		for (var row = 0; row < Constants.Grid.Rows; ++row)
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

		for (var row = startRow; row < startRow + Constants.Subgrid.Rows; ++row)
		{
			for (var col = startCol; col < startCol + Constants.Subgrid.Columns; ++col)
			{
				result[counter++] = Data[row, col];
			}
		}

		return result;
	}
}