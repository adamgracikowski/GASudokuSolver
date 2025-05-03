using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Loading.Puzzles;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver;
using GASudokuSolver.GUI.Controls;
using GASudokuSolver.GUI.Enums;
using GASudokuSolver.GUI.Models;
using GASudokuSolver.GUI.Windows;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using GASudokuSolver.Core.Solver;
using GASudokuSolver.Core.Solver.Crossovers;
using GASudokuSolver.Core.Solver.Mutations;
using GASudokuSolver.Core.Solver.FitnessFunctions;
using GASudokuSolver.Core.Solver.Representations;
using GASudokuSolver.Core.Solver.Selections;
using LiveChartsCore.Kernel;
using SkiaSharp;
using LiveChartsCore.Kernel.Sketches;
using System.ComponentModel;
using System.Windows.Input;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Drawing;

namespace GASudokuSolver.GUI;

public partial class MainWindow : Window
{
	private readonly Stopwatch Stopwatch = new();
	private readonly System.Timers.Timer Timer = new(50);

	private CancellationTokenSource? TokenSource;

	public Sudoku? Sudoku { get; set; }
	public ObservableCollection<SudokuCell> Board { get; set; } = [];

	public SudokuSolver Solver { get; set; }

	public ObservableCollection<ISeries> FitnessSeries { get; set; }
	public ObservableCollection<ChartPointData> ChartPointsColection { get; set; } = [];

	private ChartPointData? selectedPoint = null;
	private object SelectedLock = new();

	public Axis[] XAxes { get; set; } =
	{
		new Axis
		{
			Name = "Generation",
			Labeler = value => value.ToString("N0")
		}
	};

	public Axis[] YAxes { get; set; } =
	{
		new Axis
		{
			Name = "Fitness",
			Labeler = value => value.ToString("N2")
		}
	};


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
		board.Clear();

		var cells = Enumerable
			.Range(0, Constants.Grid.Cells)
			.Select(i => new SudokuCell
			{
				Row = i / Constants.Grid.Rows,
				Column = i % Constants.Grid.Columns,
			})
			.ToList();

		cells.ForEach(board.Add);
	}
	public void InitializeChart()
	{
		var fitnessSeries = new LineSeries<ChartPointData>
		{
			Values = ChartPointsColection,
			Mapping = (data, _) =>
			{
				var point = new Coordinate(data.ProgressData.Generation, data.ProgressData.FitnessValue);
				return point;
			},
			ScalesYAt = 0,
			Stroke = new SolidColorPaint(new SKColor(33, 150, 243, 255), 2),
			Fill = new SolidColorPaint(SKColors.AliceBlue),
			GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
			GeometryStroke = new SolidColorPaint(SKColors.LightBlue),
			GeometrySize = 6,
			Pivot = -1e30,
			EnableNullSplitting = false,
			LineSmoothness = 0.2,
		};

		fitnessSeries.OnPointMeasured((point) =>
		{
			if (point.Visual is null || point.Model is null)
				return;
			point.Visual.Fill = point.Model.Selected ?
			new SolidColorPaint(SKColors.Red) :
			new SolidColorPaint(SKColors.AliceBlue);

			point.Visual.Stroke = point.Model.Selected ?
			new SolidColorPaint(SKColors.Red) :
			new SolidColorPaint(SKColors.LightBlue);
		});

		FitnessSeries = new ObservableCollection<ISeries>
		{
			fitnessSeries
		};

		GeneticChart.UpdaterThrottler = TimeSpan.FromMilliseconds(100);
		GeneticChart.DataPointerDown += GeneticChartDataClick;

		XAxes[0].PropertyChanged += AxisXRangeChanged;
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

	public static void LoadBoard(byte[,] source, ObservableCollection<SudokuCell> destination, bool updateMutable = false, byte[,]? solved = null)
	{
		for (var row = 0; row < Constants.Grid.Rows; ++row)
		{
			for (var col = 0; col < Constants.Grid.Columns; ++col)
			{
				var index = row * Constants.Grid.Rows + col;
				var cellValue = source[row, col];

				if (updateMutable)
				{
					destination[index].Mutable = cellValue == Constants.Cell.EmptyValue;
				}

				destination[index].Value =
					cellValue == Constants.Cell.EmptyValue
					? null
					: cellValue;

				if (solved is not null)
				{
					destination[index].CorrectValue = solved[row, col];
				}
			}
		}
	}

	public void ClearResults()
	{
		ChartPointsColection.Clear();
		TimeText.Text = @"00:00:00:000";
		FitnessText.Text = "0";
		GenerationText.Text = "0";
		selectedPoint = null;
		XAxes[0].MinLimit = null;
		XAxes[0].MaxLimit = null;
	}

	private async void StartButtonClickAsync(object sender, RoutedEventArgs e)
	{
		if (Sudoku is null) return;

		ClearResults();

		MainMenu.IsAlgorithmRunning = true;
		ShowButton(ButtonType.Stop);
		AlgorithmResultsGrid.Visibility = Visibility.Visible;

		var progress = new Progress<AlgorithmProgressData>(data =>
		{
			ChartPointsColection.Add(new ChartPointData(progressData: data));

			lock (SelectedLock)
			{
				if(selectedPoint is null)
				{
					FitnessText.Text = data.FitnessValue.ToString("F4");
					GenerationText.Text = data.Generation.ToString();

					LoadBoard(data.Board, Board);
				}
			}
		});

		TokenSource = new CancellationTokenSource();

		var solver = BuildSolver();

		Stopwatch.Restart();
		Timer.Start();

		var bestResult = await Task.Run(
			() => solver.Run(progress: progress, cancellationToken: TokenSource.Token)
		);

		Stopwatch.Stop();
		Timer.Stop();

		MainMenu.IsAlgorithmRunning = false;
		TokenSource = null;
		ShowButton(ButtonType.Clear);

		ShowBestResultWindow(bestResult.BestIndividual);
	}

	private void ClearButtonClick(object sender, RoutedEventArgs e)
	{
		if (Sudoku is null) return;

		if (TokenSource != null && !TokenSource.IsCancellationRequested)
		{
			TokenSource.Cancel();
			ShowButton(ButtonType.Clear);
			return;
		}

		ClearResults();
		InitializeBoard(Board);
		LoadBoard(Sudoku.Unsolved.Data, Board, updateMutable: true, Sudoku.Solved.Data);
		ShowButton(ButtonType.Start);
	}

	private SudokuSolver BuildSolver()
	{
		var solverSettings = SolverControl.ViewModel;

		return new SudokuSolver(
			Sudoku!,
			solverSettings.PopulationSize,
			solverSettings.NumberOfParents,
			MutationControl.SelectedMutation,
			SelectionControl.SelectedSelection,
			CrossoverControl.SelectedCrossover,
			FitnessFunctionControl.SelectedFitnessFunction,
			RepresentationControl.SelectedRepresentation,
			solverSettings.MaxGenerations,
			solverSettings.MaxTimeSpanMinutes
		);
	}

	private void ShowBestResultWindow(AlgorithmProgressData bestResult)
	{
		if (Sudoku is null) return;

		var bestBoard = new ObservableCollection<SudokuCell>();

		InitializeBoard(bestBoard);

		LoadBoard(Sudoku.Unsolved.Data, bestBoard, updateMutable: true, Sudoku.Solved.Data);
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

	private void GeneticChartDataClick(IChartView chart, IEnumerable<ChartPoint> points)
	{
		var mousePosition = Mouse.GetPosition(GeneticChart);
		points = GeneticChart.GetPointsAt(new LvcPoint(mousePosition.X, mousePosition.Y),
			LiveChartsCore.Measure.TooltipFindingStrategy.CompareAll);
		if (points is null || !points.Any()) return;
		var point = points.First();
		var generation = point.Index;
		var chartPointForGeneration = ChartPointsColection[generation];
		var progressData = chartPointForGeneration.ProgressData;

		lock (SelectedLock)
		{
			if (selectedPoint != chartPointForGeneration)
			{
				if(selectedPoint is not null)
				{
					selectedPoint.Selected = false;
				}

				selectedPoint = chartPointForGeneration;
				selectedPoint.Selected = true;

				LoadBoard(progressData.Board, Board);
				FitnessText.Text = progressData.FitnessValue.ToString("F4");
				GenerationText.Text = progressData.Generation.ToString();
			}
			else
			{
				chartPointForGeneration.Selected = false;
				selectedPoint = null; 
				XAxes[0].MaxLimit = null;

				progressData = ChartPointsColection.Last().ProgressData;
				LoadBoard(progressData.Board, Board);
				FitnessText.Text = progressData.FitnessValue.ToString("F4");
				GenerationText.Text = progressData.Generation.ToString();
			}
			GeneticChart.InvalidateVisual();
		}
	}

	private void AxisXRangeChanged(object? sender, PropertyChangedEventArgs e)
	{
		Axis xAxis = XAxes[0];
		if (e.PropertyName is (nameof(xAxis.MaxLimit)) or (nameof(xAxis.MinLimit)))
		{
			if (xAxis.MinLimit < 0)
			{
				xAxis.MinLimit = null;
			}
			if (ChartPointsColection.Count > 0 && xAxis.MaxLimit > ChartPointsColection.Last().ProgressData.Generation)
			{
				xAxis.MaxLimit = null;
			}
		}
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
				InitializeBoard(Board);
				LoadBoard(Sudoku.Unsolved.Data, Board, updateMutable: true, Sudoku.Solved.Data);

				ShowButton(ButtonType.Start);
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

	private void ShowButton(ButtonType buttonType)
	{
		switch (buttonType)
		{
			case ButtonType.Start:
				StartButtonCard.Visibility = Visibility.Visible;
				ClearButtonCard.Visibility = Visibility.Collapsed;
				break;
			case ButtonType.Clear:
				ClearButton.Content = nameof(ButtonType.Clear);
				StartButtonCard.Visibility = Visibility.Collapsed;
				ClearButtonCard.Visibility = Visibility.Visible;
				break;
			case ButtonType.Stop:
				ClearButton.Content = nameof(ButtonType.Stop);
				StartButtonCard.Visibility = Visibility.Collapsed;
				ClearButtonCard.Visibility = Visibility.Visible;
				break;
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

	protected override void OnClosed(EventArgs e)
	{
		Timer.Elapsed -= TimerElapsed;
		Timer.Stop();
		Timer.Dispose();

		base.OnClosed(e);
	}
}