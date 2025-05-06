using GASudokuSolver.Core.Configurations;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GASudokuSolver.GUI.Converters;

public sealed class SudokuBackgroundConverter : IMultiValueConverter
{
	public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		var mutable = values[0] as bool? ?? false;
		var value = values[1] as int?;
		var correct = values[2] as int?;
		var highlightErrors = values[3] as bool? ?? false;

		if (!mutable)
			return Brushes.Transparent;

		if (highlightErrors 
			&& value != Constants.Cell.EmptyValue
			&& value != null
			&& correct != null
			&& value != correct)
		{
			return Brushes.LightCoral;
		}

		return Brushes.PowderBlue;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		=> throw new NotImplementedException();
}