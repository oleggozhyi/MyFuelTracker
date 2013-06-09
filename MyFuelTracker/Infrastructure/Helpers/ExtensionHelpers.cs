using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using MyFuelTracker.Resources;

namespace MyFuelTracker.Infrastructure.Helpers
{
	public static class ExtensionHelpers
	{
		public static Brush ToBrush(this Color color)
		{
			return new SolidColorBrush(color);
		}

		public static string FormatForDisplay(this double d, int digits)
		{
			d = Math.Round(d, digits, MidpointRounding.AwayFromZero);
			//Sorry, just quick and dirty
			if (digits != 2)
				return d.ToString();

			return String.Format("{0:#.00}", d);
		}

		public static bool IsNullOrWhitespace(this string s)
		{
			return String.IsNullOrWhiteSpace(s);
		}

		public static double GetPositiveDoubleFor(this string s, string fieldName)
		{
			if (s.IsNullOrWhitespace())
				throw new ValidationException(string.Format(AppResources.Validation_Should_Not_Be_Empty, fieldName));
			s = s.Replace(",", ".");

			double result;
			if (!double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out result) || result < 0)
				throw new ValidationException(string.Format(AppResources.Validation_Should_Be_Positive_Number, fieldName));

			return result;
		}

		public static void Foreach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var element in collection)
			{
				action(element);
			}
		}
	}
}
