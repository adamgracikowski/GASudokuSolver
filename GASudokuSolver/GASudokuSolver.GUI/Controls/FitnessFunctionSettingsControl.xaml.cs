using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.GUI.Controls.ViewModels;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class FitnessFunctionSettingsControl : UserControl
{
	public FitnessFunctionSettingsControl()
	{
		InitializeComponent();
	}

	public IFitnessFunction SelectedFitnessFunction
		=> ((FitnessFunctionSettingsViewModel)DataContext).BuildFitnessFunction();
}