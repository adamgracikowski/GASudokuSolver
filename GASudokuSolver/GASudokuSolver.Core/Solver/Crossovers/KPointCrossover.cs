using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Crossovers;

public sealed class KPointCrossover : ICrossover
{
	public int NumberOfPoints;

	public KPointCrossover(int k)
	{
		NumberOfPoints = k;
	}

	public void Crossover(List<List<Gene>> parents, List<Individual> population)
	{
		var parentsCount = parents.Count;
		var geneCount = parents[0].Count;
		var childrenCount = population.Count;
		var halfChildrenCount = childrenCount / 2;

		var numberOfPoints = Math.Min(NumberOfPoints, geneCount - 1);

		Parallel.For(0, (childrenCount + 1) / 2, (i, state) =>
		{

			SortedSet<int> points = new SortedSet<int>();
			while (points.Count < numberOfPoints)
			{
				points.Add(Random.Shared.Next(1, geneCount));
			}

			var parentA = parents[i % parentsCount];
			var parentB = parents[(i + 1) % parentsCount];

			var geneIndex = 0;
			foreach (var point in points)
			{
				for (; geneIndex < point; geneIndex++)
				{
					population[i].Genes[geneIndex].Copy(parentA[geneIndex]);
					if (i + halfChildrenCount < childrenCount)
						population[i + halfChildrenCount].Genes[geneIndex].Copy(parentB[geneIndex]);
				}
				(parentA, parentB) = (parentB, parentA);
			}
			for (; geneIndex < geneCount; geneIndex++)
			{
				population[i].Genes[geneIndex].Copy(parentA[geneIndex]);
				if (i + halfChildrenCount < childrenCount)
					population[i + halfChildrenCount].Genes[geneIndex].Copy(parentB[geneIndex]);
			}
		});
	}
}
