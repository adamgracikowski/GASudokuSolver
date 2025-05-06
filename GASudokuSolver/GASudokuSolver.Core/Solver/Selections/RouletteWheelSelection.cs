using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Selections;

public sealed class RouletteWheelSelection : ISelection
{
	public List<List<Gene>> Select(
		List<Individual> population,
		int count,
		IComparer<Individual> comparer)
	{
		if (population == null || population.Count == 0)
			throw new ArgumentException("Population must not be empty", nameof(population));

		var minFitness = population.Min(ind => ind.Fitness);
		var offset = minFitness < 0 ? -minFitness : 0;

		var weights = population
			.Select(ind => ind.Fitness + offset)
			.ToArray();

		var totalWeight = weights.Sum();

		// if totalWeight is zero, fall back to uniform random selection
		if (totalWeight <= 0)
		{
			return new UniformRandomSelection().Select(population, count, comparer);
		}

		var parents = new List<List<Gene>>(count);

		Parallel.For(0, count, (i, _) =>
		{
			var pick = Random.Shared.NextDouble() * totalWeight;
			var cumulative = 0.0;

			for (var j = 0; j < population.Count; j++)
			{
				cumulative += weights[j];

				if (cumulative >= pick)
				{
					parents.Add(population[j].CloneGenes());
					break;
				}
			}
		});

		return parents;
	}
}