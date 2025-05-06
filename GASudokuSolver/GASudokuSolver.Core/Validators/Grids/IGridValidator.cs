using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Validators.Grids;

public interface IGridValidator
{
	public bool IsValidUnsolved(Grid unsolved);

	public bool IsValidSolved(Grid solved, Grid unsolved);
}