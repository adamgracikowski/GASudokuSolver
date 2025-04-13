using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using System.Windows;

namespace GASudokuSolver.GUI;

public partial class MainWindow : Window
{
	public SeriesCollection FitnessSeries { get; set; }
	public ChartValues<double> FittnessValues { get; set; } = new();

	public double CurrentFitness { get; set; }
	public int CurrentGeneration { get; set; }
	public ObservableCollection<string> AlgorithmResults { get; set; } = new();

	public Func<double, string> AxisLabelFormatter { get; set; } = value => value.ToString("N2");

	public ObservableCollection<SudokuCell> SudokuCells { get; set; } = [];

	public void InitializeBoard()
	{
		SudokuCells = [.. Enumerable.Range(0, 81).Select(i => new SudokuCell
			{
				Row = i / 9,
				Column = i % 9,
				Value = Random.Shared.Next(0, 9)
			})];
	}



	public MainWindow()
	{

		InitializeComponent();
		InitializeBoard();

		FitnessSeries =
		[
			new LineSeries
			{
				Title = "Fitness",
				Values = FittnessValues,
				PointGeometry = null
			}
		];

		DataContext = this;
	}

	private void StartButtonClick(object sender, RoutedEventArgs e)
	{
		AlgorithmResultsGrid.Visibility = Visibility.Visible;
	}
}

public class SudokuCell
{
	public int Row { get; set; }
	public int Column { get; set; }
	public int? Value { get; set; }
}

public sealed record AlgorithmProgressData(
	double FitnessValue, 
	int[,] Board, 
	int Generation
);

public sealed class AlgorithmMock
{
	public async Task<AlgorithmProgressData> Run(int[,] board, IProgress<AlgorithmProgressData>? progress = null) 
	{
		var generations = 300;

		var empty = new List<(int r, int c)>();

		for(var r = 0; r < board.GetLength(0); r++)
		{
			for(var c = 0; c < board.GetLength(1); c++)
			{
				if (board[r,c] == 0)
					empty.Add((r, c));
			}
		}

		for(var i = 0; i < generations; i++)
		{
			foreach(var (r, c) in empty)
			{
				board[r, c] = Random.Shared.Next(1, 9);
			}

			await Task.Delay(200);

			progress?.Report(new AlgorithmProgressData(10 * Random.Shared.NextDouble() + 1, board, i));
		}

		return new AlgorithmProgressData(10 * Random.Shared.NextDouble() + 1, board, generations);
	}
}