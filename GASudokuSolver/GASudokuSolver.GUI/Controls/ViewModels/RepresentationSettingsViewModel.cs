using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Solver.Representations;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Controls.ViewModels;

public class RepresentationSettingsViewModel : INotifyPropertyChanged
{
	public ObservableCollection<RepresentationOptionViewModel> RepresentationOptions { get; } = [defaultOption];

	private RepresentationOptionViewModel selectedOption = defaultOption;

	private static readonly RepresentationOptionViewModel defaultOption
		= new SimpleRepresentationOption(
			"Single Cell Row Column",
			"One gene per empty cell in row‑major order.",
			() => new SingleCellRowColumnRepresentation()
		);

	public RepresentationOptionViewModel SelectedOption
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

	public RepresentationSettingsViewModel()
	{
		RepresentationOptions.Add(
			new SimpleRepresentationOption(
				"Candidate Choice",
				"One gene per empty cell, each initialized with that cell’s list of valid potential Sudoku values.", 
				() => new CandidateChoiceRepresentation()
			)
		); 
		
		RepresentationOptions.Add(new MultiCellRepresentationOption());
		RepresentationOptions.Add(new MultiCellPermutationRepresentationOption());
	}

	public IRepresentation BuildRepresentation()
	  => SelectedOption.BuildRepresentation();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

public abstract class RepresentationOptionViewModel : INotifyPropertyChanged
{
	public string Name { get; }
	public string Description { get; }

	protected RepresentationOptionViewModel(string name, string description)
	{
		Name = name;
		Description = description;
	}

	public abstract IRepresentation BuildRepresentation();

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string? propName = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
}

public class SimpleRepresentationOption : RepresentationOptionViewModel
{
	private readonly Func<IRepresentation> _factory;

	public SimpleRepresentationOption(string name, string description, Func<IRepresentation> factory)
		: base(name, description)
	{
		_factory = factory;
	}

	public override IRepresentation BuildRepresentation() => _factory();
}

public class MultiCellRepresentationOption : RepresentationOptionViewModel
{
	private GroupByStrategy groupByStrategy;

	public MultiCellRepresentationOption()
		: base(
			"Multi Cell Representation",
			"Groups mutable cells by row, column, or subgrid (based on strategy), " +
			"encoding each group as a single gene holding all its values. " +
			"This reduces gene count and enforces local structure in solutions.")
	{ }


	public GroupByStrategy GroupByStrategy
	{
		get => groupByStrategy;
		set
		{
			if (groupByStrategy != value)
			{
				groupByStrategy = value;
				OnPropertyChanged();
			}
		}
	}

	public override IRepresentation BuildRepresentation()
		=> new MultiCellRepresentation(GroupByStrategy);
}

public class MultiCellPermutationRepresentationOption : RepresentationOptionViewModel
{
	private GroupByStrategy groupByStrategy;

	public MultiCellPermutationRepresentationOption()
		: base(
			"Multi Cell Permutation Representation",
			"Encodes each row, column, or subgrid as a gene containing a valid permutation of missing values, " +
			"ensuring no duplicates within groups and promoting Sudoku-valid structures by design.")
	{ }


	public GroupByStrategy GroupByStrategy
	{
		get => groupByStrategy;
		set
		{
			if (groupByStrategy != value)
			{
				groupByStrategy = value;
				OnPropertyChanged();
			}
		}
	}

	public override IRepresentation BuildRepresentation()
		=> new MultiCellPermutationRepresentation(GroupByStrategy);
}