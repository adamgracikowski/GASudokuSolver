using CommandLine;
using CsvHelper;
using CsvHelper.Configuration;
using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Loading.Puzzles;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver;
using GASudokuSolver.Core.Solver.Crossovers;
using GASudokuSolver.Core.Solver.FitnessFunctions;
using GASudokuSolver.Core.Solver.Mutations;
using GASudokuSolver.Core.Solver.Representations;
using GASudokuSolver.Core.Solver.Selections;
using GASudokuSolver.Core.Validators.Puzzles;
using System.Globalization;

namespace GASudokuSolver;

public sealed class Program
{
	private class Options
	{
		[Option('i', "input", Required = true, HelpText = "Path to the Sudoku input file in .csv format.")]
		public string InputFilePath { get; set; } = string.Empty;

		[Option('o', "output", Required = false, HelpText = "Path to the output results file in .csv format.", Default = "results.csv")]
		public string OutputFilePath { get; set; } = string.Empty;

		[Option('r', "runs", Required = false, HelpText = "Number of times to run the algorithm.", Default = 50)]
		public int Runs { get; set; }
	}

	private class AlgorithmResultCsvRecord
	{
		public double FitnessValue { get; set; }
		public int Generation { get; set; }
		public string TerminationReason { get; set; } = string.Empty;
	}

	static async Task Main(string[] args)
	{
		await Parser.Default.ParseArguments<Options>(args)
			.MapResult(
				async opts => await RunScript(opts),
				errs =>
				{
					Console.WriteLine("Invalid command-line arguments.");
					return Task.FromResult(1);
				}
			);
	}

	private static async Task RunScript(Options options)
	{
		var sudokuLoader = new CsvSudokuLoader();

		Console.WriteLine("Loading records from the input file...");

		var records = await File.ReadAllLinesAsync(options.InputFilePath);
		
		var tasks = records.Select(record =>
			sudokuLoader.LoadSudokuFromStringAsync(record, Difficulty.Unknown)
		);

		var puzzles = await Task.WhenAll(tasks);

		Console.WriteLine($"Loaded {puzzles.Length} Sudoku puzzles...");

		if (puzzles.Length == 0)
		{
			Console.WriteLine("Finishing execution...");
			return;
		}

		Console.WriteLine("Validating Sudoku puzzles...");

		var sudokuValidator = new SudokuValidator();

		if (!puzzles.All(sudokuValidator.IsValid))
		{
			Console.WriteLine("Invalid Sudoku puzzles detected...");
			Console.WriteLine("Finishing execution...");
			return;
		}

		Console.WriteLine("All puzzles are valid...");

		var puzzle = puzzles[0];

		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			NewLine = Environment.NewLine,
		};

		using var writer = new StreamWriter(options.OutputFilePath);
		using var csv = new CsvWriter(writer, config);

		csv.WriteHeader<AlgorithmResultCsvRecord>();
		csv.NextRecord();

		for (var i = 0; i < options.Runs; i++)
		{
			Console.WriteLine($"Starting solver for the {i + 1} time...");
			
			var result = BuildSolver(puzzle).Run();
			
			Console.WriteLine("Solving finished...");
			Console.WriteLine("Saving result to the output file...");

			csv.WriteRecord(new AlgorithmResultCsvRecord()
			{
				FitnessValue = result.BestIndividual.FitnessValue,
				Generation = result.BestIndividual.Generation,
				TerminationReason = result.TerminationReason.ToString(),
			});
			csv.NextRecord();

			Console.WriteLine("The result has been saved to the output file...");
		}

		Console.WriteLine("Press any key to finish execution...");
		Console.ReadKey();
	}

	private static SudokuSolver BuildSolver(Sudoku sudoku)
	{
		return new SudokuSolver(
			sudoku,
			populationSize: 10_000,
			numberOfParents: 50,
			mutation: new PercentChanceMutation(chance: 20.0),
			selection: new TruncateSelection(),
			crossover: new KPointCrossover(k: 2),
			fitnessFunction: new WeightedConflictFitnessFunction(
				rowPenalty: 1.0,
				colPenalty: 1.0,
				subgridPenalty: 2.0),
			representation: new CandidateChoiceRepresentation(),
			maxGenerations: 1_000
		);
	}
}