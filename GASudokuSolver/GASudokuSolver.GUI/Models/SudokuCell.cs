using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Models;

public class SudokuCell : INotifyPropertyChanged
{
	private bool readOnly;
	private int? _value;

	public int Row { get; set; }
	public int Column { get; set; }

	public int? Value
	{
		get => _value;
		set
		{
			if (_value != value)
			{
				_value = value;
				OnPropertyChanged();
			}
		}
	}

	public bool ReadOnly
	{
		get => readOnly;
		set
		{
			if (readOnly != value)
			{
				readOnly = value;
				OnPropertyChanged();
			}
		}
	}


	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
