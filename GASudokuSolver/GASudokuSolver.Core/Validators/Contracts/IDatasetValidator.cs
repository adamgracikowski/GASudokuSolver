using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Validators.Contracts;

public interface IDatasetValidator
{
	public void Validate(Dictionary<Difficulty, List<Sudoku>> dataset);
}