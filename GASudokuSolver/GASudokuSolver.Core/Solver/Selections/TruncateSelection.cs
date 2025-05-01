using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Selections;

public class TruncateSelection : ISelection
{
	public List<List<Gene>> Select(List<Individual> population, int count, IComparer<Individual> comparer)
	{
		var parents = population
			.OrderDescending(comparer).Take(count)
			.Select(individual => individual.CloneGenes())
			.ToList();
		return parents;
	}
}
