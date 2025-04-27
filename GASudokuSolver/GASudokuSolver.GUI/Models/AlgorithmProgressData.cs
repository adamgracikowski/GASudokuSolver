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
		get => this.fitnessValue;
		set
		{
			if (this.fitnessValue != value)
			{
				this.fitnessValue = value;
				OnPropertyChanged(nameof(FitnessValue));
			}
		}
	}

	public int[,] Board
	{
		get => this.board;
		set
		{
			this.board = value;
			OnPropertyChanged(nameof(Board));
		}
	}

	public int Generation
	{
		get => this.generation;
		set
		{
			if (this.generation != value)
			{
				this.generation = value;
				OnPropertyChanged(nameof(Generation));
			}
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
