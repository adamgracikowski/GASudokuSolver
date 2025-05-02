using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.GUI.Controls.ViewModels;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class RepresentationSettingsControl : UserControl
{
	public RepresentationSettingsControl()
	{
		InitializeComponent();
	}

	public IRepresentation SelectedRepresentation
	=> ((RepresentationSettingsViewModel)DataContext).BuildRepresentation();
}