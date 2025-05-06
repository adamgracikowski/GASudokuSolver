using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class SudokuBoardControl : UserControl
{
	public SudokuBoardControl()
	{
		InitializeComponent();
	}

	public static readonly DependencyProperty BoardProperty =
		DependencyProperty.Register(
			nameof(Board),
			typeof(IEnumerable),
			typeof(SudokuBoardControl),
			new PropertyMetadata(default(IEnumerable)));

	public IEnumerable Board
	{
		get => (IEnumerable)GetValue(BoardProperty);
		set => SetValue(BoardProperty, value);
	}

	public static readonly DependencyProperty HighlightErrorsProperty =
	DependencyProperty.Register(
		nameof(HighlightErrors),
		typeof(bool),
		typeof(SudokuBoardControl),
		new PropertyMetadata(false));

	public bool HighlightErrors
	{
		get => (bool)GetValue(HighlightErrorsProperty);
		set => SetValue(HighlightErrorsProperty, value);
	}
}