using System;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;

namespace MyFuelTracker.ViewModels
{
	public class EditFillupViewModel : PropertyChangedBase
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly ILog _log;
		private readonly IFillupService _fillupService;
		private DateTime _date;
		private double _volume;
		private double _price;
		private double _odometerStart;
		private double _odometerEnd;
		private bool _isPartial;
		private const double EPSILON = 0.01;

		#endregion

		#region ctor

		public EditFillupViewModel(INavigationService navigationService, 
									ILog log,
									IFillupService fillupService)
		{
			_navigationService = navigationService;
			_log = log;
			_fillupService = fillupService;
			_date = DateTime.Now;
		}

		#endregion

		#region Properties

		public DateTime Date
		{
			get { return _date; }
			set
			{
				if (value.Equals(_date)) return;
				_date = value;
				NotifyOfPropertyChange(() => Date);
			}
		}

		public double Volume
		{
			get { return _volume; }
			set
			{
				if (Math.Abs(value - _volume) < EPSILON) return;
				_volume = value;
				NotifyOfPropertyChange(() => Volume);
			}
		}

		public double Price
		{
			get { return _price; }
			set
			{
				if (Math.Abs(value - _price) < EPSILON) return;
				_price = value;
				NotifyOfPropertyChange(() => Price);
			}
		}

		public double OdometerStart
		{
			get { return _odometerStart; }
			set
			{
				if (Math.Abs(value - _odometerStart) < EPSILON) return;
				_odometerStart = value;
				NotifyOfPropertyChange(() => OdometerStart);
			}
		}

		public double OdometerEnd
		{
			get { return _odometerEnd; }
			set
			{
				if (Math.Abs(value - _odometerEnd) < EPSILON) return;
				_odometerEnd = value;
				NotifyOfPropertyChange(() => OdometerEnd);
			}
		}

		public bool IsPartial
		{
			get { return _isPartial; }
			set
			{
				if (value.Equals(_isPartial)) return;
				_isPartial = value;
				NotifyOfPropertyChange(() => IsPartial);
			}
		}

		#endregion

		#region methods

		public void SaveFillup()
		{
			_log.Info("Start saving fillup");
			var fillup = new Fillup
			{
				Date = Date,
				IsPartial = IsPartial,
				OdometerEnd = OdometerEnd,
				OdometerStart = OdometerStart,
				Price = Price,
				Volume = Volume
			};

			_fillupService.SaveFillupAsync(fillup);
			_navigationService.GoBack();
		}

		public void Cancel()
		{
			_log.Info("Cancel editing fillup and go back");
			_navigationService.GoBack();
		}

		#endregion
	}
}
