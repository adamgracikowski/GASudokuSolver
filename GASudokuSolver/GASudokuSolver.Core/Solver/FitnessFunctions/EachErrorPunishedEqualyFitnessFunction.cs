using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.FitnessFunctions;

public class EachErrorPunishedEqualyFitnessFunction : IFitnessFunction
{
	public int Compare(Individual? x, Individual? y)
	{
		if(x == null || y == null)
		{
			return 0;
		}
		return x.Fitness.CompareTo(y.Fitness);
	}

	public bool CompareFitness(double lhsFitnessValue, double rhsFitnessValue)
	{
		return lhsFitnessValue > rhsFitnessValue;
	}

	public double Eveluate(Grid sudoku)
	{
		double fitness = 0.0;

		bool[,] isNumberInRow = new bool[Constants.Grid.Rows, Constants.Cell.MaxValue + 1];
		bool[,] isNumberInCollumn = new bool[Constants.Grid.Columns, Constants.Cell.MaxValue + 1];
		bool[,] isNumberInSubgrid = new bool[Constants.Grid.Subgrids, Constants.Cell.MaxValue + 1];

		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for(var col = 0; col < Constants.Grid.Columns; col++)
			{
				int numberInCell = sudoku.Data[row, col];

				if (isNumberInRow[row, numberInCell])
				{
					fitness -= 1.0;
				}
				isNumberInRow[row, numberInCell] = true;

				if(isNumberInCollumn[col, numberInCell])
				{
					fitness -= 1.0;
				}
				isNumberInCollumn[col, numberInCell] = true;

				int subgrid = row / Constants.Subgrid.Rows * Constants.Grid.SubgridsInRow + col / Constants.Subgrid.Columns;
				if (isNumberInSubgrid[subgrid, numberInCell])
				{
					fitness -= 1.0;
				}
				isNumberInSubgrid[subgrid, numberInCell] = true;
			}
		}

		return fitness;
	}

	public bool IsSolved(double fitness)
	{
		return fitness == 0;
	}
}
