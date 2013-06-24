using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyFuelTracker.Infrastructure.Conveters
{
	public class EnumToIntConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			// Note: as pointed out by Martin in the comments on this answer, this line
			// depends on the enum values being sequentially ordered from 0 onward,
			// since combobox indices are done that way. A more general solution would
			// probably look up where in the GetValues array our value variable
			// appears, then return that index.
			return (int)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Enum.Parse(targetType, value.ToString(), true);
		}
	}
}
