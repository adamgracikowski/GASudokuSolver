using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GASudokuSolver.GUI.Models;

public class SudokuCell : INotifyPropertyChanged
{
	private bool mutable;
	private int? _value;
	private int? _correctValue;

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

	public bool Mutable
	{
		get => mutable;
		set
		{
			if (mutable != value)
			{
				mutable = value;
				OnPropertyChanged();
			}
		}
	}

	public int? CorrectValue
	{
		get => _correctValue;
		set
		{
			if (_correctValue != value)
			{
				_correctValue = value;
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
