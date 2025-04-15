using GASudokuSolver.GUI.Resources;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

namespace GASudokuSolver.GUI.Windows;

public partial class AboutWindow : Window
{
	public string AboutText { get; set; }

#pragma warning disable CS8618
	public AboutWindow()
#pragma warning restore CS8618
	{ 
		InitializeComponent();

		LoadAboutText();

		DataContext = this;
	}

	private void CloseClick(object sender, RoutedEventArgs e)
	{
		this.Close();
	}

	private void LoadAboutText()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourceName = ResourcePaths.About;

		using var stream = assembly.GetManifestResourceStream(resourceName);

		if(stream is null)
		{
			return;
		}

		using var reader = new StreamReader(stream);

		AboutText = reader.ReadToEnd();
	}

	private void HyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
		e.Handled = true;
	}
}