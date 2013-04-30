using Microsoft.Phone.Controls;
using MyFuelTracker.Core.DataAccess;
using MyFuelTracker.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core
{
	public interface IMyFuelTrackerApp
	{
		INavigator Navigator { get; }
		MainViewModel MainViewModel { get; }
		FuelTrackerDataContext DbContext { get; }
		bool IsDatabaseReady { get; }
		event EventHandler DatabaseReady;
	}
}
