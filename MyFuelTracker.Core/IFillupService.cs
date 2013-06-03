using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public interface IFillupService
	{
        Task<FillupHistoryItem[]> GetHistoryAsync();
        Task<FuelConsumptionStatistics> GetStatisticsAsync();
        Task<Fillup> CreateNewFillupAsync();
        Task SaveFillupAsync(Fillup fillup);
		Task DeleteFillupAsync(Fillup fillup);
        Task<Fillup> GetFillupAsync(Guid id);
		Task RestoreDataAsync(IEnumerable<Fillup> fillupsData);
	}
}