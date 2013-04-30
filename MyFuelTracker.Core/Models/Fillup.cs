using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace MyFuelTracker.Core.Models
{
	[Table]
	public class Fillup
	{
		//private EntityRef<Petrol> _petrol;

		//[Column(CanBeNull = true)]
		//internal int? _petrolId;

		[Column(IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id { get; set; }

		[Column]
		public DateTime Date { get; set; }

		[Column]
		public decimal Volume { get; set; }

		[Column]
		public decimal Price { get; set; }

		[Column]
		public decimal Odometer { get; set; }

		//[Association(Storage = "_petrol", IsForeignKey = true, ThisKey = "_petrolId", OtherKey = "Id", IsUnique=false)]
		//public Petrol Petrol
		//{
		//	get { return _petrol.Entity; }
		//	set { _petrol.Entity = value; }
		//}
	}
}
