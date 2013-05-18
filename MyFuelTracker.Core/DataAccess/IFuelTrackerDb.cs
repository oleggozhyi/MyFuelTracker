﻿using System;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.Core.DataAccess
{
	public interface IFuelTrackerDb
	{
		Task SaveFillupAsync(Fillup fillup);
		Task DeleteFillupAsync(Fillup fillup);
		Task<Fillup[]> LoadAllFillupsAsync();
	}
}