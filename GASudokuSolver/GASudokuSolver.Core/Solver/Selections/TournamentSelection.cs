﻿using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Genes;
using System.Collections.Concurrent;

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
		var parentsBag = new ConcurrentBag<List<Gene>>();

		Parallel.For(0, count, i =>
		{
			var tourney = Enumerable
				.Range(0, _tournamentSize)
				.Select(_ => population[Random.Shared.Next(population.Count)])
				.ToList();

			var winner = tourney
				.OrderByDescending(ind => ind, comparer)
				.First();

			parentsBag.Add(winner.CloneGenes());
		});

		return [.. parentsBag];
	}
}