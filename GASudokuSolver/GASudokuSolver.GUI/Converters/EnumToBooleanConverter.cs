﻿using System.Globalization;
using System.Windows.Data;

namespace GASudokuSolver.GUI.Converters;

[ValueConversion(typeof(Enum), typeof(bool))]
public sealed class EnumToBooleanConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value?.ToString() == parameter?.ToString();

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> (bool)value 
			? Enum.Parse(targetType, parameter?.ToString() ?? string.Empty) 
			: Binding.DoNothing;
}