using MyFuelTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFuelTracker.Core.ViewModels
{
	public class FillupItemViewModel : ViewModelBase
	{
		#region Fields

		private Fillup _fillup;
		private DateTime _date;
		private decimal _volume;
		private decimal _price;
		private decimal _odometer;
		private IMyFuelTrackerApp _app;

		#endregion

		#region ctor

		public FillupItemViewModel() : this (null)
		{ 
		}

		public FillupItemViewModel(Fillup fillup)
		{
			bool isNew = fillup == null;
			_fillup = fillup ?? new Fillup();
			Date = isNew ? DateTime.Now : fillup.Date;
			Odometer = isNew ? 0m : fillup.Odometer;
			Volume = isNew ? 0m : fillup.Volume;
			Price = isNew ? 0m : fillup.Price;
		}

		#endregion

		#region properties

		public DateTime Date
		{
			get { return _date; }
			set { SetProperty(value, ref _date); }
		}

		public decimal Volume
		{
			get { return _volume; }
			set { SetProperty(value, ref _volume); }
		}

		public decimal Price
		{
			get { return _price; }
			set { SetProperty(value, ref _price); }
		}

		public decimal Odometer
		{
			get { return _odometer; }
			set { SetProperty(value, ref _odometer); }
		} 

		#endregion

		#region methods

		public Fillup GetFillup()
		{
			_fillup.Date = Date;
			_fillup.Odometer = Odometer;
			_fillup.Price = Price;
			_fillup.Odometer = Odometer;
			_fillup.Volume = Volume;

			return _fillup;
		} 

		#endregion
	}
}
