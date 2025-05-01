using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver;

/// <summary>
/// Represents a genetic algorithm solver for Sudoku puzzles.
/// </summary>
public class SudokuSolver
{
	private int generation;
	private TimeSpan maxTime;
	private int maxGenerations;
	private int populationSize;
	private int numberOfParents;
	private int bestIndividualThroughtGenerationsIndex;

	public List<Individual> Population;
	public List<Individual> BestIndividuals;

	public Individual bestIndividualThroughtGenerations;

	public IMutation Mutation;
	public ISelection Selection;
	public ICrossover Crossover;

	public IFitnessFunction FitnessFunction;

	/// <summary>
	/// Gets the current generation number.
	/// </summary>
	public int Generation => generation;

	/// <summary>
	/// Initializes a new instance of the <see cref="SudokuSolver"/> class.
	/// </summary>
	/// <param name="sudoku">The unsolved <see cref="Sudoku"/> puzzle to be solved.</param>
	/// <param name="populationSize">The number of <see cref="Individual"/>s in the population.</param>
	/// <param name="numberOfParents">The number of parents selected for crossover in each generation.</param>
	/// <param name="mutation">The mutation strategy to be used.</param>
	/// <param name="selection">The selection strategy to be used.</param>
	/// <param name="crossover">The crossover strategy to be used.</param>
	/// <param name="fitnessFunction">The fitness function used to evaluate <see cref="Individual"/>s.</param>
	/// <param name="representation">The representation strategy for <see cref="Individual"/>s.</param>
	/// <param name="maxGenerations">The maximum number of generations to run the solver. Default is configured in constants.</param>
	/// <param name="maxTime">The maximum time to run the solver. Default is configured in constants.</param>
	public SudokuSolver(
		Sudoku sudoku,
		int populationSize,
		int numberOfParents,
		IMutation mutation,
		ISelection selection,
		ICrossover crossover,
		IFitnessFunction fitnessFunction,
		IRepresentation representation,
		int maxGenerations = Constants.Solver.DefaultMaxGenerations,
		TimeSpan? maxTime = null
	)
	{
		this.maxGenerations = maxGenerations > 0 ? maxGenerations : 1;
		this.generation = 0;
		this.maxTime = maxTime ?? TimeSpan.FromMinutes(
			Constants.Solver.DefaultMaxTimeInMinutes);
		this.bestIndividualThroughtGenerationsIndex = 0;

		this.populationSize = Math.Max(populationSize, 2);
		this.numberOfParents = Math.Clamp(numberOfParents, 2, this.populationSize);

		Population = new List<Individual>(this.populationSize);
		Parallel.For(0, this.populationSize, (i, state) =>
		{
			Population.Add(new Individual(representation, sudoku.Unsolved));
		});
		BestIndividuals = new List<Individual>(this.populationSize);
		bestIndividualThroughtGenerations = Population[0];

		Mutation = mutation;
		Selection = selection;
		Crossover = crossover;
		FitnessFunction = fitnessFunction;
	}

	/// <summary>
	/// Runs the genetic algorithm to solve the Sudoku puzzle.
	/// </summary>
	/// <param name="cancellationToken">Optional external cancellation token to abort the algorithm early.</param>
	/// <param name="progress">Optional progress reporter to receive intermediate results.</param>
	/// <returns>An <see cref="AlgorithmResult"/> containing the best solution found, time elapsed and termination reason.</returns>
	public AlgorithmResult Run(
		CancellationToken cancellationToken = default,
		IProgress<AlgorithmProgressData>? progress = null)
	{

		using var cancellationTokenSource = new CancellationTokenSource(maxTime);

		var timeoutToken = cancellationTokenSource.Token;
		DateTime start = DateTime.UtcNow;
		EvaluatePopulation();
		while (generation < maxGenerations)
		{

			Parallel.ForEach(Population, individual =>
			{
				individual.UpdateBoard();
				individual.Evaluate(FitnessFunction);
			});

			var bestIndividualInGeneration = EvaluatePopulation();

			progress?.Report(new AlgorithmProgressData(
				bestIndividualInGeneration,
				generation)
			);

			if (FitnessFunction.IsSolved(bestIndividualThroughtGenerations.Fitness))
			{
				return new AlgorithmResult(
					new AlgorithmProgressData(bestIndividualThroughtGenerations, generation),
					start - DateTime.UtcNow,
					TerminationReason.SoultionFound
				);
			}

			if (timeoutToken.IsCancellationRequested)
			{
				return new AlgorithmResult(
					new AlgorithmProgressData(bestIndividualThroughtGenerations, generation),
					start - DateTime.UtcNow,
					TerminationReason.Timeout
				);
			}

			if (cancellationToken.IsCancellationRequested)
			{
				return new AlgorithmResult(
					new AlgorithmProgressData(bestIndividualThroughtGenerations, generation),
					start - DateTime.UtcNow,
					TerminationReason.Cancelled
				);
			}

			var parentsGenes = Selection.Select(Population, numberOfParents, FitnessFunction);
			Crossover.Crossover(parentsGenes, Population);
			Parallel.ForEach(Population , individual =>
			{
				foreach(Gene gene in individual.Genes)
				{
					Mutation.Mutate(gene);
				}
			});
			generation++;
		}

		return new AlgorithmResult(
			new AlgorithmProgressData(bestIndividualThroughtGenerations, bestIndividualThroughtGenerationsIndex),
			start - DateTime.UtcNow,
			TerminationReason.MaxGenerationsReached
		);
	}

	private Individual EvaluatePopulation()
	{
		var bestIndividualInGeneration = Population.Max(FitnessFunction)!.Clone();
		BestIndividuals.Add(bestIndividualInGeneration);

		if (FitnessFunction.Compare(
			bestIndividualInGeneration,
			bestIndividualThroughtGenerations) > 0)
		{
			bestIndividualThroughtGenerations = bestIndividualInGeneration;
			bestIndividualThroughtGenerationsIndex = generation;
		}

		return bestIndividualInGeneration;
	}
}

