using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Contracts;

public interface ISudokuLoader
{
	Task<Sudoku> LoadSudokuFromFilesAsync(string unsolvedPath, string solvedPath, DifficultyLevel difficultyLevel);
}