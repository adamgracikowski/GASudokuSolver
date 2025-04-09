using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Loading.Contracts;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Implementations;

public sealed class SudokuLoader : ISudokuLoader
{
	private readonly IGridLoader gridLoader;

	public SudokuLoader(IGridLoader? gridLoader = null)
	{
		this.gridLoader = gridLoader ?? new CsvGridLoader();
	}

	public async Task<Sudoku> LoadSudokuFromFilesAsync(
		string unsolvedPath, 
		string solvedPath, 
		DifficultyLevel difficultyLevel)
	{
		var unsolved = await gridLoader.LoadGridFromFileAsync(unsolvedPath);
		var solved = await gridLoader.LoadGridFromFileAsync(solvedPath);

		return new Sudoku(difficultyLevel, unsolved, solved); 
	}
}