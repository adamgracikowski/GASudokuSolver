using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Loading.Puzzles;
using GASudokuSolver.Core.Models;
using GASudokuSolver.GUI.Models;
using GASudokuSolver.GUI.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace GASudokuSolver.GUI;

public partial class MainWindow : Window
{
	private readonly Stopwatch Stopwatch = new();
	private readonly System.Timers.Timer Timer = new(50);

	public Sudoku? Sudoku { get; set; }
	public ObservableCollection<SudokuCell> Board { get; set; } = [];
	public List<AlgorithmProgressData> ProgressDataColection { get; set; } = [];
	public ChartValues<double> FitnessValues { get; set; } = [];
	public SeriesCollection FitnessSeries { get; set; }

	public Func<double, string> AxisLabelFormatter { get; set; } = value => value.ToString("N2");

#pragma warning disable CS8618
	public MainWindow()
#pragma warning restore CS8618
	{
		InitializeComponent();

		InitializeBoard();
		InitializeChart();
		InitializeTimer();

		DataContext = this;
	}

	public void InitializeBoard()
	{
		var cells = Enumerable
			.Range(0, Constants.Grid.Cells)
			.Select(i => new SudokuCell
			{
				Row = i / Constants.Grid.Rows,
				Column = i % Constants.Grid.Columns,
				Value = null,
			})
			.ToList();

		Board = [.. cells];
	}
	public void InitializeChart()
	{
		FitnessSeries =
		[
			new LineSeries
			{
				Title = "Fitness",
				Values = FitnessValues,
				//PointGeometry = null
			}
		];
	}
	public void InitializeTimer()
	{
		Timer.Elapsed += (s, e) =>
		{
			Dispatcher.Invoke(() =>
			{
				TimeText.Text = Stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
			});
		};
	}
	
	public void LoadBoard(int[,] board, bool updateReadOnly = false)
	{
		for (var row = 0; row < Constants.Grid.Rows; ++row)
		{
			for (var col = 0; col < Constants.Grid.Columns; ++col)
			{
				var cellValue = board[row, col];
				if (updateReadOnly)
				{
					Board[row * Constants.Grid.Rows + col].ReadOnly = 
						cellValue == Constants.Cell.EmptyValue;
				}
				Board[row * Constants.Grid.Rows + col].Value =
					cellValue == Constants.Cell.EmptyValue
					? null
					: cellValue;
			}
		}
	}

	public void ClearResults()
	{
		FitnessValues.Clear();
		ProgressDataColection.Clear();
		TimeText.Text = @"00:00:00:000";
		FitnessText.Text = "0";
		GenerationText.Text = "0";
	}

	private async void StartButtonClickAsync(object sender, RoutedEventArgs e)
	{
		if (Sudoku is null) return;

		ClearResults();
		StartButton.IsEnabled = false;

		AlgorithmResultsGrid.Visibility = Visibility.Visible;

		var progress = new Progress<AlgorithmProgressData>(data =>
		{
			FitnessText.Text = data.FitnessValue.ToString("F4");
			GenerationText.Text = data.Generation.ToString();
			
			ProgressDataColection.Add(data);
			FitnessValues.Add(data.FitnessValue);
			
			LoadBoard(data.Board);
		});

		Stopwatch.Restart();
		Timer.Start();

		await Task.Run(() => GeneticAlgorithmMock.Run(Sudoku.Unsolved.Data, progress));

		Stopwatch.Stop();
		Timer.Stop();
	}

	private void SaveBoardCsvClick(object sender, RoutedEventArgs e)
	{
		// TODO
	}

	private async void LoadBoardCsvClickAsync(object sender, RoutedEventArgs e)
	{
		var initialDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datasets", "Csv"));
		var openFileDialog = new OpenFileDialog
		{
			Title = "Select a Sudoku CSV File",
			Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
			InitialDirectory = initialDirectory,
			Multiselect = false
		};

		if (openFileDialog.ShowDialog() != true) return;

		try
		{
			var filename = openFileDialog.FileName;

			using var reader = new StreamReader(filename);

			var line = await reader.ReadLineAsync() ?? string.Empty;

			var sudokuLoader = new CsvSudokuLoader();

			Sudoku = await sudokuLoader.LoadSudokuFromStringAsync(line, Difficulty.Unknown);

			if(Sudoku is not null)
			{
				ClearResults();
				LoadBoard(Sudoku.Unsolved.Data, updateReadOnly: true);
				StartButton.IsEnabled = true;
			}

		}
		catch (Exception ex)
		{
			MessageBox.Show(
				$"Error while loading the Sudoku: {ex.Message}", 
				"Error", 
				MessageBoxButton.OK, 
				MessageBoxImage.Error
			);
		}
	}

	private void ExitClick(object sender, RoutedEventArgs e)
	{
		Application.Current.Shutdown();
	}

	private void AboutClick(object sender, RoutedEventArgs e)
	{
		var aboutWindow = new AboutWindow
		{
			Owner = this
		};

		aboutWindow.ShowDialog();
	}

	private void UserGuideClick(object sender, RoutedEventArgs e)
	{
		// TODO
	}

	private void GeneticChartDataClick(object sender, ChartPoint chartPoint)
	{
		var generation = (int)chartPoint.X;
		var progressData = ProgressDataColection[generation];

		LoadBoard(progressData.Board);
		FitnessText.Text = progressData.FitnessValue.ToString("F4");
		GenerationText.Text = progressData.Generation.ToString();
	}
}