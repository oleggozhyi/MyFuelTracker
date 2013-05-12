using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lex.Db;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core.DataAccess
{
	public class FuelTrackerDb : DbInstance, IFuelTrackerDb
	{
		#region fields

		private DbTable<Fillup> _fillups;

		#endregion

		#region ctors

		public FuelTrackerDb() : this("fuel-tracker.db")
		{
		}

		public FuelTrackerDb(string path) : base(path)
		{
		}

		#endregion

		#region properties

		private DbTable<Fillup> Fillups
		{
			get { return _fillups ?? (_fillups = Table<Fillup>()); }
		}

		#endregion

		public Task<Fillup> GetFillupAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task SaveFillupAsync(Fillup fillup)
		{
			await Fillups.SaveAsync(fillup);
		}

		public Task DeleteFillupAsync(Fillup fillup)
		{
			throw new NotImplementedException();
		}

		public async Task<Fillup[]> LoadAllFillupsAsync()
		{
			return await Fillups.LoadAllAsync();
		}
	}
}
