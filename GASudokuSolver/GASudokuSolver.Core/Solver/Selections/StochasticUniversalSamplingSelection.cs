using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Selections;

public sealed class StochasticUniversalSamplingSelection : ISelection
{
	public List<List<Gene>> Select(
		List<Individual> population,
		int count,
		IComparer<Individual> comparer)
	{
		if (population == null || population.Count == 0)
			throw new ArgumentException("Population must not be empty", nameof(population));

		var fitnesses = population.Select(ind => ind.Fitness).ToArray();
		var minFitness = fitnesses.Min();
		var offset = minFitness < 0 ? -minFitness : 0;

		var weights = fitnesses.Select(f => f + offset).ToArray();
		var totalWeight = weights.Sum();

		var parents = new List<List<Gene>>(count);

		// if all weights are zero, fall back to uniform random selection
		if (totalWeight <= 0)
		{
			return new UniformRandomSelection().Select(population, count, comparer);
		}

		var pointerSpacing = totalWeight / count;
		var start = Random.Shared.NextDouble() * pointerSpacing;
		var pointers = Enumerable
			.Range(0, count)
			.Select(i => start + i * pointerSpacing)
			.ToArray();

		var cumulative = 0.0;
		var idx = 0;

		foreach (var pointer in pointers)
		{
			while (idx < population.Count && cumulative + weights[idx] < pointer)
			{
				cumulative += weights[idx];
				idx++;
			}

			parents.Add(population[idx].CloneGenes());
		}

		return parents;
	}
}