using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.FitnessFunctions;

public sealed class WeightedConflictFitnessFunction : IFitnessFunction
{
	private readonly double _rowPenalty, _columnPenalty, _subgridPenalty;

	public WeightedConflictFitnessFunction(
		double rowPenalty = 1.0,
		double colPenalty = 1.0,
		double subgridPenalty = 1.0)
	{
		_rowPenalty = rowPenalty;
		_columnPenalty = colPenalty;
		_subgridPenalty = subgridPenalty;
	}

	public int Compare(Individual? x, Individual? y)
		=> x == null || y == null ? 0
		   : x.Fitness.CompareTo(y.Fitness);

	public bool CompareFitness(double lhs, double rhs)
		=> lhs > rhs;

	public bool IsSolved(double fitness)
		=> Math.Abs(fitness) < 1e-6;

	public double Eveluate(Grid sudoku)
	{
		var fitness = 0.0;

		var isNumberInRow = new bool[Constants.Grid.Rows, Constants.Cell.MaxValue + 1];
		var isNumberInCollumn = new bool[Constants.Grid.Columns, Constants.Cell.MaxValue + 1];
		var isNumberInSubgrid = new bool[Constants.Grid.Subgrids, Constants.Cell.MaxValue + 1];

		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				int numberInCell = sudoku.Data[row, col];

				if (isNumberInRow[row, numberInCell]) fitness -= _rowPenalty;
				isNumberInRow[row, numberInCell] = true;

				if (isNumberInCollumn[col, numberInCell]) fitness -= _columnPenalty;
				isNumberInCollumn[col, numberInCell] = true;

				var subgrid = row / Constants.Subgrid.Rows * Constants.Grid.SubgridsInRow + col / Constants.Subgrid.Columns;

				if (isNumberInSubgrid[subgrid, numberInCell]) fitness -= _subgridPenalty;
				isNumberInSubgrid[subgrid, numberInCell] = true;
			}
		}

		return fitness;
	}
}