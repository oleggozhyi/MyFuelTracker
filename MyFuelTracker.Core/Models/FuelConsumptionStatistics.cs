using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.Models
{
	public class FuelConsumptionStatistics
	{
		public double LastConsumption { get; set; }
		public double MinConsumption { get; set; }
		public double MaxConsumption { get; set; }
		public double AllTimeAvgConsumption { get; set; }
		public double Last4FillupsAvgConsumption { get; set; }
		public double AllTimeAvgMonthCost { get; set; }
		public double LastMonthCost { get; set; }
		public double AvgFillupCost { get; set; }
		public double LastFillupCost { get; set; }
		public string MostOftenFuelType { get; set; }
	}
}
