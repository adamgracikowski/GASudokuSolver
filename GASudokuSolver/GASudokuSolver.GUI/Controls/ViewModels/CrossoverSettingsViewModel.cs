using GASudokuSolver.Core.Solver.Crossovers;
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
			"Chooses a random cut point along the gene sequence, then produces two children by swapping parent segments: " +
			"Child 1 inherits Parent A’s genes up to the cut and Parent B’s genes after it, while Child 2 does the opposite.",
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
		// CrossoverOptions.Add(new YetAnotherCrossover());
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