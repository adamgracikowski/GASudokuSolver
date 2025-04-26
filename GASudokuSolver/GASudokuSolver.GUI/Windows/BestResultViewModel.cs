using GASudokuSolver.GUI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GASudokuSolver.GUI.Windows;

public sealed class BestResultViewModel : INotifyPropertyChanged
{
	private ObservableCollection<SudokuCell> _board;
	private string _currentFitness;
	private string _currentGeneration;
	private Visibility _resultsVisibility = Visibility.Visible;

	public BestResultViewModel(
		ObservableCollection<SudokuCell> board, 
		string currentFitness, 
		string currentGeneration
		)
	{
		_board = board;
		_currentFitness = currentFitness;
		_currentGeneration = currentGeneration;
	}

	public ObservableCollection<SudokuCell>  Board
	{
		get => _board;
		set { _board = value; OnPropertyChanged(); }
	}

	public string CurrentFitness
	{
		get => _currentFitness;
		set { _currentFitness = value; OnPropertyChanged(); }
	}

	public string CurrentGeneration
	{
		get => _currentGeneration;
		set { _currentGeneration = value; OnPropertyChanged(); }
	}

	public Visibility ResultsVisibility
	{
		get => _resultsVisibility;
		set { _resultsVisibility = value; OnPropertyChanged(); }
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}