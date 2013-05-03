using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class SummaryViewModel : Screen, IUpdatable
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IMessageBox _messageBox;
		private readonly IFillupService _fillupService;
		private readonly IStatisticsService _statisticsService;
		private double _lastConsumption;
		private double _minConsumption;
		private double _maxConsumption;
		private double _allTimeAvgConsumption;
		private double _last4FillupsAvgConsumption;
		private double _allTimeAvgMonthCost;
		private double _lastMonthCost;

		#endregion

		#region ctor

		public SummaryViewModel(INavigationService navigationService, 
								IMessageBox messageBox, 
								IFillupService fillupService, 
								IStatisticsService statisticsService)
		{
			_navigationService = navigationService;
			_messageBox = messageBox;
			_fillupService = fillupService;
			_statisticsService = statisticsService;
			Debug.WriteLine("SummaryViewModel created");
			DisplayName = "summary";
			//For designer support
		}

		#endregion

		#region Properties

		public double LastConsumption
		{
			get { return _lastConsumption; }
			set
			{
				if (value.Equals(_lastConsumption)) return;
				_lastConsumption = value;
				NotifyOfPropertyChange(() => LastConsumption);
			}
		}

		public double MinConsumption
		{
			get { return _minConsumption; }
			set
			{
				if (value.Equals(_minConsumption)) return;
				_minConsumption = value;
				NotifyOfPropertyChange(() => MinConsumption);
			}
		}

		public double MaxConsumption
		{
			get { return _maxConsumption; }
			set
			{
				if (value.Equals(_maxConsumption)) return;
				_maxConsumption = value;
				NotifyOfPropertyChange(() => MaxConsumption);
			}
		}

		public double AllTimeAvgConsumption
		{
			get { return _allTimeAvgConsumption; }
			set
			{
				if (value.Equals(_allTimeAvgConsumption)) return;
				_allTimeAvgConsumption = value;
				NotifyOfPropertyChange(() => AllTimeAvgConsumption);
			}
		}

		public double Last4FillupsAvgConsumption
		{
			get { return _last4FillupsAvgConsumption; }
			set
			{
				if (value.Equals(_last4FillupsAvgConsumption)) return;
				_last4FillupsAvgConsumption = value;
				NotifyOfPropertyChange(() => Last4FillupsAvgConsumption);
			}
		}

		public double AllTimeAvgMonthCost
		{
			get { return _allTimeAvgMonthCost; }
			set
			{
				if (value.Equals(_allTimeAvgMonthCost)) return;
				_allTimeAvgMonthCost = value;
				NotifyOfPropertyChange(() => AllTimeAvgMonthCost);
			}
		}

		public double LastMonthCost
		{
			get { return _lastMonthCost; }
			set
			{
				if (value.Equals(_lastMonthCost)) return;
				_lastMonthCost = value;
				NotifyOfPropertyChange(() => LastMonthCost);
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

		public async Task UpdateAsync()
		{
			var fillupHistoryItems = await _fillupService.GetHistoryAsync();
			var statistics = await _statisticsService.CalculateStatisticsAsync(fillupHistoryItems);

			this.AllTimeAvgConsumption = statistics.AllTimeAvgConsumption;
			this.AllTimeAvgMonthCost = statistics.AllTimeAvgMonthCost;
			this.Last4FillupsAvgConsumption = statistics.Last4FillupsAvgConsumption;
			this.LastConsumption = statistics.LastConsumption;
			this.LastMonthCost = statistics.LastMonthCost;
			this.MaxConsumption = statistics.MaxConsumption;
			this.MinConsumption = statistics.MinConsumption;
		}
	}
}
