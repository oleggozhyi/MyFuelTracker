using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.Models
{
	class UserPreferences
	{
		public FuelEconomyStrategy FuelEconomyStrategy { get; set; }
	}

	public enum FuelEconomyStrategy
	{
		Mpg,
		KmL,
		L100Km
	}
}
