using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class FillupHistoryItemViewModel
	{
		public FillupHistoryItem HistoryItem { get; set; }
		private bool _isPartial;

		public FillupHistoryItemViewModel() { /* for design time support */}

		public FillupHistoryItemViewModel(FillupHistoryItem historyItem, FuelConsumptionStatistics statistics)
		{
			HistoryItem = historyItem;
			_isPartial = historyItem.Fillup.IsPartial;
			Consumption = historyItem.Consumption.HasValue ? historyItem.Consumption.Value.FormatForDisplay(2) : "<partial>";
			Date = historyItem.Fillup.Date.ToString("dd MMM yyyy");
			FillupBrush = GetFillupBrush(historyItem, statistics);

			Distance = (historyItem.Fillup.OdometerEnd - historyItem.Fillup.OdometerStart).FormatForDisplay(0);
			Volume = historyItem.Fillup.Volume.FormatForDisplay(2);
			Cost = (historyItem.Fillup.Volume * historyItem.Fillup.Price).FormatForDisplay(2);
			FuelType = historyItem.Fillup.FuelType;

			FillupDate = historyItem.Fillup.Date;
			OdometerStart = HistoryItem.Fillup.OdometerStart.FormatForDisplay(0);
			OdometerEnd = HistoryItem.Fillup.OdometerEnd.FormatForDisplay(0);
			Price = HistoryItem.Fillup.Price.FormatForDisplay(2);
			ShowConsumption = !historyItem.Fillup.IsPartial;
			IsPartialFillup = historyItem.Fillup.IsPartial ? "yes" : "no";
		}

		private Brush GetFillupBrush(FillupHistoryItem historyItem, FuelConsumptionStatistics statistics)
		{
			if (historyItem.Fillup.IsPartial)
				return new SolidColorBrush(Colors.Gray);

			double consumption = historyItem.Consumption.Value;
			var color = ColorHelper.GetColor(consumption, statistics.MinConsumption, 
											statistics.AllTimeAvgConsumption, statistics.MaxConsumption);
			
			return new SolidColorBrush(color);
		}

		public DateTime FillupDate { get; set; }

		public string FuelType { get; set; }

		public string Price { get; set; }

		public string Date { get; set; }

		public string Consumption { get; set; }

		public Brush FillupBrush { get; set; }

		public string Distance { get; set; }

		public string Cost { get; set; }

		public string Volume { get; set; }

		public string ConsumptionDimension { get { return _isPartial ? "" : "L/100km"; } }

		public string CostDimension { get { return "hr"; } }

		public string VolumeDimension { get { return "L"; } }

		public string DistanceDimension { get { return "km"; } }

		public string OdometerStart { get; set; }

		public string OdometerEnd { get; set; }

		public string IsPartialFillup { get; set; }
		public bool ShowConsumption { get; set; }
	}
}
