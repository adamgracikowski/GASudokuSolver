using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.FitnessFunctions;

public sealed class RisingConflictPenaltyFitnessFunction : IFitnessFunction
{
	public int Compare(Individual? x, Individual? y)
		=> x == null || y == null ? 0
		   : x.Fitness.CompareTo(y.Fitness);

	public bool CompareFitness(double lhs, double rhs)
		=> lhs > rhs;

	public double Eveluate(Grid sudoku)
	{
		var fitness = 0.0;

		var isNumberInRow = new bool[Constants.Grid.Rows, Constants.Cell.MaxValue + 1];
		var isNumberInColumn = new bool[Constants.Grid.Columns, Constants.Cell.MaxValue + 1];
		var isNumberInSubgrid = new bool[Constants.Grid.Subgrids, Constants.Cell.MaxValue + 1];

		var conflictsInRows = new int[Constants.Grid.Rows];
		var conflictsInColumns = new int[Constants.Grid.Columns];
		var conflictsInSubgrids = new int[Constants.Grid.Subgrids];

		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				int numberInCell = sudoku.Data[row, col];

				if (isNumberInRow[row, numberInCell]) 
				{
					conflictsInRows[row]++;
					fitness -= 1.0*conflictsInRows[row];
				}
				isNumberInRow[row, numberInCell] = true;

				if (isNumberInColumn[col, numberInCell])
				{
					conflictsInColumns[col]++;
					fitness -= 1.0 * conflictsInColumns[col];
				}
				isNumberInColumn[col, numberInCell] = true;

				var subgrid = row / Constants.Subgrid.Rows * Constants.Grid.SubgridsInRow + col / Constants.Subgrid.Columns;

				if (isNumberInSubgrid[subgrid, numberInCell])
				{
					conflictsInSubgrids[subgrid]++;
					fitness -= 1.0 * conflictsInSubgrids[subgrid];
				}
				isNumberInSubgrid[subgrid, numberInCell] = true;
			}
		}

		return fitness;
	}

	public bool IsSolved(double fitness)
		=> fitness == 0;
}
