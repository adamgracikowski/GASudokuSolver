using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Validators.Contracts;

public interface ISudokuValidator
{
	public bool IsValid(Sudoku sudoku);
}
