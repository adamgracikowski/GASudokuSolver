namespace GASudokuSolver.Core.Solver.Interfaces;

/// <summary>
/// Defines a crossover strategy for combining parent <see cref="Individual"/>s to produce offspring.
/// </summary>
public interface ICrossover
{
	/// <summary>
	/// Performs crossover on a list of parent <see cref="Individual"/>s to produce a specified number of children.
	/// </summary>
	/// <param name="parents">The list of parent <see cref="Individual"/>s used in the crossover.</param>
	/// <param name="childrenCount">The number of children to generate.</param>
	/// <returns>A list of newly created child <see cref="Individual"/>.</returns>
	List<Individual> Crossover(List<Individual> parents, int childrenCount);
}
