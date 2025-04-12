using System.Collections.ObjectModel;
using System.Windows;

namespace GASudokuSolver.GUI;

public partial class MainWindow : Window
{
	public Func<double, string> AxisLabelFormatter { get; set; } = value => value.ToString("N2");

	public ObservableCollection<SudokuCell> SudokuCells { get; set; } = [];

	public void InitializeBoard()
	{
		SudokuCells = [.. Enumerable.Range(0, 81).Select(i => new SudokuCell
			{
				Row = i / 9,
				Column = i % 9,
				Value = Random.Shared.Next(1, 9)
			})];
	}



	public MainWindow()
	{
		InitializeComponent();
		InitializeBoard();

		DataContext = this;
	}
}

public class SudokuCell
{
	public int Row { get; set; }
	public int Column { get; set; }
	public int? Value { get; set; }
}