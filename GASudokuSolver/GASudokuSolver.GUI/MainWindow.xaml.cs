using System.Collections.ObjectModel;
using System.Windows;

namespace GASudokuSolver.GUI;

public partial class MainWindow : Window
{
	public Func<double, string> AxisLabelFormatter { get; set; } = value => value.ToString("N2");

	public ObservableCollection<int> SudokuCells { get; set; } = new([.. Enumerable.Range(1, 81).Select(_ => Random.Shared.Next(1, 9))]);


	public MainWindow()
	{
		InitializeComponent();

		DataContext = this;
	}
}