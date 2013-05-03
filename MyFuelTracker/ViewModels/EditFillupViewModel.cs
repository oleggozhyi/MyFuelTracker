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
		private decimal _volume;
		private decimal _price;
		private decimal _odometerStart;
		private decimal _odometerEnd;
		private bool _isPartial;

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

		public decimal Volume
		{
			get { return _volume; }
			set
			{
				if (value == _volume) return;
				_volume = value;
				NotifyOfPropertyChange(() => Volume);
			}
		}

		public decimal Price
		{
			get { return _price; }
			set
			{
				if (value == _price) return;
				_price = value;
				NotifyOfPropertyChange(() => Price);
			}
		}

		public decimal OdometerStart
		{
			get { return _odometerStart; }
			set
			{
				if (value == _odometerStart) return;
				_odometerStart = value;
				NotifyOfPropertyChange(() => OdometerStart);
			}
		}

		public decimal OdometerEnd
		{
			get { return _odometerEnd; }
			set
			{
				if (value == _odometerEnd) return;
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
