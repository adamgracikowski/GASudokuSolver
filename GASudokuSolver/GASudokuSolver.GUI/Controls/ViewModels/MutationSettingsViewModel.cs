using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Mutations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Controls.ViewModels;

public class MutationSettingsViewModel : INotifyPropertyChanged
{
	public ObservableCollection<MutationOptionViewModel> MutationOptions { get; } = [defaultOption];

	private MutationOptionViewModel selectedOption = defaultOption;

	private static readonly MutationOptionViewModel defaultOption
		= new SimpleMutatationOption(
			  "Never Mutation",
			  "Disables all mutation: offspring are created only by crossover, so no random gene changes ever occur.",
			  () => new NeverMutation()
		);

	public MutationOptionViewModel SelectedOption
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

	public MutationSettingsViewModel()
	{
		MutationOptions.Add(new PercentChanceMutationOption());

		MutationOptions.Add(
			new SimpleMutatationOption(
				"Always Mutation", 
				"Applies mutation to every gene in each offspring—maximizing diversity but risking high randomness.",
				() => new AlwaysMutation()
			)
		);
	}

	public IMutation BuildMutation()
	  => SelectedOption.BuildMutation();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public abstract class MutationOptionViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public string Description { get; }

	protected MutationOptionViewModel(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public abstract IMutation BuildMutation();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string? propName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
}

public class SimpleMutatationOption : MutationOptionViewModel
{
	private readonly Func<IMutation> _factory;

	public SimpleMutatationOption(string name, string description, Func<IMutation> factory)
		: base(name, description)
	{
		_factory = factory;
	}

	public override IMutation BuildMutation() => _factory();
}

public class PercentChanceMutationOption : MutationOptionViewModel
{
	private double percentChance = DefaultPercentChance;
	public double MinimumPercentChance { get; } = 0;
	public double MaximumPercentChance { get; } = 100;

	public const double DefaultPercentChance = 20;

	public PercentChanceMutationOption()
		: base(
			"Percent Chance",
			"Gives each gene an independent % chance to flip or change, balancing exploration via the mutation rate.")
	{ }


	public double PercentChance
	{
		get => percentChance;
		set
		{
			var clamped = Math.Clamp(value, MinimumPercentChance, MaximumPercentChance);
			if (percentChance != clamped)
			{
				percentChance = clamped;
				OnPropertyChanged();
			}
		}
	}

	public override IMutation BuildMutation()
		=> new PercentChanceMutation(PercentChance);
}