using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GASudokuSolver.GUI.Converters;

[ValueConversion(typeof(bool), typeof(bool))]
public sealed class BooleanInverseConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is bool b ? !b : DependencyProperty.UnsetValue;
	}

	public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is bool b ? !b : DependencyProperty.UnsetValue;
	}
}
