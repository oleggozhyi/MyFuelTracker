using System.Diagnostics;
using System.Windows.Input;
using Caliburn.Micro;
using MyFuelTracker.Core;

namespace MyFuelTracker.ViewModels
{
	public class MainViewModel : Conductor<IScreen>.Collection.OneActive
	{
		private readonly INavigationService _navigationService;
		private readonly ILog _log;
		public SummaryViewModel SummaryViewModel { get; set; }
		public HistoryViewModel HistoryViewModel { get; set; }

		public MainViewModel()
		{

			//for design time support
		}

		public MainViewModel(SummaryViewModel summaryViewModel, 
							HistoryViewModel historyViewModel,
							INavigationService navigationService,
							ILog log
						)
		{
			_navigationService = navigationService;
			_log = log;
			Debug.WriteLine("MainViewModel created");

			SummaryViewModel = summaryViewModel;
			HistoryViewModel = historyViewModel;
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			Items.Add(SummaryViewModel);
			Items.Add(HistoryViewModel);

			ActivateItem(SummaryViewModel);
		}

		public void GoToAddFillup()
		{
			_log.Info("Navigating to Edit Fillup View");
			_navigationService.UriFor<EditFillupViewModel>().Navigate();
		}

		public void GoToSettings()
		{
			_log.Info("Navigating to settings (not implemented)");
		}
	}
}
