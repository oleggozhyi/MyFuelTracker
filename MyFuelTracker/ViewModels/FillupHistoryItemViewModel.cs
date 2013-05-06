﻿using System;
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
		private bool _isPartial;

		public FillupHistoryItemViewModel() { /* for design time support */}

		public FillupHistoryItemViewModel(FillupHistoryItem historyItem, FuelConsumptionStatistics statistics)
		{
			_isPartial = historyItem.Fillup.IsPartial;
			FuelEconomy = historyItem.Consumption.HasValue ? historyItem.Consumption.Value.FormatForDisplay(2) : "<partial>";
			Date = historyItem.Fillup.Date.ToString("dd MMM yyyy");
			FillupBrush = GetFillupBrush(historyItem, statistics);

			Distance = (historyItem.Fillup.OdometerEnd - historyItem.Fillup.OdometerStart).FormatForDisplay(0);
			Volume = historyItem.Fillup.Volume.FormatForDisplay(2);
			Cost = (historyItem.Fillup.Volume * historyItem.Fillup.Price).FormatForDisplay(2);
			Petrol = historyItem.Fillup.Petrol;

			FillupDate = historyItem.Fillup.Date;
		}

		private Brush GetFillupBrush(FillupHistoryItem historyItem, FuelConsumptionStatistics statistics)
		{
			if (historyItem.Fillup.IsPartial)
				return new SolidColorBrush(Colors.Gray);

			double maxConsumptionHue = 0.0;
			double minConsumptionHue = 140.0;
			double avgConsumptionHue = 60;
			double hue;
			
			double consumption = historyItem.Consumption.Value;
			if (consumption >= statistics.MinConsumption &&
			    consumption < statistics.AllTimeAvgConsumption)
			{
				double d = (consumption - statistics.MinConsumption)/
				           (statistics.AllTimeAvgConsumption - statistics.MinConsumption);
				hue = minConsumptionHue - (minConsumptionHue - avgConsumptionHue)*d;
			}
			else
			{
				double d = (consumption - statistics.AllTimeAvgConsumption) /
					   (statistics.MaxConsumption - statistics.AllTimeAvgConsumption);
				hue = avgConsumptionHue - (avgConsumptionHue - maxConsumptionHue) * d;
			}

			var color = new HslColor {A = 1, H = hue, L = 0.5, S = 1}.ToColor();

			return new SolidColorBrush(color);
		}

		public DateTime FillupDate { get; set; }

		public string Petrol { get; set; }

		public string Date { get; set; }

		public string FuelEconomy { get; set; }

		public Brush FillupBrush { get; set; }

		public string Distance { get; set; }

		public string Cost { get; set; }

		public string Volume { get; set; }

		public string ConsumptionDimension { get { return _isPartial ? "" : "L/100km"; } }

		public string CostDimension { get { return "hr"; } }

		public string VolumeDimension { get { return "L"; } }

		public string DistanceDimension { get { return "km"; } }
	}
}
