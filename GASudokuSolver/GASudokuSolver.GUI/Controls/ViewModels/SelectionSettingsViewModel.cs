using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Selections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Controls.ViewModels;

public class SelectionSettingsViewModel : INotifyPropertyChanged
{
	public ObservableCollection<SelectionOptionViewModel> SelectionOptions { get; } = [ defaultOption ];

	private SelectionOptionViewModel selectedOption = defaultOption;

	private static readonly SelectionOptionViewModel defaultOption 
		= new SimpleSelectionOption(
			  "Roulette Wheel",
			  "Spins a ‘fitness wheel’ so individuals are chosen in proportion to their fitness.",
			  () => new RouletteWheelSelection()
		);

	public SelectionOptionViewModel SelectedOption
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

	public SelectionSettingsViewModel()
	{
		SelectionOptions.Add(
			new SimpleSelectionOption(
				"Stochastic Universal Sampling",
				"Evenly spaced pointers sample the wheel, giving a more consistent spread than roulette.",
				() => new StochasticUniversalSamplingSelection()
			)
		);

		SelectionOptions.Add(
			new SimpleSelectionOption(
				"Rank Selection",
				"Sorts the population by fitness, then assigns selection probabilities linearly by rank.",
				() => new RankSelection()
			)
		);

		SelectionOptions.Add(new TournamentSelectionOption());

		SelectionOptions.Add(
			new SimpleSelectionOption(
				"Truncate Selection",
				"Sorts the population by fitness (best first), then keeps only the top N individuals" +
				" — discarding the rest to form the next generation.",
				() => new TruncateSelection()
			)
		);

		SelectionOptions.Add(
			new SimpleSelectionOption(
				"Uniform Random Selection",
				"Ignores fitness: every individual has an equal chance to be chosen.",
				() => new UniformRandomSelection()
			)
		);
	}

	public ISelection BuildSelection()
	  => SelectedOption.BuildSelection();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) 
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public abstract class SelectionOptionViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public string Description { get; }

	protected SelectionOptionViewModel(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public abstract ISelection BuildSelection();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string? propName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
}

public class SimpleSelectionOption : SelectionOptionViewModel
{
	private readonly Func<ISelection> _factory;

	public SimpleSelectionOption(string name, string description, Func<ISelection> factory)
		: base(name, description)
	{
		_factory = factory;
	}

	public override ISelection BuildSelection() => _factory();
}

public class TournamentSelectionOption : SelectionOptionViewModel
{
	private int tournamentSize = DefaultTrournamentSize;
	public int MinimumTournamentSize { get; } = 2;
	public int MaximumTournamentSize { get; } = 100;
	public const int DefaultTrournamentSize = 20;

	public TournamentSelectionOption()
		: base(
			"Tournament Selection",
			"Randomly picks a group of candidates to compete against each other. The best in each mini‑tournament wins.")
	{ }


	public int TournamentSize
	{
		get => tournamentSize;
		set
		{
			var clamped = Math.Clamp(value, MinimumTournamentSize, MaximumTournamentSize);
			if (tournamentSize != clamped)
			{
				tournamentSize = clamped;
				OnPropertyChanged();
			}
		}
	}

	public override ISelection BuildSelection()
		=> new TournamentSelection(TournamentSize);
}
