using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Puzzles;

public interface ISudokuLoader
{
	Task<Sudoku> LoadSudokuFromStringAsync(
		string content, 
		Difficulty difficulty, 
		CancellationToken cancellationToken = default
	);
}