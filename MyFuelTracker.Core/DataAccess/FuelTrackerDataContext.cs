using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core.DataAccess
{
	public class FuelTrackerDataContext : DataContext
	{
		public FuelTrackerDataContext(string connectionString = null)
			: base(connectionString ?? "isostore:/FuelTrackerDb.sdf")
		{
		}

		public Table<Fillup> Fillups
		{
			get { return this.GetTable<Fillup>(); }
		}

		public Table<Petrol> Petrols
		{
			get { return this.GetTable<Petrol>(); }
		}

		public static async Task<FuelTrackerDataContext> EnsureDatabase(string connectionString = null)
		{
			var dataContext = await Task.Factory.StartNew(() =>
			{
				var db = new FuelTrackerDataContext(connectionString);
				if (!db.DatabaseExists())
					db.CreateDatabase();

				return db;
			});

			return dataContext;
		}
	}
}
