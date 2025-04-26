using System.Windows;
using System.Windows.Input;

namespace GASudokuSolver.GUI.Windows;

public partial class BestResultWindow : Window
{
	public BestResultWindow()
	{
		InitializeComponent();

	}

	public BestResultWindow(BestResultViewModel viewModel)
		: this()
	{
		DataContext = viewModel;
	}

	private void WindowMouseDown(object sender, MouseButtonEventArgs e)
	{
		if (e.ChangedButton == MouseButton.Left)
			DragMove();
	}

	private void WindowKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Escape)
			Close();
	}
}