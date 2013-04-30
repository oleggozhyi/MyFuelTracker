using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.ViewModels
{
	public class MainViewModel
	{
		private IMyFuelTrackerApp _app;

		public MainViewModel()
		{
			//for design time support
		}

		public MainViewModel(IMyFuelTrackerApp app)
		{
			_app = app;
			Summary = new SummaryViewModel(app);
			History = new HistoryViewModel(app);
		}

		public SummaryViewModel Summary { get; set; }
		public HistoryViewModel History { get; set; }
	}
}
