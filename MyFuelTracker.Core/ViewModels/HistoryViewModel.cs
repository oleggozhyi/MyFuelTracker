using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyFuelTracker.Core.ViewModels
{
	public class HistoryViewModel : ViewModelBase
	{
		#region fields

		private IMyFuelTrackerApp _app;

		#endregion

		#region ctors

		public HistoryViewModel()
		{
			//for design time
		}

		public HistoryViewModel(IMyFuelTrackerApp app)
		{
			Fillups = new ObservableCollection<FillupItemViewModel>();
			_app = app;
			if (!_app.IsDatabaseReady)
			{
				_app.DatabaseReady += (s, e) => OnDatabaseReady();
			}
			else
			{
				OnDatabaseReady();
			}
		}

		private void OnDatabaseReady()
		{
			Debug.Assert(_app.DbContext != null);
			foreach(var vm in _app.DbContext.Fillups.Select(f => new FillupItemViewModel(f)))
			{
				Fillups.Add(vm);
			}
		}

		#endregion

		#region properties

		public ObservableCollection<FillupItemViewModel> Fillups { get; set; }

		#endregion
	}
}
