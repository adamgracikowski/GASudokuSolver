using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Loading.Exceptions;
using GASudokuSolver.Core.Loading.Grids;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Puzzles;

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
			var fields = content.Split(fieldDelimiter, splitOptions);

			if (fields.Length != fieldsCount)
			{
				throw new FormatException("Invalid format for sudoku puzzle");
			}

			var unsolved = await gridLoader.LoadGridFromStringAsync(fields[0], cancellationToken);

			var solved = await gridLoader.LoadGridFromStringAsync(fields[1], cancellationToken);

			return new Sudoku(difficulty, unsolved, solved);

		}
		catch (Exception ex)
		{
			throw new SudokuLoadingException("Failed to load sudoku puzzle", ex);
		}
	}
}