using GASudokuSolver.Core.Solver.FitnessFunctions;
using GASudokuSolver.Core.Solver.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Controls.ViewModels;

public class FitnessFunctionSettingsViewModel : INotifyPropertyChanged
{
	public ObservableCollection<FitnessFunctionOptionViewModel> FitnessFunctionOptions { get; } = [defaultOption];

	private FitnessFunctionOptionViewModel selectedOption = defaultOption;

	private static readonly FitnessFunctionOptionViewModel defaultOption
		= new SimpleFitnessFunctionOption(
			"Equally Punished Conflict",
			"Evaluates a Sudoku by scanning for duplicate numbers in each row, column, and subgrid, subtracting one point per conflict. " +
			"All conflicts count equally, and a perfect solution scores 0.",
			() => new EquallyPunishedConflictFitnessFunction()
		);

	public FitnessFunctionOptionViewModel SelectedOption
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

	public FitnessFunctionSettingsViewModel()
	{
		FitnessFunctionOptions.Add(new SimpleFitnessFunctionOption(
				"Rising Conflict",
				"Each repeated number in a row, column, or subgrid is penalized more heavily the more it appears, " +
				"with conflict penalties growing cumulatively. A perfect solution scores 0.",
				() => new RisingConflictPenaltyFitnessFunction()
			)
		);

		FitnessFunctionOptions.Add(new WeightedConflictFitnessFunctionOption());
	}

	public IFitnessFunction BuildFitnessFunction()
	  => SelectedOption.BuildFitnessFunction();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public abstract class FitnessFunctionOptionViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public string Description { get; }

	protected FitnessFunctionOptionViewModel(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public abstract IFitnessFunction BuildFitnessFunction();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string? propName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
}

public class SimpleFitnessFunctionOption : FitnessFunctionOptionViewModel
{
	private readonly Func<IFitnessFunction> _factory;

	public SimpleFitnessFunctionOption(string name, string description, Func<IFitnessFunction> factory)
		: base(name, description)
	{
		_factory = factory;
	}

	public override IFitnessFunction BuildFitnessFunction() => _factory();
}

public class WeightedConflictFitnessFunctionOption : FitnessFunctionOptionViewModel
{
	private double rowPenalty = DefaultPenalty;
	private double columnPenalty = DefaultPenalty;
	private double subgridPenalty = DefaultPenalty;

	public double MinimumPenalty { get; } = 0;
	public double MaximumPenalty { get; } = 100;

	public const double DefaultPenalty = 1;

	public WeightedConflictFitnessFunctionOption()
		: base(
			"Weighted Conflict",
			"Similar to the equally‑punished variant, but you can assign different penalties for row, column, and subgrid conflicts " +
			"(e.g. make subgrid violations cost more). The fitness is the negative sum of weighted conflicts, aiming for 0.")
	{ }

	public double RowPenalty
	{
		get => rowPenalty;
		set
		{
			var clamped = Math.Clamp(value, MinimumPenalty, MaximumPenalty);
			if (rowPenalty != clamped)
			{
				rowPenalty = clamped;
				OnPropertyChanged();
			}
		}
	}

	public double ColumnPenalty
	{
		get => columnPenalty;
		set
		{
			var clamped = Math.Clamp(value, MinimumPenalty, MaximumPenalty);
			if (columnPenalty != clamped)
			{
				columnPenalty = clamped;
				OnPropertyChanged();
			}
		}
	}

	public double SubgridPenalty
	{
		get => subgridPenalty;
		set
		{
			var clamped = Math.Clamp(value, MinimumPenalty, MaximumPenalty);
			if (subgridPenalty != clamped)
			{
				subgridPenalty = clamped;
				OnPropertyChanged();
			}
		}
	}

	public override IFitnessFunction BuildFitnessFunction()
		=> new WeightedConflictFitnessFunction(RowPenalty, ColumnPenalty, SubgridPenalty);
}