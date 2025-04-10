using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Validators.Puzzles;

public interface ISudokuValidator
{
	public bool IsValid(Sudoku sudoku);
}
