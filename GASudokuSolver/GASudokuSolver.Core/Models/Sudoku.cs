using GASudokuSolver.Core.Enums;

namespace GASudokuSolver.Core.Models;

public sealed class Sudoku
{
	public readonly DifficultyLevel	DifficultyLevel;

	public readonly Grid Solved;
	public readonly Grid Unsolved;

	public Sudoku(DifficultyLevel difficultyLevel, Grid unsolved, Grid solved)
	{
		DifficultyLevel = difficultyLevel;
		Unsolved = unsolved;
		Solved = solved;
	}
}