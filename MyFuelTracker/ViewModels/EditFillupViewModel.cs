using Caliburn.Micro;
using MyFuelTracker.Core;

namespace MyFuelTracker.ViewModels
{
	public class EditFillupViewModel : PropertyChangedBase
	{
		#region Fields

		private FillupItemViewModel _fillupItemViewModel;

		#endregion

		#region ctor

		public EditFillupViewModel()
		{
			_fillupItemViewModel = new FillupItemViewModel();
		}

		#endregion

		#region Properties
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


		#endregion
	}
}
