using System.Collections.Generic;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public interface IStatisticsService
	{
		Task<FuelConsumptionStatistics> CalculateStatisticsAsync(IEnumerable<FillupHistoryItem> fillups);
	}
}