using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.FitnessFunctions;

public sealed class EquallyPunishedConflictFitnessFunction : IFitnessFunction
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
		var isNumberInCollumn = new bool[Constants.Grid.Columns, Constants.Cell.MaxValue + 1];
		var isNumberInSubgrid = new bool[Constants.Grid.Subgrids, Constants.Cell.MaxValue + 1];

		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for(var col = 0; col < Constants.Grid.Columns; col++)
			{
				int numberInCell = sudoku.Data[row, col];

				if (isNumberInRow[row, numberInCell]) fitness -= 1.0;
				isNumberInRow[row, numberInCell] = true;

				if(isNumberInCollumn[col, numberInCell]) fitness -= 1.0;
				isNumberInCollumn[col, numberInCell] = true;

				var subgrid = row / Constants.Subgrid.Rows * Constants.Grid.SubgridsInRow + col / Constants.Subgrid.Columns;
				
				if (isNumberInSubgrid[subgrid, numberInCell]) fitness -= 1.0;
				isNumberInSubgrid[subgrid, numberInCell] = true;
			}
		}

		return fitness;
	}

	public bool IsSolved(double fitness)
		=> fitness == 0;
}