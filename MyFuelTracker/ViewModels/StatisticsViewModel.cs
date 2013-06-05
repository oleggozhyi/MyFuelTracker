using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Events;
using MyFuelTracker.Infrastructure.Helpers;
using MyFuelTracker.Infrastructure.UiServices;

namespace MyFuelTracker.ViewModels
{
	public class StatisticsViewModel : Screen, IHandle<FillupHistoryChangedEvent>, IAppBarItemsProvider
	{
		#region Fields

		private bool _loadingFirstTime = true;
		public readonly DynamicAppBarButton AddFillupButton = new DynamicAppBarButton { IconUri = Icons.Add, Text = "add fillup" };
		private readonly DynamicAppBarButton[] _buttons;

		private const string L_100LM = "l/100km";
		private const string UAH = "гр";
		private readonly INavigationService _navigationService;
		private readonly IMessageBox _messageBox;
		private readonly IFillupService _fillupService;
		private readonly AppBarMenuModel _appBarMenuModel;
		private string _lastFuelEconomy;
		private string _minFuelEconomy;
		private string _maxFuelEconomy;
		private string _allTimeAvgFuelEconomy;
		private string _last4FillupsAvgFuelEconomy;
		private string _lastMonthCost;
		private Brush _lastFuelEconomyBrush;
		private Brush _minFuelEconomyBrush;
		private Brush _maxFuelEconomyBrush;
		private Brush _allTimeAvgFuelEconomyBrush;
		private Brush _last4FillupsAvgFuelEconomyBrush;
		private string _avgFillupCost;
		private string _mostOftenFuelType;
		private string _lastFillupCost;
		private bool _showNoHistoryMessage;

		#endregion

		#region ctor

		public StatisticsViewModel() { /* for design time support */ }

		public StatisticsViewModel(INavigationService navigationService,
								IMessageBox messageBox,
								IFillupService fillupService,
								AppBarMenuModel appBarMenuModel,
								IEventAggregator eventAggregator)
		{
			_navigationService = navigationService;
			_messageBox = messageBox;
			_fillupService = fillupService;
			_appBarMenuModel = appBarMenuModel;
			Debug.WriteLine("StatisticsViewModel created");
			DisplayName = "statistics";
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

		public string ConsumptionDimension { get { return "L/100km"; } }

		public string CostDimension { get { return "hr"; } }

		public string LastFuelEconomy
		{
			get { return _lastFuelEconomy; }
			set
			{
				if (value.Equals(_lastFuelEconomy)) return;
				_lastFuelEconomy = value;
				NotifyOfPropertyChange(() => LastFuelEconomy);
			}
		}

		public Brush LastFuelEconomyBrush
		{
			get { return _lastFuelEconomyBrush; }
			set
			{
				if (Equals(value, _lastFuelEconomyBrush)) return;
				_lastFuelEconomyBrush = value;
				NotifyOfPropertyChange(() => LastFuelEconomyBrush);
			}
		}


		public string MinFuelEconomy
		{
			get { return _minFuelEconomy; }
			set
			{
				if (value.Equals(_minFuelEconomy)) return;
				_minFuelEconomy = value;
				NotifyOfPropertyChange(() => MinFuelEconomy);
			}
		}

		public Brush MinFuelEconomyBrush
		{
			get { return _minFuelEconomyBrush; }
			set
			{
				if (Equals(value, _minFuelEconomyBrush)) return;
				_minFuelEconomyBrush = value;
				NotifyOfPropertyChange(() => MinFuelEconomyBrush);
			}
		}

		public string MaxFuelEconomy
		{
			get { return _maxFuelEconomy; }
			set
			{
				if (value.Equals(_maxFuelEconomy)) return;
				_maxFuelEconomy = value;
				NotifyOfPropertyChange(() => MaxFuelEconomy);
			}
		}

		public Brush MaxFuelEconomyBrush
		{
			get { return _maxFuelEconomyBrush; }
			set
			{
				if (Equals(value, _maxFuelEconomyBrush)) return;
				_maxFuelEconomyBrush = value;
				NotifyOfPropertyChange(() => MaxFuelEconomyBrush);
			}
		}

		public string AllTimeAvgFuelEconomy
		{
			get { return _allTimeAvgFuelEconomy; }
			set
			{
				if (value.Equals(_allTimeAvgFuelEconomy)) return;
				_allTimeAvgFuelEconomy = value;
				NotifyOfPropertyChange(() => AllTimeAvgFuelEconomy);
			}
		}

		public Brush AllTimeAvgFuelEconomyBrush
		{
			get { return _allTimeAvgFuelEconomyBrush; }
			set
			{
				if (Equals(value, _allTimeAvgFuelEconomyBrush)) return;
				_allTimeAvgFuelEconomyBrush = value;
				NotifyOfPropertyChange(() => AllTimeAvgFuelEconomyBrush);
			}
		}

		public string Last4FillupsAvgFuelEconomy
		{
			get { return _last4FillupsAvgFuelEconomy; }
			set
			{
				if (value.Equals(_last4FillupsAvgFuelEconomy)) return;
				_last4FillupsAvgFuelEconomy = value;
				NotifyOfPropertyChange(() => Last4FillupsAvgFuelEconomy);
			}
		}

		public Brush Last4FillupsAvgFuelEconomyBrush
		{
			get { return _last4FillupsAvgFuelEconomyBrush; }
			set
			{
				if (Equals(value, _last4FillupsAvgFuelEconomyBrush)) return;
				_last4FillupsAvgFuelEconomyBrush = value;
				NotifyOfPropertyChange(() => Last4FillupsAvgFuelEconomyBrush);
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
			var statistics = await _fillupService.GetStatisticsAsync();

			IsolatedStorageSettings.ApplicationSettings["statistics"]=  statistics;
			IsolatedStorageSettings.ApplicationSettings.Save();

			Update(statistics);

		}

		private void Update(Statistics statistics)
		{
			if (statistics == null)
			{
				ShowNoHistoryMessage = true;
				return;
			}
			ShowNoHistoryMessage = false;

			AllTimeAvgFuelEconomy = statistics.AllTimeAvgFuelEconomy.FormatForDisplay(2);
			AllTimeAvgFuelEconomyBrush = ColorHelper.AvgColor.ToBrush();

			MaxFuelEconomy = statistics.MaxFuelEconomy.FormatForDisplay(2);
			MaxFuelEconomyBrush = ColorHelper.MaxColor.ToBrush();

			MinFuelEconomy = statistics.MinFuelEconomy.FormatForDisplay(2);
			MinFuelEconomyBrush = ColorHelper.MinColor.ToBrush();

			Last4FillupsAvgFuelEconomy = statistics.Last4FillupsAvgFuelEconomy.FormatForDisplay(2);
			Last4FillupsAvgFuelEconomyBrush = ColorHelper.GetColor(statistics.Last4FillupsAvgFuelEconomy,
																	statistics.MinFuelEconomy,
																	statistics.AllTimeAvgFuelEconomy,
																	statistics.MaxFuelEconomy)
														.ToBrush();
			LastFuelEconomy = statistics.LastFuelEconomy.FormatForDisplay(2);
			LastFuelEconomyBrush = ColorHelper.GetColor(statistics.LastFuelEconomy,
																	statistics.MinFuelEconomy,
																	statistics.AllTimeAvgFuelEconomy,
																	statistics.MaxFuelEconomy)
														.ToBrush();



			LastMonthCost = statistics.LastMonthCost.FormatForDisplay(2);

			AvgFillupCost = statistics.AvgFillupCost.FormatForDisplay(2);
			LastFillupCost = statistics.LastFillupCost.FormatForDisplay(2);

			MostOftenFuelType = statistics.MostOftenFuelType;
	}

		public async void Handle(FillupHistoryChangedEvent message)
		{
			if (_loadingFirstTime)
			{
				_loadingFirstTime = false;
				Statistics statistics;
				if (IsolatedStorageSettings.ApplicationSettings.TryGetValue("statistics", out statistics))
				{
					Update(statistics);
					return;
				}
				
			}
			await UpdateAsync();
		}

		#endregion
	}
}
