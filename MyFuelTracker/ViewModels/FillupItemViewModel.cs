using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using System;

namespace MyFuelTracker.ViewModels
{
	public class FillupItemViewModel : PropertyChangedBase
	{
		#region Fields

		private Fillup _fillup;
		private DateTime _date;
		private decimal _volume;
		private decimal _price;
		private decimal _odometer;

		#endregion

		#region ctor

		public FillupItemViewModel()
			: this(null)
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
			set
			{
				_date = value;
				NotifyOfPropertyChange(() => Date);
			}
		}

		public decimal Volume
		{
			get { return _volume; }
			set
			{
				_volume = value;
				NotifyOfPropertyChange(() => Volume);
			}
		}

		public decimal Price
		{
			get { return _price; }
			set
			{
				_price = value;
				NotifyOfPropertyChange(() => Price);
			}
		}

		public decimal Odometer
		{
			get { return _odometer; }
			set
			{
				_odometer = value;
				NotifyOfPropertyChange(() => Odometer);
			}
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
