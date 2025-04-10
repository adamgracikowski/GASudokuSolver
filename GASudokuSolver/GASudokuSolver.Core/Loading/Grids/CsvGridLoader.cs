using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Loading.Exceptions;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Grids;

public sealed class CsvGridLoader : IGridLoader
{
	public async Task<Grid> LoadGridFromStringAsync(string content, CancellationToken cancellationToken)
	{
		try
		{
			if (content.Length != Constants.Grid.Cells)
			{
				throw new FormatException(
					$"Number of cells {content.Length} different from expected {Constants.Grid.Cells}."
				);
			}

			var data = new int[Constants.Grid.Rows, Constants.Grid.Columns];

			for (var row = 0; row < Constants.Grid.Rows; ++row)
			{
				for (var col = 0; col < Constants.Grid.Columns; ++col)
				{
					data[row, col] = int.Parse(content[row * Constants.Grid.Rows + col].ToString());
				}
			}

			return await Task.FromResult(new Grid(data: data));
		}
		catch (Exception ex)
		{
			throw new GridLoadingException($"Failed to load sudoku from {content}.", ex);
		}
	}
}