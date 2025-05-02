using GASudokuSolver.GUI.Controls.ViewModels;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class PopulationSettingsControl : UserControl
{
	public PopulationSettingsControl()
	{
		InitializeComponent();
	}

	public PopulationSettingsViewModel ViewModel
		=> (PopulationSettingsViewModel)DataContext;
}