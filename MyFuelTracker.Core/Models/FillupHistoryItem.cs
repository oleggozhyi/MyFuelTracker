using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.Models
{
	public class FillupHistoryItem
	{
		public Fillup Fillup { get; set; }
		public decimal? FuelEconomy { get; set; }
		public bool IsGreaterThanAverage { get; set; }
	}
}
