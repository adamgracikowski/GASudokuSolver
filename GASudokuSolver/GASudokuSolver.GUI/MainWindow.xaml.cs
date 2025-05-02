using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Loading.Puzzles;
using GASudokuSolver.GUI.Controls;
using GASudokuSolver.GUI.Models;
using GASudokuSolver.GUI.Windows;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Events;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using GASudokuSolver.Core.Solver;
using GASudokuSolver.Core.Solver.Crossovers;
using GASudokuSolver.Core.Solver.Mutations;
using GASudokuSolver.Core.Solver.FitnessFunctions;
using GASudokuSolver.Core.Solver.Representations;
using GASudokuSolver.Core.Solver.Selections;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.GUI;

public partial class MainWindow : Window
{
	private readonly Stopwatch Stopwatch = new();
	private readonly System.Timers.Timer Timer = new(50);

	public Sudoku? Sudoku { get; set; }
	public ObservableCollection<SudokuCell> Board { get; set; } = [];

	public SudokuSolver Solver { get; set; }

	public SeriesCollection FitnessSeries { get; set; }
	public ChartValues<ChartPointData> ChartPointsColection { get; set; } = [];
	
	public ChartPointData? Selected = null;
	private object SelectedLock = new();

	public Func<double, string> YAxisLabelFormatter { get; set; } = value => value.ToString("N2");
	public Func<double, string> XAxisLabelFormatter { get; set; } = value => value.ToString("N0");

#pragma warning disable CS8618
	public MainWindow()
#pragma warning restore CS8618
	{
		InitializeComponent();

		InitializeBoard(Board);
		InitializeChart();
		InitializeTimer();

		DataContext = this;
	}

	public static void InitializeBoard(ObservableCollection<SudokuCell> board)
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

		cells.ForEach(board.Add);
	}
	public void InitializeChart()
	{
		FitnessSeries =
		[
			new LineSeries
			{
				Title = "Fitness",
				Values = ChartPointsColection,
				Configuration = new CartesianMapper<ChartPointData>()
					.X(point => point.ProgressData.Generation)
					.Y(point => point.ProgressData.FitnessValue)
					.Stroke(point => point.Selected ? Brushes.Red : Brushes.LightBlue)
					.Fill(point => point.Selected ? Brushes.Red : null),
			}
		];

		GeneticChart.DisableAnimations = true;
	}
	public void InitializeTimer()
	{
		Timer.Elapsed += TimerElapsed;
	}
	private void TimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
	{
		try
		{
			Dispatcher.Invoke(() =>
			{
				if (TimeText != null)
				{
					TimeText.Text = Stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff");
				}
			});
		}
		catch
		{
			// Window is probably closing, ignore
		}
	}

	public static void LoadBoard(byte[,] source, ObservableCollection<SudokuCell> destination, bool updateReadOnly = false)
	{
		for (var row = 0; row < Constants.Grid.Rows; ++row)
		{
			for (var col = 0; col < Constants.Grid.Columns; ++col)
			{
				var cellValue = source[row, col];
				if (updateReadOnly)
				{
					destination[row * Constants.Grid.Rows + col].ReadOnly =
						cellValue == Constants.Cell.EmptyValue;
				}
				destination[row * Constants.Grid.Rows + col].Value =
					cellValue == Constants.Cell.EmptyValue
					? null
					: cellValue;
			}
		}
	}

	public void ClearResults()
	{
		ChartPointsColection.Clear();
		TimeText.Text = @"00:00:00:000";
		FitnessText.Text = "0";
		GenerationText.Text = "0";
		Selected = null;
	}

	private async void StartButtonClickAsync(object sender, RoutedEventArgs e)
	{
		if (Sudoku is null) return;

		ClearResults();

		MainMenu.IsAlgorithmRunning = true;
		StartButton.IsEnabled = false;
		AlgorithmResultsGrid.Visibility = Visibility.Visible;

		var progress = new Progress<AlgorithmProgressData>(data =>
		{
			ChartPointsColection.Add(new ChartPointData(progressData: data));

			lock (SelectedLock)
			{
				if(Selected is null)
				{
					FitnessText.Text = data.FitnessValue.ToString("F4");
					GenerationText.Text = data.Generation.ToString();

					LoadBoard(data.Board, Board);
				}
			}
		});

		var selection = SelectionControl.SelectedSelection;
		var mutation = MutationControl.SelectedMutation;

		Stopwatch.Restart();
		Timer.Start();

		Solver = new SudokuSolver(Sudoku, 10000, 10,
			mutation,
			selection,
			new OnePointCrossover(),
			new EquallyPunishedConflictFitnessFunction(),
			new SingleCellRowCollumnRepresentation(),
			1000,
			TimeSpan.FromMinutes(1)
		);

		var bestResult = await Task.Run(
			() => Solver.Run(progress: progress)
		);

		Stopwatch.Stop();
		Timer.Stop();

		MainMenu.IsAlgorithmRunning = false;

		ShowBestResultWindow(bestResult.BestIndividual);
	}

	private void ShowBestResultWindow(AlgorithmProgressData bestResult)
	{
		if (Sudoku is null) return;

		var bestBoard = new ObservableCollection<SudokuCell>();

		InitializeBoard(bestBoard);

		LoadBoard(Sudoku.Unsolved.Data, bestBoard, updateReadOnly: true);
		LoadBoard(bestResult.Board, bestBoard);

		var viewModel = new BestResultViewModel(
			board: bestBoard,
			currentFitness: bestResult.FitnessValue.ToString("F4"),
			currentGeneration: bestResult.Generation.ToString()
		);

		var bestResultWindow = new BestResultWindow(viewModel)
		{
			Owner = this
		};

		bestResultWindow.Show();
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

			if (Sudoku is not null)
			{
				ClearResults();
				LoadBoard(Sudoku.Unsolved.Data, Board, updateReadOnly: true);
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
		var chartPointForGeneration = ChartPointsColection[generation];
		var progressData = chartPointForGeneration.ProgressData;

		lock (SelectedLock)
		{
			if (Selected != chartPointForGeneration)
			{
				if (Selected is not null) Selected.Selected = false;

				chartPointForGeneration.Selected = true;
				Selected = chartPointForGeneration;

				LoadBoard(progressData.Board, Board);
				FitnessText.Text = progressData.FitnessValue.ToString("F4");
				GenerationText.Text = progressData.Generation.ToString();
			}
			else
			{
				chartPointForGeneration.Selected = false;
				Selected = null;
				GeneticChart.AxisX[0].MaxValue = double.NaN;
			}
		}
	}

	private void AxisXRangeChanged(RangeChangedEventArgs eventArgs)
	{
		Axis axis = (Axis)eventArgs.Axis;
		if (axis.MinValue < 0)
		{
			axis.MinValue = 0;
		}
		if (ChartPointsColection.Count > 0 && axis.MaxValue > ChartPointsColection.Last().ProgressData.Generation)
		{
			axis.MaxValue = double.NaN;
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		Timer.Elapsed -= TimerElapsed;
		Timer.Stop();
		Timer.Dispose();

		base.OnClosed(e);
	}
}