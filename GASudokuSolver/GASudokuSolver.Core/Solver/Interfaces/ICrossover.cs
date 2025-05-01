using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Interfaces;

/// <summary>
/// Defines a crossover strategy for combining parent <see cref="Individual"/>s to produce offspring.
/// </summary>
public interface ICrossover
{
	/// <summary>
	/// Performs crossover on the provided parent genes and modifies the given population of <see cref="Individual"/>s,
	/// turning them into children generated from the parent genes.
	/// </summary>
	/// <param name="parentsGenes">The list of genes representing the parents.</param>
	/// <param name="population">
	/// The list of <see cref="Individual"/>s to be modified in-place.
	/// After the method executes, these individuals will contain genes resulting from the crossover.
	/// </param>
	void Crossover(List<List<Gene>> parentsGenes, List<Individual> population);
}
