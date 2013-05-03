using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace MyFuelTracker.Core.Models
{
	public class Fillup
	{
		public int Id { get; set; }

		public DateTime Date { get; set; }

		public decimal Volume { get; set; }

		public decimal Price { get; set; }

		public decimal OdometerStart { get; set; }

		public decimal OdometerEnd { get; set; }

		public bool IsPartial { get; set; } 
	}
}
