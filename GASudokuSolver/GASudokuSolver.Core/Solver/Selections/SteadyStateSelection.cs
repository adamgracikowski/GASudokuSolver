using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Selections;

public class SteadyStateSelection : ISelection
{
	public List<Individual> Select(List<Individual> population, int count, IComparer<Individual> comparer)
	{
		var parents = population
			.Order(comparer).Take(count)
			.ToList();
		return parents;
	}
}
