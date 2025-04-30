using GASudokuSolver.Core.Solver.Interfaces;
using System.ComponentModel;

namespace GASudokuSolver.Core.Models;

public class AlgorithmProgressData : INotifyPropertyChanged
{
	private double fitnessValue;
	private byte[,] board;
	private int generation;

	public AlgorithmProgressData(double fitnessValue, int generation, byte[,] board)
	{
		this.fitnessValue = fitnessValue;
		this.generation = generation;
		this.board = board;
	}

	public AlgorithmProgressData(Individual individual, int generation)
	{
		fitnessValue = individual.Fitness;
		board = individual.Board.CloneBoard();
		this.generation = generation;
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

	public byte[,] Board
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
