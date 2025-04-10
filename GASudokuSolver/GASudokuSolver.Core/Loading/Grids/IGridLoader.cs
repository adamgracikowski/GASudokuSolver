using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Grids;

public interface IGridLoader
{
	Task<Grid> LoadGridFromStringAsync(string content, CancellationToken cancellationToken);
}