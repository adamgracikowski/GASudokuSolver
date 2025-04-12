using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GASudokuSolver.GUI.Converters;

public sealed class SudokuBorderConverter : IValueConverter
{
	public const double BorderSlim = 0.4;
	public const double BorderThick = 1.2;

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is SudokuCell cell)
		{
			var top = ThickOrSlim(cell.Row);
			var left = ThickOrSlim(cell.Column);
			var right = ThickOrSlim(cell.Column + 1);
			var bottom = ThickOrSlim(cell.Row + 1);

			return new Thickness(left, top, right, bottom);
		}

		return new Thickness(0.5);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private static double ThickOrSlim(int index)
	{
		return index % 3 == 0 ? BorderThick : BorderSlim;
	}
}
