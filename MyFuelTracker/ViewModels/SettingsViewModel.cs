using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure.Events;

namespace MyFuelTracker.ViewModels
{
	public class SettingsViewModel : Screen
	{
		#region Fields

		private readonly IUserSetttingsManager _setttingsManager;
		private readonly IEventAggregator _eventAggregator;
		private readonly IFillupService _fillupService;

		#endregion

		public SettingsViewModel(IUserSetttingsManager setttingsManager, 
								IEventAggregator eventAggregator,
								IFillupService fillupService)
		{
			_setttingsManager = setttingsManager;
			_eventAggregator = eventAggregator;
			_fillupService = fillupService;
		}

		#region Propeties

		public FuelEconomyType FuelEconomyType
		{
			get { return _setttingsManager.Settings.FuelEconomyType; }
			set
			{
				if (value == _setttingsManager.Settings.FuelEconomyType) return;
				_setttingsManager.Settings.FuelEconomyType = value;
				SaveSettings();
				NotifyOfPropertyChange(() => FuelEconomyType);
			}
		}

		public SupportedLocale Locale
		{
			get { return _setttingsManager.Settings.Locale; }
			set
			{
				if (value == _setttingsManager.Settings.Locale) return;
				_setttingsManager.Settings.Locale = value;
				SaveSettings();
				NotifyOfPropertyChange(() => Locale);
			}
		}

		#endregion

		#region methods

		public void SaveSettings()
		{
			Task.Run(() =>
				{
					_setttingsManager.Save();
					_fillupService.ClearCache();
					_eventAggregator.Publish(new FillupHistoryChangedEvent());
				});
		}

		#endregion


	}
}
