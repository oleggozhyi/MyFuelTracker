﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class SummaryViewModel : Screen, IHandle<FillupHistoryChangedEvent>, IAppBarItemsProvider
	{
		#region Fields

		public readonly DynamicAppBarButton AddFillupButton = new DynamicAppBarButton { IconUri = Icons.Add, Text = "add fillup" };
		private readonly DynamicAppBarButton[] _buttons;

		private const string L_100LM = "l/100km";
		private const string UAH = "гр";
		private readonly INavigationService _navigationService;
		private readonly IMessageBox _messageBox;
		private readonly IFillupService _fillupService;
		private readonly AppBarMenuModel _appBarMenuModel;
		private string _lastConsumption;
		private string _minConsumption;
		private string _maxConsumption;
		private string _allTimeAvgConsumption;
		private string _last4FillupsAvgConsumption;
		private string _allTimeAvgMonthCost;
		private string _lastMonthCost;
		private Brush _lastConsumptionBrush;
		private Brush _minConsumptionBrush;
		private Brush _maxConsumptionBrush;
		private Brush _allTimeAvgConsumptionBrush;
		private Brush _last4FillupsAvgConsumptionBrush;
		private string _avgFillupCost;
		private string _mostOftenFuelType;
		private string _lastFillupCost;
		private bool _historyEmpty = true;
		private int _hideStoryboardFrom = 2000;
		private bool _showNoHistoryMessage;
		private bool _statisticsReady;

		#endregion

		#region ctor

		public SummaryViewModel() { /* for design time support */ }

		public SummaryViewModel(INavigationService navigationService,
								IMessageBox messageBox,
								IFillupService fillupService,
								AppBarMenuModel appBarMenuModel,
								IEventAggregator eventAggregator)
		{
			_navigationService = navigationService;
			_messageBox = messageBox;
			_fillupService = fillupService;
			_appBarMenuModel = appBarMenuModel;
			Debug.WriteLine("SummaryViewModel created");
			DisplayName = "summary";
			eventAggregator.Subscribe(this);

			AddFillupButton.OnClick = GoToAddFillup;
			_buttons = new[] { AddFillupButton };

		}

		#endregion

		#region Properties

		public IEnumerable<DynamicAppBarButton> Buttons { get { return _buttons; } }

		public IEnumerable<DynamicAppBarItem> MenuItems
		{
			get { return _appBarMenuModel.MenuItems; }
		}

		public bool ShowNoHistoryMessage
		{
			get { return _showNoHistoryMessage; }
			set
			{
				if (value.Equals(_showNoHistoryMessage)) return;
				_showNoHistoryMessage = value;
				NotifyOfPropertyChange(() => ShowNoHistoryMessage);
			}
		}

		public bool StatisticsReady
		{
			get { return _statisticsReady; }
			set
			{
				if (value.Equals(_statisticsReady)) return;
				_statisticsReady = value;
				NotifyOfPropertyChange(() => StatisticsReady);
			}
		}

		public string ConsumptionDimension { get { return "L/100km"; } }

		public string CostDimension { get { return "hr"; } }

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

		public Brush LastConsumptionBrush
		{
			get { return _lastConsumptionBrush; }
			set
			{
				if (Equals(value, _lastConsumptionBrush)) return;
				_lastConsumptionBrush = value;
				NotifyOfPropertyChange(() => LastConsumptionBrush);
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

		public Brush MinConsumptionBrush
		{
			get { return _minConsumptionBrush; }
			set
			{
				if (Equals(value, _minConsumptionBrush)) return;
				_minConsumptionBrush = value;
				NotifyOfPropertyChange(() => MinConsumptionBrush);
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

		public Brush MaxConsumptionBrush
		{
			get { return _maxConsumptionBrush; }
			set
			{
				if (Equals(value, _maxConsumptionBrush)) return;
				_maxConsumptionBrush = value;
				NotifyOfPropertyChange(() => MaxConsumptionBrush);
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

		public Brush AllTimeAvgConsumptionBrush
		{
			get { return _allTimeAvgConsumptionBrush; }
			set
			{
				if (Equals(value, _allTimeAvgConsumptionBrush)) return;
				_allTimeAvgConsumptionBrush = value;
				NotifyOfPropertyChange(() => AllTimeAvgConsumptionBrush);
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

		public Brush Last4FillupsAvgConsumptionBrush
		{
			get { return _last4FillupsAvgConsumptionBrush; }
			set
			{
				if (Equals(value, _last4FillupsAvgConsumptionBrush)) return;
				_last4FillupsAvgConsumptionBrush = value;
				NotifyOfPropertyChange(() => Last4FillupsAvgConsumptionBrush);
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

		public string AvgFillupCost
		{
			get { return _avgFillupCost; }
			set
			{
				if (value == _avgFillupCost) return;
				_avgFillupCost = value;
				NotifyOfPropertyChange(() => AvgFillupCost);
			}
		}

		public string LastFillupCost
		{
			get { return _lastFillupCost; }
			set
			{
				if (value == _lastFillupCost) return;
				_lastFillupCost = value;
				NotifyOfPropertyChange(() => LastFillupCost);
			}
		}

		public string MostOftenFuelType
		{
			get { return _mostOftenFuelType; }
			set
			{
				if (value == _mostOftenFuelType) return;
				_mostOftenFuelType = value;
				NotifyOfPropertyChange(() => MostOftenFuelType);
			}
		}

		public int HideStoryboardFrom
		{
			get { return _hideStoryboardFrom; }
			set
			{
				if (value == _hideStoryboardFrom) return;
				_hideStoryboardFrom = value;
				NotifyOfPropertyChange(() => HideStoryboardFrom);
			}
		}

		#endregion

		#region methods

		public void GoToAddFillup()
		{
			_navigationService.UriFor<EditFillupViewModel>().Navigate();
		}

		public void GoToSettings()
		{
			_messageBox.Info("not implemented");
		}

		public async Task UpdateAsync()
		{
			HideStoryboardFrom = 0;
			var statistics = await _fillupService.GetStatisticsAsync();
			if (statistics == null)
			{
				ShowNoHistoryMessage = true;
				StatisticsReady = false;
				return;
			}
			StatisticsReady = true;
			ShowNoHistoryMessage = false;

			AllTimeAvgConsumption = statistics.AllTimeAvgConsumption.FormatForDisplay(2);
			AllTimeAvgConsumptionBrush = ColorHelper.AvgColor.ToBrush();

			MaxConsumption = statistics.MaxConsumption.FormatForDisplay(2);
			MaxConsumptionBrush = ColorHelper.MaxColor.ToBrush();

			MinConsumption = statistics.MinConsumption.FormatForDisplay(2);
			MinConsumptionBrush = ColorHelper.MinColor.ToBrush();

			Last4FillupsAvgConsumption = statistics.Last4FillupsAvgConsumption.FormatForDisplay(2);
			Last4FillupsAvgConsumptionBrush = ColorHelper.GetColor(statistics.Last4FillupsAvgConsumption,
																	statistics.MinConsumption,
																	statistics.AllTimeAvgConsumption,
																	statistics.MaxConsumption)
														.ToBrush();
			LastConsumption = statistics.LastConsumption.FormatForDisplay(2);
			LastConsumptionBrush = ColorHelper.GetColor(statistics.LastConsumption,
																	statistics.MinConsumption,
																	statistics.AllTimeAvgConsumption,
																	statistics.MaxConsumption)
														.ToBrush();



			AllTimeAvgMonthCost = statistics.AllTimeAvgMonthCost.FormatForDisplay(2);
			LastMonthCost = statistics.LastMonthCost.FormatForDisplay(2);

			AvgFillupCost = statistics.AvgFillupCost.FormatForDisplay(2);
			LastFillupCost = statistics.LastFillupCost.FormatForDisplay(2);

			MostOftenFuelType = statistics.MostOftenFuelType;

		}

		public async void Handle(FillupHistoryChangedEvent message)
		{
			await UpdateAsync();
		}

		#endregion
	}
}
