using GASudokuSolver.Core.Configurations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Controls.ViewModels;

public class SolverSettingsViewModel : INotifyPropertyChanged
{
	private int populationSize = Constants.Solver.DefaultPopulationSize;
	private int numberOfParents = Constants.Solver.DefaultNumberOfParentsSize;
	private int maxGenerations = Constants.Solver.DefaultGenerations;
	private double maxTimeMinutes = Constants.Solver.DefaultTimeInMinutes;

	public static int MinimumPopulationSize => Constants.Solver.MinimumPopulationSize;
	public static int MaximumPopulationSize => Constants.Solver.MaximumPopulationSize;

	public static int MinimumNumberOfParentsSize => Constants.Solver.MinimumNumberOfParentsSize;
	public static int MaximumNumberOfParentsSize => Constants.Solver.MaximumNumberOfParentsSize;

	public static int MinimumGenerations => Constants.Solver.MinimumGenerations;
	public static int MaximumGenerations => Constants.Solver.MaximumGenerations;

	public static double MinimumTimeInMinutes => Constants.Solver.MinimumTimeInMinutes;
	public static double MaximumTimeInMinutes => Constants.Solver.MaximumTimeInMinutes;

	public int PopulationSize
	{
		get => populationSize;
		set
		{
			var v = Math.Clamp(value, MinimumPopulationSize, MaximumPopulationSize);

			if (populationSize != v)
			{
				populationSize = v;
				OnPropertyChanged();
			}
		}
	}

	public int NumberOfParents
	{
		get => numberOfParents;
		set
		{
			var v = Math.Clamp(
				value, 
				MinimumNumberOfParentsSize, 
				Math.Min(MaximumNumberOfParentsSize, PopulationSize)
			);
			
			if (numberOfParents != v)
			{
				numberOfParents = v;
				OnPropertyChanged();
			}
		}
	}

	public int MaxGenerations
	{
		get => maxGenerations;
		set
		{
			var v = Math.Clamp(value, MinimumGenerations, MaximumGenerations);

			if (maxGenerations != v)
			{
				maxGenerations = v;
				OnPropertyChanged();
			}
		}
	}

	public double MaxTimeMinutes
	{
		get => maxTimeMinutes;
		set
		{
			var v = Math.Clamp(value, MinimumTimeInMinutes,MaximumTimeInMinutes);
			
			if (maxTimeMinutes != v)
			{
				maxTimeMinutes = v;
				OnPropertyChanged();
			}
		}
	}

	public TimeSpan MaxTimeSpanMinutes => TimeSpan.FromMinutes(MaxTimeMinutes);

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string? name = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}