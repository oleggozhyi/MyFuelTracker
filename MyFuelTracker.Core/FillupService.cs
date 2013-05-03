using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public class FillupService : IFillupService
	{
		private readonly List<Fillup> _fillups = new List<Fillup>
		{
			new Fillup {Date = new DateTime(2013,04, 13), OdometerStart = 2000, OdometerEnd = 2360, Volume = 37.2m, Price = 12.59M },
			new Fillup {Date = new DateTime(2013,04, 20), OdometerStart = 2360, OdometerEnd = 2700, Volume = 35.6m, Price = 12.59M },
			new Fillup {Date = new DateTime(2013,04, 25), OdometerStart = 2700, OdometerEnd = 3050, Volume = 34.2m, Price = 12.59M },
			new Fillup {Date = new DateTime(2013,04, 29), OdometerStart = 3050, OdometerEnd = 3460, Volume = 39.2m, Price = 12.59M },
			new Fillup {Date = new DateTime(2013,05, 1), OdometerStart = 3460, OdometerEnd = 3850, Volume = 42.2m, Price = 12.59M },
		};

		public async Task SaveFillupAsync(Fillup fillup)
		{
			await Task.Factory.StartNew(() => _fillups.Add(fillup));
		}

		public async Task<IEnumerable<FillupHistoryItem>> GetHistoryAsync()
		{
			return await Task.Factory.StartNew(() => CalculateHistory().OrderByDescending(h => h.Fillup.Date).ToArray());
		}

		private IEnumerable<FillupHistoryItem> CalculateHistory()
		{
			if (_fillups.Count == 0)
				return Enumerable.Empty<FillupHistoryItem>();

			var historyItems = _fillups.Select(f => new FillupHistoryItem
			{
				Fillup = f,
				FuelEconomy = f.IsPartial ? null : (decimal?)(f.Volume * 100.0M / (f.OdometerEnd - f.OdometerStart))
			}).ToArray();
			decimal average = historyItems.Where(i => i.FuelEconomy.HasValue).Average(i => i.FuelEconomy.Value);
			foreach (var fillupHistoryItem in historyItems.Where(i => i.FuelEconomy.HasValue))
			{
				fillupHistoryItem.IsGreaterThanAverage = fillupHistoryItem.FuelEconomy > average;
			}

			return historyItems;
		}
	}
}
