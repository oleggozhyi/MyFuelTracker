using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.ViewModels
{
	public class FillupHistoryItemViewModel
	{
		public FillupHistoryItem HistoryItem { get; set; }

		public FillupHistoryItemViewModel(FillupHistoryItem historyItem)
		{
			HistoryItem = historyItem;
		}

		public string Date
		{
			get { return HistoryItem.Fillup.Date.ToShortDateString(); }
		}

		public string FuelEconomy
		{
			get { return string.Format("{0:#.##}", HistoryItem.FuelEconomy) + "L/100km"; }
		}

		public Brush FillupBrush
		{
			get { return new SolidColorBrush(HistoryItem.IsGreaterThanAverage ? Colors.Red : Colors.Green); }
		}
	}
}
