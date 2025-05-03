using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;
using System.Collections.Concurrent;

namespace GASudokuSolver.Core.Solver.Selections;

public sealed class UniformRandomSelection : ISelection
{
	public List<List<Gene>> Select(List<Individual> population, int count, IComparer<Individual> compare)
	{
		var parentsBag = new ConcurrentBag<List<Gene>>();

		Parallel.For(0, count, i =>
		{
			var randomIndividual = population[Random.Shared.Next(population.Count)];
			parentsBag.Add(randomIndividual.CloneGenes());
		});

		return [.. parentsBag];
	}
}