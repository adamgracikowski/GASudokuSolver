using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Selections;

public sealed class TournamentSelection : ISelection
{
	private readonly int _tournamentSize;

	public TournamentSelection(int tournamentSize)
	{
		_tournamentSize = Math.Max(2, tournamentSize);
	}

	public List<List<Gene>> Select(
		List<Individual> population,
		int count,
		IComparer<Individual> comparer)
	{
		var parents = new List<List<Gene>>(count);

		for (var i = 0; i < count; i++)
		{
			var tourney = Enumerable
				.Range(0, _tournamentSize)
				.Select(_ => population[Random.Shared.Next(population.Count)])
				.ToList();

			var winner = tourney
				.OrderByDescending(ind => ind, comparer)
				.First();

			parents.Add(winner.CloneGenes());
		}

		return parents;
	}
}