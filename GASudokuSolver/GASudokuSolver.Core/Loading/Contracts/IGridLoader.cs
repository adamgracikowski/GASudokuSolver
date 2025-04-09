using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Contracts;

public interface IGridLoader
{
	Task<Grid> LoadGridFromFileAsync(string path);
}