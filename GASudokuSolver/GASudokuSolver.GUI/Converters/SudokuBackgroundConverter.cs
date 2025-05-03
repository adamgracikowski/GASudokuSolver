using GASudokuSolver.Core.Configurations;
using GASudokuSolver.GUI.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GASudokuSolver.GUI.Converters;

[ValueConversion(typeof(SudokuCell), typeof(Brush))]
public sealed class SudokuBackgroundConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is SudokuCell cell)
		{
			if (!cell.Mutable)
			{
				return Brushes.Transparent;
			}

			if (cell.Value != Constants.Cell.EmptyValue && 
				cell.Value != null &&
				cell.CorrectValue != null &&
				cell.Value != cell.CorrectValue)
			{
				return Brushes.LightCoral;
			}

			return Brushes.PowderBlue;
		}

		return DependencyProperty.UnsetValue;
	}


	public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
