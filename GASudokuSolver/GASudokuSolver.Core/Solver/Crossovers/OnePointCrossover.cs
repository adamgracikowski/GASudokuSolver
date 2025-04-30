using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Crossovers;

public class OnePointCrossover : ICrossover
{
	public List<Individual> Crossover(List<Individual> parents, int childrenCount)
	{
		List<Individual> children = new List<Individual>(childrenCount);
		int parentsCount = parents.Count;
		var geneCount = parents[0].Genes.Count;
		int parentIndex = 0;

		for(int childIndex = 0; childIndex < childrenCount; childIndex++)
		{
			var parentA = parents[parentIndex];
			parentIndex = (parentIndex + 1) % parentsCount;
			var parentB = parents[parentIndex];

			
			var point = Random.Shared.Next(geneCount - 1)+1;

			var newGenesChildA = parentA.Genes.Take(point).ToList();
			var newGenesChildB = parentB.Genes.Take(point).ToList();

			newGenesChildA.AddRange(parentB.Genes.Skip(point));
			newGenesChildB.AddRange(parentA.Genes.Skip(point));
			
			children.Add(new Individual(parentA, newGenesChildA));
			childIndex++;
			if(childIndex >= childrenCount)
				break;
			children.Add(new Individual(parentB, newGenesChildB));
		}

		return children;
	}
}
