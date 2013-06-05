using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFuelTracker.Core.Annotations;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public class StatisticsService : IStatisticsService
	{
		public async Task<Statistics> CalculateStatisticsAsync([NotNull] IEnumerable<FillupHistoryItem> fillups)
		{
			if (fillups == null)
				throw new ArgumentNullException("fillups");
			if (!fillups.Any(f => !f.Fillup.IsPartial))
				return await Task.FromResult(new Statistics());

			return await Task.Factory.StartNew(() =>
			{
				DateTime start = fillups.Last().Fillup.Date.Date;
				DateTime last = fillups.First().Fillup.Date.Date.AddDays(1);

				var statistics = new Statistics();

				statistics.LastFuelEconomy = fillups.First(f => !f.Fillup.IsPartial).FuelEconomy.Value;
				statistics.LastFillupCost = fillups.First().Fillup.Price*fillups.First().Fillup.Volume;

				statistics.Last4FillupsAvgFuelEconomy = fillups.Where(f => !f.Fillup.IsPartial)
																.Take(4)
																.Average(f => f.FuelEconomy.Value);
				statistics.LastMonthCost = fillups.Where(f => f.Fillup.Date >= last.AddDays(-30))
												  .Sum(f => f.Fillup.Price * f.Fillup.Volume);

				double totalCost = fillups.Sum(f => f.Fillup.Price * f.Fillup.Volume);

				statistics.MinFuelEconomy = fillups.Where(f => !f.Fillup.IsPartial).Min(f => f.FuelEconomy.Value);
				statistics.MaxFuelEconomy = fillups.Where(f => !f.Fillup.IsPartial).Max(f => f.FuelEconomy.Value);
				statistics.AvgFillupCost = fillups.Where(f => !f.Fillup.IsPartial).Average(f => f.Fillup.Price * f.Fillup.Volume);
				statistics.AllTimeAvgFuelEconomy = fillups.Where(f => !f.Fillup.IsPartial).Average(f => f.FuelEconomy.Value);

				statistics.MostOftenFuelType = (from f in fillups
												group f.Fillup by f.Fillup.FuelType
													into g
													select new { FuelType = g.Key, Count = g.Count() })
												.OrderByDescending(f => f.Count)
												.Select(f => f.FuelType)
												.FirstOrDefault();
				return statistics;
			});
		}
	}
}
