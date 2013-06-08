using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Resources;

namespace MyFuelTracker.Infrastructure
{


	public class UserSetttingsManager : IUserSetttingsManager
	{
		public UserSetttings Settings
		{
			get
			{
				UserSetttings settings;
				if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue("UserSetttings", out settings))
				{
					IsolatedStorageSettings.ApplicationSettings["UserSetttings"] = settings
																				   = new UserSetttings();
					IsolatedStorageSettings.ApplicationSettings.Save();
				}
				return settings;
			}
		}

		public CultureInfo GetCurrentCulture()
		{
			switch (Settings.Locale)
			{
				case SupportedLocale.en_US: return new CultureInfo("en-US");
				case SupportedLocale.ru_RU: return new CultureInfo("ru-RU");
				default: throw new ArgumentOutOfRangeException();
			}
		}


		public void Save()
		{
			IsolatedStorageSettings.ApplicationSettings.Save();
		}

		public Units GetCurrentUnits()
		{
			var units = new Units
				{
					Currency = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol,

					Distance =Settings.FuelEconomyType == FuelEconomyType.Mpg 
								  ? AppResources.Units_Miles 
								  : AppResources.Units_Kilometers,
					Volume = Settings.FuelEconomyType == FuelEconomyType.Mpg 
					              ? AppResources.Units_Gallon 
								  : AppResources.Units_Litres,
					Economy = Settings.FuelEconomyType == FuelEconomyType.Mpg
						          ? AppResources.Units_Mpg
						          : Settings.FuelEconomyType == FuelEconomyType.KmL
							            ? AppResources.Units_KmL
							            : AppResources.Units_L100km
				};
			return units;
		}
	}
}
