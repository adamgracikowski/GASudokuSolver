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
	private string terminationReason;
	private Visibility resultsVisibility = Visibility.Visible;

	public BestResultViewModel(
		ObservableCollection<SudokuCell> board,
		string currentFitness,
		string currentGeneration,
		string terminationReason
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

	public string TerminationReason
	{
		get => terminationReason;
		set { terminationReason = value; OnPropertyChanged(); }
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}