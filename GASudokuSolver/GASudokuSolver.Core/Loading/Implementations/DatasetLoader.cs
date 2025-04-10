using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Loading.Contracts;
using GASudokuSolver.Core.Loading.Exceptions;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Implementations;

public sealed class DatasetLoader : IDatasetLoader
{
	private readonly ISudokuLoader sudokuLoader;

	public DatasetLoader(ISudokuLoader? sudokuLoader = null)
	{
		this.sudokuLoader = sudokuLoader ?? new CsvSudokuLoader();
	}

	public async Task<Dictionary<Difficulty, List<Sudoku>>> LoadDatasetAsync(
		string path, 
		CancellationToken cancellationToken = default)
	{
		var existingDifficulties = Enum
			.GetValues(typeof(Difficulty))
			.Cast<Difficulty>()
			.Where(level => Directory.Exists(Path.Combine(path, level.ToString())))
			.ToList();

		var tasks = existingDifficulties.Select(async difficulty =>
		{
			var puzzles = await LoadDifficultyLevelAsync(path, difficulty, cancellationToken);
			
			return new KeyValuePair<Difficulty, List<Sudoku>>(difficulty, puzzles);
		});

		var results = await Task.WhenAll(tasks);

		return results.ToDictionary(kv => kv.Key, kv => kv.Value);
	}

	private async Task<List<Sudoku>> LoadDifficultyLevelAsync(string path, Difficulty difficulty, CancellationToken cancellationToken = default)
	{
		try
		{
			var directory = Path.Combine(path, difficulty.ToString());

			var files = Directory.GetFiles(directory);

			var tasks = files.Select(file =>
				LoadPuzzlesFromFileAsync(file, difficulty, cancellationToken)
			);

			var results = await Task.WhenAll(tasks);

			return [.. results.SelectMany(result => result)];
		}
		catch (GridLoadException ex)
		{
			throw new DatasetLoadException($"Failed to load dataset from {path}", ex);
		}
	}

	private async Task<List<Sudoku>> LoadPuzzlesFromFileAsync(
		string path, 
		Difficulty difficulty, 
		CancellationToken cancellationToken = default)
	{
		var records = await File.ReadAllLinesAsync(path, cancellationToken);

		var tasks = records.Select(record =>
			this.sudokuLoader.LoadSudokuFromStringAsync(record, difficulty, cancellationToken)
		);

		var puzzles = await Task.WhenAll(tasks);

		return [.. puzzles];
	}
}