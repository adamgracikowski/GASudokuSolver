using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GASudokuSolver.GUI.Converters;

[ValueConversion(typeof(bool), typeof(Brushes))]
public sealed class ReadOnlyToBackgroundConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if(value is not bool readOnly)
		{
			return null;
		}

		return readOnly 
			? Brushes.PowderBlue 
			: Brushes.Transparent;
	}

	public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}