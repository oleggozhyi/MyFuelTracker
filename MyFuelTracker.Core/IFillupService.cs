using System.Collections.Generic;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core
{
	public interface IFillupService
	{
		Task<Fillup> CreateNewFillupAsync();
		Task SaveFillupAsync(Fillup fillup);
		Task<IEnumerable<FillupHistoryItem>> GetHistoryAsync();
	}
}