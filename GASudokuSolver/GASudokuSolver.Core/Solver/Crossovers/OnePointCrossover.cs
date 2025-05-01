using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Crossovers;

public class OnePointCrossover : ICrossover
{
	public void Crossover(List<List<Gene>> parents, List<Individual> population)
	{
		var parentsCount = parents.Count;
		var geneCount = parents[0].Count;
		var halfGeneCount = geneCount / 2;
		var childrenCount = population.Count;

		Parallel.For(0, (childrenCount+1)/2, (i, state) =>
		{
			var parentA = parents[i % parentsCount];
			var parentB = parents[(i+1) % parentsCount];

			var point = Random.Shared.Next(geneCount - 1) + 1;
			var geneIndex = 0;
			for (; geneIndex < point; geneIndex++)
			{
				population[i].Genes[geneIndex].Copy(parentA[geneIndex]);
				if(i+halfGeneCount < childrenCount)
					population[i + halfGeneCount].Genes[geneIndex].Copy(parentB[geneIndex]);
			}
			for(; geneIndex < geneCount; geneIndex++)
			{
				population[i].Genes[geneIndex].Copy(parentB[geneIndex]);
				if (i + halfGeneCount < childrenCount)
					population[i + halfGeneCount].Genes[geneIndex].Copy(parentA[geneIndex]);
			}

		});
	}
}
