using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Validators.Puzzles;

namespace GASudokuSolver.Core.Validators.Datasets;

public sealed class DatasetValidator : IDatasetValidator
{
	private readonly ISudokuValidator sudokuValidator;

	public DatasetValidator(ISudokuValidator? sudokuValidator = null)
	{
		this.sudokuValidator = sudokuValidator ?? new SudokuValidator();
	}

	public void Validate(Dictionary<Difficulty, List<Sudoku>> dataset)
	{
		foreach(var (difficultyLevel, puzzles) in dataset)
		{
			Console.WriteLine($" {difficultyLevel}:");

			var counter = 0;

			foreach (var puzzle in puzzles)
			{
				Console.Write($"  {counter}: ");

				var isValid = sudokuValidator.IsValid(puzzle);

				Console.WriteLine(isValid ? "Passed" : "Failed");

				counter++;
			}
		}
	}
}