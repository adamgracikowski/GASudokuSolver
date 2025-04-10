using GASudokuSolver.Core.Enums;

namespace GASudokuSolver.Core.Models;

public sealed class Sudoku
{
	public readonly Difficulty	DifficultyLevel;

	public readonly Grid Solved;
	public readonly Grid Unsolved;

	public Sudoku(Difficulty difficultyLevel, Grid unsolved, Grid solved)
	{
		DifficultyLevel = difficultyLevel;
		Unsolved = unsolved;
		Solved = solved;
	}
}