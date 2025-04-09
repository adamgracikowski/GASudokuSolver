using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Validators.Contracts;

public interface IGridValidator
{
	public bool IsValidUnsolved(Grid unsolved);

	public bool IsValidSolved(Grid solved, Grid unsolved);
}