using GASudokuSolver.Core.Enums;
using GASudokuSolver.GUI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GASudokuSolver.GUI.Windows.ViewModels;

public sealed class BestResultViewModel : INotifyPropertyChanged
{
	private ObservableCollection<SudokuCell> board;
	private string currentFitness;
	private string currentGeneration;
	private TerminationReason terminationReason;
	private Visibility resultsVisibility = Visibility.Visible;

	public BestResultViewModel(
		ObservableCollection<SudokuCell> board,
		string currentFitness,
		string currentGeneration,
		TerminationReason terminationReason
		)
	{
		this.board = board;
		this.currentFitness = currentFitness;
		this.currentGeneration = currentGeneration;
		this.terminationReason = terminationReason;
	}

	public ObservableCollection<SudokuCell> Board
	{
		get => board;
		set { board = value; OnPropertyChanged(); }
	}

	public string CurrentFitness
	{
		get => currentFitness;
		set { currentFitness = value; OnPropertyChanged(); }
	}

	public string CurrentGeneration
	{
		get => currentGeneration;
		set { currentGeneration = value; OnPropertyChanged(); }
	}

	public Visibility ResultsVisibility
	{
		get => resultsVisibility;
		set { resultsVisibility = value; OnPropertyChanged(); }
	}

	public string TerminationReasonDescription
	{
		get => GetTerminationReasonDescription(terminationReason);
	}

	public TerminationReason TerminationReason
	{
		get => terminationReason;
	}

	private static string GetTerminationReasonDescription(TerminationReason terminationReason)
	{
		return terminationReason switch
		{
			TerminationReason.SoultionFound => "A valid Sudoku solution was successfully found.",
			TerminationReason.Timeout => "The solver stopped because it exceeded the allowed time limit.",
			TerminationReason.MaxGenerationsReached => "The maximum number of generations was reached without finding a solution.",
			TerminationReason.Cancelled => "The solving process was manually cancelled by the user.",
			_ => "Unknown termination reason."
		};
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}