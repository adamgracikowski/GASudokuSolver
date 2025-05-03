using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.GUI.Controls.ViewModels;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class MutationSettingsControl : UserControl
{
	public MutationSettingsControl()
	{
		InitializeComponent();
	}

	public IMutation SelectedMutation
		=> ((MutationSettingsViewModel)DataContext).BuildMutation();
}