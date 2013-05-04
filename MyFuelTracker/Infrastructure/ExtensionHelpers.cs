using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Infrastructure
{
	public static class ExtensionHelpers
	{
		public static string FormatForDisplay(this double d, string dimension)
		{
			return string.Format("{0:#.##} {1}", d, dimension);
		}
	}
}
