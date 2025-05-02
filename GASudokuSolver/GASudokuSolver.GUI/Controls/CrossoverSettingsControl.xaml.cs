using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.GUI.Controls.ViewModels;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;
public partial class CrossoverSettingsControl : UserControl
{
	public CrossoverSettingsControl()
	{
		InitializeComponent();
	}

	public ICrossover SelectedCrossover
		=> ((CrossoverSettingsViewModel)DataContext).BuildCrossover();
}