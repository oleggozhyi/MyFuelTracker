using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Helpers;

namespace MyFuelTracker.ViewModels
{
	public class FillupHistoryItemViewModel
	{
		private readonly Units _units;
		public FillupHistoryItem HistoryItem { get; set; }
		private bool _isPartial;

		public FillupHistoryItemViewModel() { /* for design time support */}

		public FillupHistoryItemViewModel(FillupHistoryItem historyItem, Statistics statistics, Units units)
		{
			_units = units;
			HistoryItem = historyItem;
			_isPartial = historyItem.Fillup.IsPartial;
			FuelEconomy = historyItem.FuelEconomy.HasValue ? historyItem.FuelEconomy.Value.FormatForDisplay(2) : "<partial>";
			Date = historyItem.Fillup.Date.ToString("dd MMMM yyyy", Thread.CurrentThread.CurrentUICulture);
			FillupBrush = GetFillupBrush(historyItem, statistics);

			Distance = (historyItem.Fillup.OdometerEnd - historyItem.Fillup.OdometerStart).FormatForDisplay(0);
			Volume = historyItem.Fillup.Volume.FormatForDisplay(2);
			Cost = (historyItem.Fillup.Volume * historyItem.Fillup.Price).FormatForDisplay(2);
			FuelType = historyItem.Fillup.FuelType;

			FillupDate = historyItem.Fillup.Date;
			OdometerStart = HistoryItem.Fillup.OdometerStart.FormatForDisplay(0);
			OdometerEnd = HistoryItem.Fillup.OdometerEnd.FormatForDisplay(0);
			Price = HistoryItem.Fillup.Price.FormatForDisplay(2);
			ShowfuelEconomy = !historyItem.Fillup.IsPartial;
			IsPartialFillup = historyItem.Fillup.IsPartial ? "yes" : "no";
		}

		private Brush GetFillupBrush(FillupHistoryItem historyItem, Statistics statistics)
		{
			if (historyItem.Fillup.IsPartial)
                return new SolidColorBrush(Colors.Gray);

			double fuelEconomy = historyItem.FuelEconomy.Value;
			var color = ColorHelper.GetColor(fuelEconomy, statistics.MinFuelEconomy,
                                            statistics.AllTimeAvgFuelEconomy, statistics.MaxFuelEconomy);
			
			return new SolidColorBrush(color);
		}

		public DateTime FillupDate { get; set; }

		public string FuelType { get; set; }

		public string Price { get; set; }

		public string Date { get; set; }

		public string FuelEconomy { get; set; }

        public Brush FillupBrush { get; set; }

		public string Distance { get; set; }

		public string Cost { get; set; }

		public string Volume { get; set; }

		public string FuelEconomyDimension { get { return _isPartial ? "" : _units.Economy; } }

		public string CostDimension { get { return _units.Currency; } }

		public string VolumeDimension { get { return _units.Volume; } }

		public string DistanceDimension { get { return _units.Distance; } }

		public string OdometerStart { get; set; }

		public string OdometerEnd { get; set; }

		public string IsPartialFillup { get; set; }
		public bool ShowfuelEconomy { get; set; }
	}
}
