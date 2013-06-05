using System;
using System.Collections.Generic;
using System.Linq;
using MyFuelTracker.ViewModels;

namespace MyFuelTracker.Infrastructure.Helpers
{
	public static class FillupsGroupingHelper
	{
		public static IList<FillupHistoryGroupViewModel> GroupFillups(IEnumerable<FillupHistoryItemViewModel> fillups)
		{
			var groups = new Dictionary<DateTime, FillupHistoryGroupViewModel>();
			foreach (var fillup in fillups)
			{
				var groupDate = fillup.FillupDate.Date.AddDays(1 - fillup.FillupDate.Day);
				FillupHistoryGroupViewModel group;
				if (!groups.TryGetValue(groupDate, out group))
				{
					group = new FillupHistoryGroupViewModel {MonthDateTime = groupDate};
					groups[groupDate] = group;
				}

				group.Add(fillup);
			}

			return groups.Values.ToList();
		}
	}
}
