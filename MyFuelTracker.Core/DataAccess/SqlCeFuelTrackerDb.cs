using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;
using Newtonsoft.Json;

namespace MyFuelTracker.Core.DataAccess
{
	public class SqlCeFuelTrackerDb : IFuelTrackerDb
	{
		private bool _initialized;
		private readonly FuelTrackerDataContext _dataContext;

		public Table<Fillup> Fillups { get { return _dataContext.Fillups; } }

		public SqlCeFuelTrackerDb()
		{
			_dataContext = new FuelTrackerDataContext("Data Source=isostore:/fueltrackerdb.v1.sdf");
		}

		public Task SaveFillupAsync(Fillup fillup)
		{
			return Task.Run(() =>
			{
				if (!Fillups.Any(f => f.Id == fillup.Id))
					Fillups.InsertOnSubmit(fillup);

				_dataContext.SubmitChanges();
			});
		}

		public Task DeleteFillupAsync(Fillup fillup)
		{
			return Task.Run(() =>
				{
					Fillups.DeleteOnSubmit(fillup);
					_dataContext.SubmitChanges();
				});
		}

		public Task<Fillup[]> LoadAllFillupsAsync()
		{
			EnsureDatabase();
			return Task.Run(() => Fillups.OrderBy(f => f.Date).ToArray());
		}

		public Task RestoreAsync(IEnumerable<Fillup> fillupsData)
		{
			return Task.Run(() =>
				{
					var oldFillups = _dataContext.Fillups.ToArray();
					try
					{
						_dataContext.Fillups.DeleteAllOnSubmit(oldFillups);
						_dataContext.SubmitChanges();

						_dataContext.Fillups.InsertAllOnSubmit(fillupsData);
						_dataContext.SubmitChanges();
					}
					catch (Exception)
					{
						_dataContext.Fillups.InsertAllOnSubmit(oldFillups);
						_dataContext.SubmitChanges();

						throw;
					}
				});
		}

		private void EnsureDatabase()
		{
			if (_initialized)
				return;

			if (!_dataContext.DatabaseExists())
			{
				_dataContext.CreateDatabase();
#if(INITIAL_DATA)
				using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyFuelTracker.Core.DataAccess.fillups.json"))
				using (var reader = new StreamReader(stream))
				{
					var fillups = JsonConvert.DeserializeObject<Fillup[]>(reader.ReadToEnd());
					Fillups.InsertAllOnSubmit(fillups);
					_dataContext.SubmitChanges();
				}
#endif
			}


			_initialized = true;
		}


	}

}
