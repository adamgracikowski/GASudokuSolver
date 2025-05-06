using GASudokuSolver.Core.Enums;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace GASudokuSolver.GUI.Converters;

[ValueConversion(typeof(TerminationReason), typeof(TextBlock))]
public class TerminationReasonToEmojiConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is TerminationReason terminationReason)
		{
			var emojiText = terminationReason switch
			{
				TerminationReason.SoultionFound => "✅",
				TerminationReason.Timeout => "⏳",
				TerminationReason.MaxGenerationsReached => "⚠️",
				TerminationReason.Cancelled => "⚠️",
				_ => "❓"
			};

			var emojiBlock = new TextBlock
			{
				Text = emojiText,
				Foreground = terminationReason switch
				{
					TerminationReason.SoultionFound => new SolidColorBrush(Colors.Green),
					TerminationReason.Timeout => new SolidColorBrush(Colors.Orange),
					TerminationReason.MaxGenerationsReached => new SolidColorBrush(Colors.Orange),
					TerminationReason.Cancelled => new SolidColorBrush(Colors.Red),
					_ => new SolidColorBrush(Colors.Gray)
				}
			};

			return emojiBlock;
		}

		return new TextBlock { Text = "❓", Foreground = new SolidColorBrush(Colors.Gray) };
	}

	public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		return System.Windows.DependencyProperty.UnsetValue;
	}
}