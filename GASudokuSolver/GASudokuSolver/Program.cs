using GASudokuSolver.Core.Loading.Implementations;
using GASudokuSolver.Core.Validators.Implementations;

namespace GASudokuSolver;

public sealed class Program
{
	static async Task Main(string[] args)
	{
		var datasetLoader = new DatasetLoader();
		var datasetValidator = new DatasetValidator();

		Console.WriteLine("Loading dataset...");
		var dataset = await datasetLoader.LoadDatasetAsync(path: "Datasets/Csv");

		Console.WriteLine("Validating dataset...");
		datasetValidator.Validate(dataset);
		Console.ReadKey();
	}
}
