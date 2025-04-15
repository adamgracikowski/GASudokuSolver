using GASudokuSolver.Core.Configurations;
using System.ComponentModel;

namespace GASudokuSolver.GUI.Models;

public class AlgorithmProgressData : INotifyPropertyChanged
{
	private double fitnessValue;
	private int[,] board;
	private int generation;

	public AlgorithmProgressData(double fitnessValue, int generation, int[,] board)
	{
		this.fitnessValue = fitnessValue;
		this.generation = generation;
		this.board = board;
	}

	public double FitnessValue
	{
		get => fitnessValue;
		set
		{
			if (fitnessValue != value)
			{
				fitnessValue = value;
				OnPropertyChanged(nameof(FitnessValue));
			}
		}
	}

	public int[,] Board
	{
		get => board;
		set
		{
			board = value;
			OnPropertyChanged(nameof(Board));
		}
	}

	public int Generation
	{
		get => generation;
		set
		{
			if (generation != value)
			{
				generation = value;
				OnPropertyChanged(nameof(Generation));
			}
		}
	}

	public static AlgorithmProgressData CreateEmpty()
	{
		return new AlgorithmProgressData(0, 0, new int[Constants.Grid.Rows, Constants.Grid.Columns]);
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
