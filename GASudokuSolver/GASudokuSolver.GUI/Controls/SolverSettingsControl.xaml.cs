using GASudokuSolver.GUI.Controls.ViewModels;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class SolverSettingsControl : UserControl
{
	public SolverSettingsControl()
	{
		InitializeComponent();
	}

	public SolverSettingsViewModel ViewModel
		=> (SolverSettingsViewModel)DataContext;
}