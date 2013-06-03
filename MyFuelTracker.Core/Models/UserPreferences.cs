using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.Models
{
	class UserPreferences
	{
		public ConsumptionStrategy ConsumptionStrategy { get; set; }
	}

	public enum ConsumptionStrategy
	{
		Mpg,
		KmL,
		L100Km
	}
}
