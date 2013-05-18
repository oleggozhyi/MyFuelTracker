using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace MyFuelTracker.Core.Models
{
    [Table]
	public class Fillup
	{
        [Column(IsPrimaryKey = true, IsDbGenerated = false)]
		public Guid Id { get; set; }

        [Column]
		public DateTime Date { get; set; }

        [Column]
		public double Volume { get; set; }

        [Column]
		public double Price { get; set; }

        [Column]
		public double OdometerStart { get; set; }

        [Column]
		public double OdometerEnd { get; set; }

        [Column]
		public bool IsPartial { get; set; }

        [Column]
		public string FuelType { get; set; }
	}
}
