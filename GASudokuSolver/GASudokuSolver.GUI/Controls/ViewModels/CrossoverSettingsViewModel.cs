using GASudokuSolver.Core.Solver.Crossovers;
using GASudokuSolver.Core.Solver.FitnessFunctions;
using GASudokuSolver.Core.Solver.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Controls.ViewModels;

public class CrossoverSettingsViewModel : INotifyPropertyChanged
{
	public ObservableCollection<CrossoverOptionViewModel> CrossoverOptions { get; } = [defaultOption];

	private CrossoverOptionViewModel selectedOption = defaultOption;

	private static readonly CrossoverOptionViewModel defaultOption
		= new SimpleCrossoverOption(
			"One Point Crossover",
			"Chooses a random cut point along the gene sequence, then produces two children by swapping parent segments.",
			() => new OnePointCrossover()
		);

	public CrossoverOptionViewModel SelectedOption
	{
		get => selectedOption;
		set
		{
			if (selectedOption != value)
			{
				selectedOption = value;
				OnPropertyChanged();
			}
		}
	}

	public CrossoverSettingsViewModel()
	{
		CrossoverOptions.Add(new KPointCrossoverOption());

		CrossoverOptions.Add(new SimpleCrossoverOption(
				"Uniform Crossover",
				"Randomly selects each gene from one of two parents, creating diverse offspring with mixed traits.",
				() => new UniformCrossover()
			)
		);
	}

	public ICrossover BuildCrossover()
	  => SelectedOption.BuildCrossover();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public abstract class CrossoverOptionViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public string Description { get; }

	protected CrossoverOptionViewModel(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public abstract ICrossover BuildCrossover();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string? propName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
}

public class SimpleCrossoverOption : CrossoverOptionViewModel
{
	private readonly Func<ICrossover> _factory;

	public SimpleCrossoverOption(string name, string description, Func<ICrossover> factory)
		: base(name, description)
	{
		_factory = factory;
	}

	public override ICrossover BuildCrossover() => _factory();
}

public class KPointCrossoverOption : CrossoverOptionViewModel
{
	private int k = DefaultK;

	public int MinimumK { get; } = 1;
	public int MaximumK { get; } = 10;

	public const int DefaultK = 2;

	public KPointCrossoverOption()
		: base(
			"K Point Crossover",
			"Splits parent genes at K random points, alternating segments between them to create varied offspring.")
	{ }

	public int K
	{
		get => k;
		set
		{
			var clamped = Math.Clamp(value, MinimumK, MaximumK);
			if (k != clamped)
			{
				k = clamped;
				OnPropertyChanged();
			}
		}
	}

	public override ICrossover BuildCrossover()
		=> new KPointCrossover(K);
}