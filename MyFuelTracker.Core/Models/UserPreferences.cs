using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.Models
{
	public class UserSetttings
	{
		public UserSetttings()
		{
			FuelEconomyType = FuelEconomyType.L100Km;
			Locale = SupportedLocale.en_US;
		}

		public FuelEconomyType FuelEconomyType { get; set; }
		public SupportedLocale Locale { get; set; }
	}

	public enum SupportedLocale
	{
		en_US,
		ru_RU
	}

	public class UserSetttingsManager
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
	}

	public interface IFuelEconomyStrategyProvider
	{
		IFuelEconomyStrategy GetCurrentStrategy();
	}

	public class FuelEconomyStrategyProvider : IFuelEconomyStrategyProvider
	{
		private readonly UserSetttingsManager _manager;
		private Dictionary<FuelEconomyType, IFuelEconomyStrategy> _strategies = new Dictionary<FuelEconomyType, IFuelEconomyStrategy>
			{
				{ FuelEconomyType.Mpg, new MpgFuelEconomyStrategy()},
				{ FuelEconomyType.L100Km, new L100KmFuelEconomyStrategy()},
				{ FuelEconomyType.KmL, new KmLFuelEconomyStrategy()}
			};

		public FuelEconomyStrategyProvider(UserSetttingsManager manager)
		{
			_manager = manager;
		}

		public IFuelEconomyStrategy GetCurrentStrategy()
		{
			return _strategies[_manager.Settings.FuelEconomyType];
		}
	}

	public interface IFuelEconomyStrategy
	{
		string FuelEconomyUnit { get; }
		string DistanceUnit { get; }
		string VolumeUnit { get; }
		double CalculateEconomy(double distance, double volume);
	}

	public class MpgFuelEconomyStrategy : IFuelEconomyStrategy
	{
		public string FuelEconomyUnit { get { return "mpg"; } }
		public string DistanceUnit { get { return "mi"; } }
		public string VolumeUnit { get { return "gal"; } }
		public double CalculateEconomy(double distance, double volume)
		{
			return distance / volume;
		}
	}

	public class KmLFuelEconomyStrategy : IFuelEconomyStrategy
	{
		public string FuelEconomyUnit { get { return "km/L"; } }
		public string DistanceUnit { get { return "km"; } }
		public string VolumeUnit { get { return "L"; } }
		public double CalculateEconomy(double distance, double volume)
		{
			return distance / volume;
		}
	}

	public class L100KmFuelEconomyStrategy : IFuelEconomyStrategy
	{
		public string FuelEconomyUnit { get { return "L/100km"; } }
		public string DistanceUnit { get { return "km"; } }
		public string VolumeUnit { get { return "L"; } }
		public double CalculateEconomy(double distance, double volume)
		{
			return 100 * volume / distance;
		}
	}

	public enum FuelEconomyType
	{
		Mpg,
		KmL,
		L100Km
	}
}
