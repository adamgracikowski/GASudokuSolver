using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.GUI.Controls.ViewModels;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;


public partial class SelectionSettingsControl : UserControl
{
	public SelectionSettingsControl()
	{
		InitializeComponent();
	}

	public ISelection SelectedSelection
			=> ((SelectionSettingsViewModel)DataContext).BuildSelection();
}