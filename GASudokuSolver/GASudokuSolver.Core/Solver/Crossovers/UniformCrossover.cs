using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Crossovers;

public sealed class UniformCrossover : ICrossover
{
	public void Crossover(List<List<Gene>> parents, List<Individual> population)
	{
		var parentsCount = parents.Count;
		var geneCount = parents[0].Count;
		var childrenCount = population.Count;
		var halfChildrenCount = childrenCount / 2;

		Parallel.For(0, (childrenCount + 1) / 2, (i, state) =>
		{
			var parentA = parents[i % parentsCount];
			var parentB = parents[(i + 1) % parentsCount];

			var point = Random.Shared.Next(geneCount - 1) + 1;
			for (var geneIndex = 0; geneIndex < geneCount; geneIndex++)
			{
				var parentForFirstChild = Random.Shared.Next(0, 2);

				population[i].Genes[geneIndex].Copy(parentForFirstChild == 0 ? parentA[geneIndex] : parentB[geneIndex]);
				if (i + halfChildrenCount < childrenCount)
					population[i + halfChildrenCount].Genes[geneIndex].Copy(parentForFirstChild == 0 ? parentB[geneIndex] : parentA[geneIndex]);
			}
		});
	}
}
