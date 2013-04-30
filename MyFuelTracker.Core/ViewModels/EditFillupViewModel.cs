using MyFuelTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.ViewModels
{
	public class EditFillupViewModel : ViewModelBase
	{
		#region Fields

		private FillupItemViewModel _fillupItemViewModel;


		private IMyFuelTrackerApp _app;

		#endregion

		#region ctor

		public EditFillupViewModel(IMyFuelTrackerApp app)
		{
			_fillupItemViewModel = new FillupItemViewModel();
			_app = app;
		}

		#endregion

		#region Properties
		public FillupItemViewModel FillupItemViewModel
		{
			get { return _fillupItemViewModel; }
			set { SetProperty(value, ref _fillupItemViewModel); }
		}

		#endregion

		#region methods

		public void Save()
		{
			var fillup = _fillupItemViewModel.GetFillup();
			_app.DbContext.Fillups.InsertOnSubmit(fillup);
			_app.DbContext.SubmitChanges();

			_app.Navigator.GoBack();
		}

		public void Cancel()
		{
			_app.Navigator.GoBack();
		}

		#endregion

	}
}
