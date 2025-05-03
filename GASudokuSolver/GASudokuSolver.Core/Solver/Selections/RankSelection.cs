using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Selections;

public sealed class RankSelection : ISelection
{
	public List<List<Gene>> Select(
		List<Individual> population,
		int count,
		IComparer<Individual> comparer)
	{
		var n = population.Count;

		// best has rank n, worst has rank 1
		var sorted = population.OrderByDescending(ind => ind, comparer).ToList();
		
		var totalRank = (n * (n + 1)) / 2.0;

		var parents = new List<List<Gene>>(count);
		
		for (var i = 0; i < count; i++)
		{
			var pick = Random.Shared.NextDouble() * totalRank;
			var cumulative = 0.0;

			for (var rank = 0; rank < n; rank++)
			{
				cumulative += (n - rank);

				if (cumulative >= pick)
				{
					parents.Add(sorted[rank].CloneGenes());
					break;
				}
			}
		}

		return parents;
	}
}