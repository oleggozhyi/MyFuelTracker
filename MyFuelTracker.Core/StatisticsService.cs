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
		public async Task<FuelConsumptionStatistics> CalculateStatisticsAsync([NotNull] IEnumerable<FillupHistoryItem> fillups)
		{
			if (fillups == null)
				throw new ArgumentNullException("fillups");
			if (!fillups.Any(f => !f.Fillup.IsPartial))
				return await Task.FromResult(new FuelConsumptionStatistics());

			return await Task.Factory.StartNew(() =>
			{
				DateTime start = fillups.Last().Fillup.Date.Date;
				DateTime last = fillups.First().Fillup.Date.Date.AddDays(1);

				double monthsCount = (double)((last - start).TotalDays) / 30.0;

				var statistics = new FuelConsumptionStatistics();

				statistics.AllTimeAvgConsumption = fillups.Where(f => !f.Fillup.IsPartial)
														.Average(f => f.Consumption.Value);
				statistics.LastConsumption = fillups.First(f => !f.Fillup.IsPartial)
													.Consumption.Value;
				statistics.Last4FillupsAvgConsumption = fillups.Where(f => !f.Fillup.IsPartial)
																.Take(4)
																.Average(f => f.Consumption.Value);
				statistics.LastMonthCost = fillups.Where(f => f.Fillup.Date >= last.AddDays(-30))
												  .Sum(f => f.Fillup.Price * f.Fillup.Volume);

				double totalCost = fillups.Sum(f => f.Fillup.Price*f.Fillup.Volume);
				statistics.AllTimeAvgMonthCost = totalCost / monthsCount;

				statistics.MinConsumption = fillups.Where(f => !f.Fillup.IsPartial).Min(f => f.Consumption.Value);
				statistics.MaxConsumption = fillups.Where(f => !f.Fillup.IsPartial).Max(f => f.Consumption.Value);

				return statistics;
			});
		}
	}
}
