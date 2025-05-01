using GASudokuSolver.Core.Configurations;

namespace GASudokuSolver.Core.Models;

public sealed class Grid
{
	public byte[,] Data;
	public bool[,] Mutable;

	public Grid(byte[,] data)
	{
		Data = data;
		Mutable = new bool[Data.GetLength(0), Data.GetLength(1)];
		SetMutables();
	}

	public Grid(Grid grid)
	{
		Data = new byte[Constants.Grid.Rows, Constants.Grid.Columns];
		Mutable = new bool[Data.GetLength(0), Data.GetLength(1)];
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for(var col = 0; col < Constants.Grid.Columns; col++)
			{
				Data[row, col] = grid.Data[row, col];
				Mutable[row, col] = grid.Mutable[row, col];
			}
		}
	}

	public void SetMutables()
	{
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				Mutable[row, col] = Data[row, col] == Constants.Cell.EmptyValue;
			}
		}
	}

	public byte[] GetRow(int row)
	{
		var result = new byte[Constants.Grid.Columns];
		for (var col = 0; col < Constants.Grid.Columns; ++col)
		{
			result[col] = Data[row, col];
		}

		return result;
	}

	public byte[] GetColumn(int col)
	{
		var result = new byte[Constants.Grid.Rows];
		for (var row = 0; row < Constants.Grid.Rows; ++row)
		{
			result[row] = Data[row, col];
		}

		return result;
	}

	public byte[] GetSubgrid(int subgrid)
	{
		var result = new byte[Constants.Subgrid.Rows * Constants.Subgrid.Columns];
		
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

	public byte[,] CloneBoard()
	{
		return (byte[,])Data.Clone(); 
	}
}