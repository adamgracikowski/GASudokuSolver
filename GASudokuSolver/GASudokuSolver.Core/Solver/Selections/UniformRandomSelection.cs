using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Selections;

public sealed class UniformRandomSelection : ISelection
{
	public List<List<Gene>> Select(List<Individual> population, int count, IComparer<Individual> compare)
	{
		var parents = new List<List<Gene>>(count);

		for (var i = 0; i < count; i++)
		{
			var randomIndividual = population[Random.Shared.Next(population.Count)];
			parents.Add(randomIndividual.CloneGenes());
		}

		return parents;
	}
}