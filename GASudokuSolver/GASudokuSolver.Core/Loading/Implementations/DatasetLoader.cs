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
		this.sudokuLoader = sudokuLoader ?? new SudokuLoader();
	}

	public async Task<Dictionary<DifficultyLevel, List<Sudoku>>> LoadDatasetAsync(string path)
	{
		var existingLevelDifficulties = Enum
			.GetValues(typeof(DifficultyLevel))
			.Cast<DifficultyLevel>()
			.Where(level => Directory.Exists(Path.Combine(path, level.ToString())))
			.ToList();

		var dataset = new Dictionary<DifficultyLevel, List<Sudoku>>();

		foreach (var difficultyLevel in existingLevelDifficulties)
		{
			dataset[difficultyLevel] = await LoadDifficultyLevelAsync(path, difficultyLevel);
		}

		return dataset;
	}

	private async Task<List<Sudoku>> LoadDifficultyLevelAsync(string path, DifficultyLevel difficultyLevel)
	{
		var directory = Path.Combine(path, difficultyLevel.ToString());

		var solvedDirectory = Path.Combine(directory, SolvedStatus.Solved.ToString());
		var unsolvedDirectory = Path.Combine(directory, SolvedStatus.Unsolved.ToString());

		if (!Directory.Exists(solvedDirectory))
		{
			throw new DatasetLoadException($"Missing {SolvedStatus.Solved} directory in {path}");
		}

		if (!Directory.Exists(unsolvedDirectory))
		{
			throw new DatasetLoadException($"Missing {SolvedStatus.Unsolved} directory in {path}");
		}

		var puzzles = new List<Sudoku>();

		var solvedFiles = Directory.GetFiles(solvedDirectory);

		try
		{
			foreach (var solvedFile in solvedFiles)
			{
				var unsolvedFile = Path.Combine(unsolvedDirectory, Path.GetFileName(solvedFile));

				var puzzle = await sudokuLoader.LoadSudokuFromFilesAsync(unsolvedFile, solvedFile, difficultyLevel);

				puzzles.Add(puzzle);
			}

			return puzzles;
		}
		catch (GridLoadException ex)
		{
			throw new DatasetLoadException($"Failed to load dataset from {path}", ex);
		}
	}
}