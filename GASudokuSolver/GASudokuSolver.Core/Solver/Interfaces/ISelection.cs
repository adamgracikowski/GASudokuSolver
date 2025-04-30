namespace GASudokuSolver.Core.Solver.Interfaces;

/// <summary>
/// Defines a selection strategy for choosing <see cref="Individual"/>s from a population based on their fitness.
/// </summary>
public interface ISelection
{
	/// <summary>
	/// Selects a specified number of individuals from a population using a comparison function.
	/// </summary>
	/// <param name="population">The population of <see cref="Individual"/>s to select from.</param>
	/// <param name="count">The number of <see cref="Individual"/>s to select.</param>
	/// <param name="compare">
	/// A comparer that determines which fitness value is better.
	/// Recommended implementation: use <see cref="IFitnessFunction.Compare"/> method.
	/// </param>
	/// <returns>A list of selected <see cref="Individual"/>s.</returns>
	List<Individual> Select(
		List<Individual> population,
		int count,
		IComparer<Individual> compare
	);
}
