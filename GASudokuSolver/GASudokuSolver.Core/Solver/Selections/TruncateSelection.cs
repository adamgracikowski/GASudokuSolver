using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Selections;

public sealed class TruncateSelection : ISelection
{
	public List<List<Gene>> Select(List<Individual> population, int count, IComparer<Individual> comparer)
	{
		if (population == null || population.Count == 0)
			throw new ArgumentException("Population must not be empty", nameof(population));

		return population
			.OrderDescending(comparer).Take(count)
			.Select(individual => individual.CloneGenes())
			.ToList();
	}
}