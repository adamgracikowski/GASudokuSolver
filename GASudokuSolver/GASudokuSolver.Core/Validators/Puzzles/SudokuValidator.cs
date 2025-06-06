﻿using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Validators.Grids;

namespace GASudokuSolver.Core.Validators.Puzzles;

public sealed class SudokuValidator : ISudokuValidator
{
	private readonly IGridValidator gridValidator;

	public SudokuValidator(IGridValidator? gridValidator = null)
	{
		this.gridValidator = gridValidator ?? new GridValidator();
	}

	public bool IsValid(Sudoku sudoku)
	{
		return gridValidator.IsValidUnsolved(sudoku.Unsolved) &&
			gridValidator.IsValidSolved(sudoku.Solved, sudoku.Unsolved);
	}
}