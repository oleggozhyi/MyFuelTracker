using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace MyFuelTracker.Core.Models
{
	[Table]
	public class Petrol
	{
		public EntitySet<Fillup> _fillups = new EntitySet<Fillup>();

		[Column(IsPrimaryKey = true, IsDbGenerated = true)]
		public int Id { get; set; }

		[Column]
		public string Name { get; set; }

		[Association(Storage = "_fillups", OtherKey = "_petrolId", ThisKey = "Id")]
		public EntitySet<Fillup> Fillups
		{
			get { return _fillups; }
			set { _fillups.Assign(value); }
		}  
	}
}