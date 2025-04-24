using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Interfaces;
using System.Threading;

namespace GASudokuSolver.Core.Solver;
class SudokuSolver
{
	private int generation;
	private TimeSpan maxTime = TimeSpan.FromMinutes(
		Constants.Solver.DefaultMaxTimeInMinutes);
	private int maxGenerations;
	private int populationSize;

	public List<Individual> Population;
	public List<Individual> BestIndividuals;

	public IMutation Mutation;
	public ISelection Selection;
	public ICrossover Crossover;

	public IFitnessFunction FitnessFunction;


	public int Generation
	{
		get { return generation; }
	}


	public SudokuSolver(
		Sudoku sudoku,
		int populationSize,
		IMutation mutation,
		ISelection selection,
		ICrossover crossover,
		IFitnessFunction fitnessFunction,
		IRepresentation representation,
		int maxGenerations = Constants.Solver.DefaultMaxGenerations,
		TimeSpan? maxTime = null
	)
	{
		this.maxGenerations = maxGenerations;
		this.generation = 0;
		if (maxTime != null)
		{
			maxTime = maxTime.Value;
		}

		Population = new List<Individual>(populationSize);
		for (var individual = 0; individual < populationSize; individual++)
		{
			Population[individual] = new Individual(representation, sudoku.Unsolved);
		}

		Mutation = mutation;
		Selection = selection;
		Crossover = crossover;
		FitnessFunction = fitnessFunction;
	}

	public Task Run()
	{
		using (var cancellationTokenSource = new CancellationTokenSource(maxTime))
		{
			var token = cancellationTokenSource.Token;

			while (generation < maxGenerations)
			{
				generation++; 

				foreach(var individual in Population)
				{
					individual.UpdateBoard();
					individual.Evaluate(FitnessFunction);
				}

				var bestIndividual = Population.Aggregate(
					(best, current) =>
						FitnessFunction.Compare(best.Fitness, current.Fitness) ? best : current
				);
				BestIndividuals.Add(bestIndividual);

				// tutaj pewnie ten progres, ale nie wiem jak chcesz to robić,
				// może przydałoby się AlgorithmProgressData dać do core

				if(FitnessFunction.IsSolved(bestIndividual.Fitness))
				{
					return Task.CompletedTask;
				}

				if (token.IsCancellationRequested)
				{
					// można dodać jakieś info
					return Task.FromCanceled(token);
				}

				// Te liczby /2 też do zmiany jakoś nie?
				var Parents = Selection.Select(Population, populationSize / 2, FitnessFunction.Compare);
				Population = Crossover.Crossover(Parents, populationSize).ToList();
			}
		}

		return Task.CompletedTask;
	}
}
