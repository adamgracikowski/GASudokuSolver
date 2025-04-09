using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Loading.Contracts;
using GASudokuSolver.Core.Loading.Exceptions;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Implementations;

public sealed class CsvGridLoader : IGridLoader
{
	private readonly char recordDelimiter = '\n';
	private readonly char fieldDelimiter = ',';

	private readonly StringSplitOptions splitOptions
		= StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

	public async Task<Grid> LoadGridFromFileAsync(string path)
	{
		try
		{
			using var reader = new StreamReader(path);

			var content = await reader.ReadToEndAsync();

			var records = content.Split(recordDelimiter, splitOptions);

			if(records.Length != SudokuConstants.Grid.NumberOfRows)
			{
				throw new FormatException(
					$"Number of rows {records.Length} different from expected {SudokuConstants.Grid.NumberOfRows}."
				);
			}

			var data = new int[SudokuConstants.Grid.NumberOfRows, SudokuConstants.Grid.NumberOfColumns];

			var row = 0;

			foreach (var line in records)
			{
				var fields = line
					.Split(fieldDelimiter, splitOptions)
					.ToList();

				if(fields.Count != SudokuConstants.Grid.NumberOfColumns)
				{
					throw new FormatException($"Number of columns {fields.Count} different from expected " +
						$"({SudokuConstants.Grid.NumberOfColumns}) in row {row}.");
				}

				for(var col = 0; col < SudokuConstants.Grid.NumberOfColumns; ++col)
				{
					data[row, col] = int.Parse(fields[col]);
				}
				
				row++;
			}

			return new Grid(data: data);
		}
		catch (Exception ex)
		{
			throw new GridLoadException($"Failed to load sudoku from {path}.", ex);
		}
	}
}