using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Controls.ViewModels;

public class PopulationSettingsViewModel : INotifyPropertyChanged
{
	private int populationSize = DefaultPopulationSize;
	private int numberOfParents = DefaultNumberOfParents;
	private int maxGenerations = DefaultMaxGenerations;
	private TimeSpan maxTime = DefaultMaxTime;

	public const int DefaultPopulationSize = 100;
	public const int DefaultNumberOfParents = 2;
	public const int DefaultMaxGenerations = 1000;
	public static readonly TimeSpan DefaultMaxTime = TimeSpan.FromSeconds(30);

	public int PopulationSize
	{
		get => populationSize;
		set
		{
			var v = Math.Max(1, value);
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
			// cannot exceed population size
			var v = Math.Clamp(value, 1, PopulationSize);
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
			var v = Math.Max(1, value);
			if (maxGenerations != v)
			{
				maxGenerations = v;
				OnPropertyChanged();
			}
		}
	}

	public double MaxTimeSeconds
	{
		get => MaxTime.TotalSeconds;
		set
		{
			var secs = Math.Max(0, value);
			var ts = TimeSpan.FromSeconds(secs);
			if (ts != MaxTime)
			{
				MaxTime = ts;
				OnPropertyChanged();
				OnPropertyChanged(nameof(MaxTime));
			}
		}
	}

	public TimeSpan MaxTime
	{
		get => maxTime;
		set
		{
			var v = value.TotalMilliseconds < 0
				? DefaultMaxTime
				: value;
			if (maxTime != v)
			{
				maxTime = v;
				OnPropertyChanged();
			}
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string? name = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}