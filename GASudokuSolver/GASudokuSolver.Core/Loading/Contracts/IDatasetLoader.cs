using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Contracts;

public interface IDatasetLoader
{
	Task<Dictionary<DifficultyLevel, List<Sudoku>>> LoadDatasetAsync(string path);
}