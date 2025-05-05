using System.Windows;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class BestResultsControl : UserControl
{
	public BestResultsControl()
	{
		InitializeComponent();
	}

	public static readonly DependencyProperty FitnessProperty =
		DependencyProperty.Register(
			nameof(Fitness),
			typeof(string),
			typeof(BestResultsControl),
			new PropertyMetadata(string.Empty));

	public string Fitness
	{
		get => (string)GetValue(FitnessProperty);
		set => SetValue(FitnessProperty, value);
	}

	public static readonly DependencyProperty GenerationProperty =
		DependencyProperty.Register(
			nameof(Generation),
			typeof(string),
			typeof(BestResultsControl),
			new PropertyMetadata(string.Empty));

	public string Generation
	{
		get => (string)GetValue(GenerationProperty);
		set => SetValue(GenerationProperty, value);
	}

	public static readonly DependencyProperty ResultsVisibilityProperty =
		DependencyProperty.Register(
			nameof(ResultsVisibility),
			typeof(Visibility),
			typeof(BestResultsControl),
			new PropertyMetadata(Visibility.Collapsed));

	public Visibility ResultsVisibility
	{
		get => (Visibility)GetValue(ResultsVisibilityProperty);
		set => SetValue(ResultsVisibilityProperty, value);
	}

	public static readonly DependencyProperty TerminationReasonProperty =
		DependencyProperty.Register(
			nameof(TerminationReason),
			typeof(string),
			typeof(BestResultsControl),
			new PropertyMetadata(string.Empty));

	public string TerminationReason
	{
		get => (string)GetValue(TerminationReasonProperty);
		set => SetValue(TerminationReasonProperty, value);
	}
}