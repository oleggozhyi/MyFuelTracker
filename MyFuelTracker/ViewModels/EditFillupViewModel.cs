using Caliburn.Micro;
using MyFuelTracker.Core;

namespace MyFuelTracker.ViewModels
{
	public class EditFillupViewModel : PropertyChangedBase
	{
		private readonly INavigationService _navigationService;
		private readonly ILog _log;

		#region Fields

		private FillupItemViewModel _fillupItemViewModel;

		#endregion

		#region ctor

		public EditFillupViewModel(INavigationService navigationService, ILog log)
		{
			_navigationService = navigationService;
			_log = log;
			_fillupItemViewModel = new FillupItemViewModel();
		}

		#endregion

		#region Properties

		public string PageName {
			get { return "Vasya"; }
		}

		public FillupItemViewModel FillupItemViewModel
		{
			get { return _fillupItemViewModel; }
			set
			{
				_fillupItemViewModel = value;
				NotifyOfPropertyChange(() => FillupItemViewModel);
			}
		}

		#endregion

		#region methods

		public void SaveFillup()
		{
			_log.Info("Start saving fillup");
			_navigationService.GoBack();
		}

		public void Cancel()
		{
			_log.Info("Cancel editing fillup and go back");
			_navigationService.GoBack();
		}

		#endregion
	}
}
