using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace MyFuelTracker.Core.Models
{
	public class Fillup
	{
		public Guid Id { get; set; }

		public DateTime Date { get; set; }

		public double Volume { get; set; }

		public double Price { get; set; }

		public double OdometerStart { get; set; }

		public double OdometerEnd { get; set; }

		public bool IsPartial { get; set; }

		public string Petrol { get; set; }
	}
}
