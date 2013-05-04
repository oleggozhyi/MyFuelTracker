using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class EditFillupViewModel : Screen
	{
		#region Fields

		private readonly INavigationService _navigationService;
		private readonly ILog _log;
		private readonly IMessageBox _messageBox;
		private readonly IFillupService _fillupService;
		private readonly IEventAggregator _eventAggregator;

		private bool _isPartial;
		private DateTime _date;
		private string _volume;
		private string _price;
		private string _odometerStart;
		private string _odometerEnd;
		private Fillup _fillup;

		#endregion

		#region ctor

		public EditFillupViewModel(INavigationService navigationService,
									ILog log,
									IMessageBox messageBox,
									IFillupService fillupService,
									IEventAggregator eventAggregator)
		{
			_navigationService = navigationService;
			_log = log;
			_messageBox = messageBox;
			_fillupService = fillupService;
			_eventAggregator = eventAggregator;
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

		public string Volume
		{
			get { return _volume; }
			set
			{
				if (value == _volume) return;
				_volume = value;
				NotifyOfPropertyChange(() => Volume);
			}
		}

		public string Price
		{
			get { return _price; }
			set
			{
				if (value == _price) return;
				_price = value;
				NotifyOfPropertyChange(() => Price);
			}
		}

		public string OdometerStart
		{
			get { return _odometerStart; }
			set
			{
				if (value == _odometerStart) return;
				_odometerStart = value;
				NotifyOfPropertyChange(() => OdometerStart);
			}
		}

		public string OdometerEnd
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

		protected async override void OnActivate()
		{
			base.OnActivate();

			_fillup = await _fillupService.CreateNewFillupAsync();
			Date = _fillup.Date;
			IsPartial = _fillup.IsPartial;
			OdometerEnd = _fillup.OdometerEnd.FormatForDisplay();
			OdometerStart = _fillup.OdometerStart.FormatForDisplay();
		}

		public async void SaveFillup()
		{
			try
			{
				_fillup.Date = Date;
				_fillup.IsPartial = IsPartial;
				_fillup.OdometerEnd = OdometerEnd.GetPositiveDoubleFor("odometer end");
				_fillup.OdometerStart = OdometerStart.GetPositiveDoubleFor("odometer start");
				_fillup.Volume = Volume.GetPositiveDoubleFor("volume");
				_fillup.Price = Price.GetPositiveDoubleFor("price");
				ValidateFillup(_fillup);
			}
			catch (ValidationException ex)
			{
				_messageBox.Show(ex.Message);
				return;
			}
			await _fillupService.SaveFillupAsync(_fillup);
			_eventAggregator.Publish(new FillupHistoryChangedEvent());
			_eventAggregator.Publish(new FillupItemChangedEvent());
			_navigationService.GoBack();
		}

		public void Cancel()
		{
			_log.Info("Cancel editing fillup and go back");
			_navigationService.GoBack();
		}

		private static void ValidateFillup(Fillup f)
		{
			if (f.OdometerStart >= f.OdometerEnd)
				throw new ValidationException("odometer start should be less than odometer end");
		}

		#endregion
	}
}
