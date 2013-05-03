using System;
using System.Diagnostics;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class SummaryViewModel : Screen
	{
		private readonly INavigationService _navigationService;
		private readonly IMessageBox _messageBox;

		#region Fields

		private decimal _avgConsumption;
		private decimal _minConsumption;
		private decimal _maxConsumption;

		#endregion

		#region ctor

		public SummaryViewModel(INavigationService navigationService, IMessageBox messageBox)
		{
			_navigationService = navigationService;
			_messageBox = messageBox;
			Debug.WriteLine("SummaryViewModel created");
			DisplayName = "summary";
			//For designer support
		}

		#endregion

		#region Properties

		public decimal AvgConsumption
		{
			get { return _avgConsumption; }
			set
			{
				if (_avgConsumption != value)
				{
					_avgConsumption = value;
					NotifyOfPropertyChange(() => AvgConsumption);
				}
			}
		}

		public decimal MinConsumption
		{
			get { return _minConsumption; }
			set
			{
				if (_minConsumption != value)
				{
					_minConsumption = value;
					NotifyOfPropertyChange(() => MinConsumption);
				}
			}
		}

		public decimal MaxConsumption
		{
			get { return _maxConsumption; }
			set
			{
				if (_maxConsumption != value)
				{
					_maxConsumption = value;
					NotifyOfPropertyChange(() => MaxConsumption);

				}
			}
		}

		#endregion

		public void GoToAddFillup()
		{
			_navigationService.UriFor<EditFillupViewModel>().Navigate();
		}

		public void GoToSettings()
		{
			_messageBox.Show("not implemented");
		}
	}
}
