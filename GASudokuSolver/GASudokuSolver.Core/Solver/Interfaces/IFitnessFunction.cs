using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Solver.Interfaces;

/// <summary>
/// Defines a fitness function used to evaluate Sudoku solutions and determine optimization criteria.
/// </summary>
public interface IFitnessFunction : IComparer<Individual>
{
	/// <summary>
	/// Calculates the fitness value of the given Sudoku grid.
	/// A higher value typically represents a better (more complete or valid) solution.
	/// </summary>
	/// <param name="sudoku">The Sudoku grid to evaluate.</param>
	/// <returns>The computed fitness value.</returns>
	double Eveluate(Grid sudoku);

	/// <summary>
	/// Compares two fitness values and determines which one is better based on the optimization goal.
	/// </summary>
	/// <param name="lhsFitnessValue">The first fitness value to compare.</param>
	/// <param name="rhsFitnessValue">The second fitness value to compare.</param>
	/// <returns><c>true</c> if the first fitness value is considered better; otherwise, <c>false</c>.</returns>
	bool CompareFitness(double lhsFitnessValue, double rhsFitnessValue);

	/// <summary>
	/// Determines whether the given fitness value represents a completely solved Sudoku board.
	/// </summary>
	/// <param name="fitness">The fitness value to check.</param>
	/// <returns><c>true</c> if the solution is considered solved; otherwise, <c>false</c>.</returns>
	bool IsSolved(double fitness);
}
