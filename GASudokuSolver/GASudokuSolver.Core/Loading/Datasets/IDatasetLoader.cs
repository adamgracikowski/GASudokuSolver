﻿using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Loading.Datasets;

public interface IDatasetLoader
{
	Task<Dictionary<Difficulty, List<Sudoku>>> LoadDatasetAsync(string path, CancellationToken cancellationToken = default);
}