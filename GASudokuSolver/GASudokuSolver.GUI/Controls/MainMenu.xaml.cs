using System.Windows;
using System.Windows.Controls;

namespace GASudokuSolver.GUI.Controls;

public partial class MainMenu : UserControl
{
	public event RoutedEventHandler SaveBoardClicked;

	public event RoutedEventHandler LoadBoardClicked;
	
	public event RoutedEventHandler ExitClicked;
	
	public event RoutedEventHandler AboutClicked;
	
	public event RoutedEventHandler UserGuideClicked;

#pragma warning disable CS8618
	public MainMenu()
#pragma warning restore CS8618
	{
		InitializeComponent();
	}

	private void SaveBoardCsvClick(object sender, RoutedEventArgs e) => SaveBoardClicked?.Invoke(sender, e);
	
	private void LoadBoardCsvClickAsync(object sender, RoutedEventArgs e) => LoadBoardClicked?.Invoke(sender, e);
	
	private void ExitClick(object sender, RoutedEventArgs e) => ExitClicked?.Invoke(sender, e);
	
	private void AboutClick(object sender, RoutedEventArgs e) => AboutClicked?.Invoke(sender, e);
	
	private void UserGuideClick(object sender, RoutedEventArgs e) => UserGuideClicked?.Invoke(sender, e);
}