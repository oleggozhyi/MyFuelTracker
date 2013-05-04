﻿using System;
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

		private const string L_100LM = "l/100km";
		private const string UAH = "гр";
		private readonly INavigationService _navigationService;
		private readonly IMessageBox _messageBox;
		private readonly IFillupService _fillupService;
		private readonly IStatisticsService _statisticsService;
		private string _lastConsumption;
		private string _minConsumption;
		private string _maxConsumption;
		private string _allTimeAvgConsumption;
		private string _last4FillupsAvgConsumption;
		private string _allTimeAvgMonthCost;
		private string _lastMonthCost;

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
		}

		#endregion

		#region Properties

		public string LastConsumption
		{
			get { return _lastConsumption; }
			set
			{
				if (value.Equals(_lastConsumption)) return;
				_lastConsumption = value;
				NotifyOfPropertyChange(() => LastConsumption);
			}
		}

		public string MinConsumption
		{
			get { return _minConsumption; }
			set
			{
				if (value.Equals(_minConsumption)) return;
				_minConsumption = value;
				NotifyOfPropertyChange(() => MinConsumption);
			}
		}

		public string MaxConsumption
		{
			get { return _maxConsumption; }
			set
			{
				if (value.Equals(_maxConsumption)) return;
				_maxConsumption = value;
				NotifyOfPropertyChange(() => MaxConsumption);
			}
		}

		public string AllTimeAvgConsumption
		{
			get { return _allTimeAvgConsumption; }
			set
			{
				if (value.Equals(_allTimeAvgConsumption)) return;
				_allTimeAvgConsumption = value;
				NotifyOfPropertyChange(() => AllTimeAvgConsumption);
			}
		}

		public string Last4FillupsAvgConsumption
		{
			get { return _last4FillupsAvgConsumption; }
			set
			{
				if (value.Equals(_last4FillupsAvgConsumption)) return;
				_last4FillupsAvgConsumption = value;
				NotifyOfPropertyChange(() => Last4FillupsAvgConsumption);
			}
		}

		public string AllTimeAvgMonthCost
		{
			get { return _allTimeAvgMonthCost; }
			set
			{
				if (value.Equals(_allTimeAvgMonthCost)) return;
				_allTimeAvgMonthCost = value;
				NotifyOfPropertyChange(() => AllTimeAvgMonthCost);
			}
		}

		public string LastMonthCost
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

		protected async override void OnActivate()
		{
			base.OnActivate();
			await UpdateAsync();
		}

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

			this.AllTimeAvgConsumption = statistics.AllTimeAvgConsumption.FormatForDisplay(L_100LM);
			this.AllTimeAvgMonthCost = statistics.AllTimeAvgMonthCost.FormatForDisplay(UAH);
			this.Last4FillupsAvgConsumption = statistics.Last4FillupsAvgConsumption.FormatForDisplay(L_100LM);
			this.LastConsumption = statistics.LastConsumption.FormatForDisplay(L_100LM);
			this.LastMonthCost = statistics.LastMonthCost.FormatForDisplay(UAH);
			this.MaxConsumption = statistics.MaxConsumption.FormatForDisplay(L_100LM);
			this.MinConsumption = statistics.MinConsumption.FormatForDisplay(L_100LM);
		}
	}
}
