using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.Models
{
	public class Statistics
	{
		public double LastFuelEconomy { get; set; }
		public double MinFuelEconomy { get; set; }
		public double MaxFuelEconomy { get; set; }
		public double AllTimeAvgFuelEconomy { get; set; }
		public double Last4FillupsAvgFuelEconomy { get; set; }
		public double LastMonthCost { get; set; }
		public double AvgFillupCost { get; set; }
		public double LastFillupCost { get; set; }
		public string MostOftenFuelType { get; set; }
	}
}
