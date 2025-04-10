using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Loading.Contracts;
using GASudokuSolver.Core.Loading.Exceptions;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Implementations;

public sealed class CsvSudokuLoader : ISudokuLoader
{
	private readonly char fieldDelimiter = ',';
	private readonly int fieldsCount = 2;

	private readonly StringSplitOptions splitOptions
		= StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;

	private readonly IGridLoader gridLoader;

	public CsvSudokuLoader(IGridLoader? gridLoader = null)
	{
		this.gridLoader = gridLoader ?? new CsvGridLoader();
	}

	public async Task<Sudoku> LoadSudokuFromStringAsync(
		string content, 
		Difficulty difficulty, 
		CancellationToken cancellationToken = default)
	{
		try
		{
			var fields = content.Split(this.fieldDelimiter, this.splitOptions);

			if (fields.Length != this.fieldsCount)
			{
				throw new FormatException("Invalid format for sudoku puzzle");
			}

			var unsolved = await this.gridLoader.LoadGridFromStringAsync(fields[0], cancellationToken);

			var solved = await this.gridLoader.LoadGridFromStringAsync(fields[1], cancellationToken);

			return new Sudoku(difficulty, unsolved, solved);

		}
		catch (Exception ex)
		{
			throw new SudokuLoadException("Failed to load sudoku puzzle", ex);
		}
	}
}