using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Infrastructure
{
	public static class ExtensionHelpers
	{
		public static string FormatForDisplay(this double d, string dimension = "")
		{
			return string.Format("{0:#.##}", d) + (dimension.IsNullOrWhitespace() ? "" : " " + dimension);
		}

		public static bool IsNullOrWhitespace(this string s)
		{
			return String.IsNullOrWhiteSpace(s);
		}

		public static double GetPositiveDoubleFor(this string s, string fieldName)
		{
			if (s.IsNullOrWhitespace())
				throw new ValidationException("enter " + fieldName);

			double result;
			if (!double.TryParse(s, out result) || result < 0)
				throw new ValidationException(fieldName + " should be a postitive number");

			return result;
		}
	}
}
