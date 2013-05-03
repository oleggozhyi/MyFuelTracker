using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lex.Db;
using MyFuelTracker.Core.Annotations;
using MyFuelTracker.Core.DataAccess;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public class FillupService : IFillupService
	{
		private IFuelTrackerDb Db { get; set; }

		public FillupService(IFuelTrackerDb db)
		{
			Db = db;
		}

		//private readonly List<Fillup> _fillups = new List<Fillup>
		//{
		//	new Fillup {Date = new DateTime(2013,04, 13), OdometerStart = 2000, OdometerEnd = 2360, Volume = 37.2m, Price = 12.59M },
		//	new Fillup {Date = new DateTime(2013,04, 20), OdometerStart = 2360, OdometerEnd = 2700, Volume = 35.6m, Price = 12.59M },
		//	new Fillup {Date = new DateTime(2013,04, 25), OdometerStart = 2700, OdometerEnd = 3050, Volume = 34.2m, Price = 12.59M },
		//	new Fillup {Date = new DateTime(2013,04, 29), OdometerStart = 3050, OdometerEnd = 3460, Volume = 39.2m, Price = 12.59M },
		//	new Fillup {Date = new DateTime(2013,05, 1), OdometerStart = 3460, OdometerEnd = 3850, Volume = 42.2m, Price = 12.59M },
		//};


		public async Task<Fillup> CreateNewFillupAsync()
		{
			var fillup = new Fillup { Id = Guid.NewGuid(), Date = DateTime.Now };
			var fillups = await Db.LoadAllFillupsAsync();
			if (fillups.Any())
			{
				var lastOdometerEnd = fillups.OrderBy(f => f.OdometerEnd).Last().OdometerEnd;
				fillup.OdometerStart = lastOdometerEnd;
				fillup.OdometerEnd = lastOdometerEnd;
			}
			return await Task.FromResult(fillup);
		}

		public async Task SaveFillupAsync([NotNull] Fillup fillup)
		{
			if (fillup == null) throw new ArgumentNullException("fillup");
			if (fillup.OdometerEnd <= fillup.OdometerStart)
				throw new InvalidOperationException("Odometer start should be less than end");

			await Db.SaveFillupAsync(fillup);
		}

		public async Task<IEnumerable<FillupHistoryItem>> GetHistoryAsync()
		{
			var fillups = await Db.LoadAllFillupsAsync();
			fillups = fillups.OrderBy(f => f.Date).ToArray();
			return await Task.Factory.StartNew(() =>
				CalculateHistory(fillups));
		}

		private IEnumerable<FillupHistoryItem> CalculateHistory(Fillup[] fillups)
		{
			var historyItems = new List<FillupHistoryItem>();
			if (!fillups.Any())
				return historyItems;

			double partialFillupOdoStart = -1;
			double partialFillupVolume = 0;
			foreach (var fillup in fillups)
			{
				var historyItem = new FillupHistoryItem { Fillup = fillup };
				historyItems.Add(historyItem);

				if (fillup.IsPartial)
				{
					if (partialFillupOdoStart < 0)
						partialFillupOdoStart = fillup.OdometerStart;
					partialFillupVolume += fillup.Volume;
				}
				else
				{
					double actualVolume = fillup.Volume + partialFillupVolume;
					double actualDistance = fillup.OdometerEnd - (partialFillupOdoStart > 0 ? partialFillupOdoStart : fillup.OdometerStart);
					historyItem.Consumption = actualVolume * 100 / actualDistance;

					partialFillupOdoStart = -1;
					partialFillupVolume = 0;
				}
			}

			//double average = historyItems.Where(i => i.FuelEconomy.HasValue).Average(i => i.FuelEconomy.Value);
			//foreach (var fillupHistoryItem in historyItems.Where(i => i.FuelEconomy.HasValue))
			//{
			//	fillupHistoryItem.IsGreaterThanAverage = fillupHistoryItem.FuelEconomy > average;
			//}

			historyItems.Reverse();
			return historyItems;
		}
	}
}
