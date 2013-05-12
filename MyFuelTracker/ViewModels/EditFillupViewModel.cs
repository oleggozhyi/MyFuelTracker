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
	public class EditFillupViewModel : Screen, IHandle<FuelTypeAddedEvent>
	{
		#region Fields

		private const double DISTANCE_TRESHOLD = 2000.0;
		private const double VOLUME_TRESHOLD = 1000.0;
		private const double CONSUMPTION_TRESHOLD = 25.0;

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
		private string _fuelType;
		private List<string> _fuelTypes;
		private IEnumerable<FillupHistoryItem> _historyItems;

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
			_eventAggregator.Subscribe(this);
			FuelTypes = new List<string>();
		}

		#endregion

		#region Properties

		public string FillupId { get; set; }

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

		public string FuelType
		{
			get { return _fuelType; }
			set
			{
				if (value == _fuelType) return;
				_fuelType = value;
				NotifyOfPropertyChange(() => FuelType);
			}
		}

		public List<string> FuelTypes
		{
			get { return _fuelTypes; }
			set
			{
				if (value == _fuelTypes) return;
				_fuelTypes = value;
				NotifyOfPropertyChange(() => FuelTypes);
			}
		}

		#endregion

		#region methods

		protected async override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			if (FillupId != null)
			{
				_fillup = await _fillupService.GetFillupAsync(Guid.Parse(FillupId));
				DisplayName = "edit fillup";
			}
			else
			{
				_fillup = await _fillupService.CreateNewFillupAsync();
				DisplayName = "add fillup";
			}

			_historyItems = await _fillupService.GetHistoryAsync();
			Date = _fillup.Date;
			IsPartial = _fillup.IsPartial;
			OdometerEnd = _fillup.OdometerEnd.FormatForDisplay(0);
			OdometerStart = _fillup.OdometerStart.FormatForDisplay(0);
			Volume = _fillup.Volume == default(double) ? String.Empty : _fillup.Volume.FormatForDisplay(2);
			var fuelTypes = _historyItems.Select(i => i.Fillup.FuelType).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

			FuelTypes = fuelTypes;
			Price = _fillup.Price.FormatForDisplay(2);
			FuelType = _fillup.FuelType;
		}

		public async void SaveFillup()
		{
			try
			{
				_fillup.Date = Date;
				_fillup.FuelType = FuelType;
				_fillup.IsPartial = IsPartial;
				_fillup.OdometerEnd = OdometerEnd.GetPositiveDoubleFor("previos odometer");
				_fillup.OdometerStart = OdometerStart.GetPositiveDoubleFor("current odometer");
				_fillup.Volume = Volume.GetPositiveDoubleFor("volume");
				_fillup.Price = Price.GetPositiveDoubleFor("price");
				ValidateFillup(_fillup);
				bool userCancelled = CheckForExtremalValues(_fillup);
				if (userCancelled)
					return;
			}
			catch (ValidationException ex)
			{
				_messageBox.Show(ex.Message, "cannot save fillup");
				return;
			}
			await _fillupService.SaveFillupAsync(_fillup);
			_eventAggregator.Publish(new FillupHistoryChangedEvent());
			_eventAggregator.Publish(new FillupItemChangedEvent());
			_navigationService.GoBack();
		}

		private bool CheckForExtremalValues(Fillup fillup)
		{
			bool cancel = false;
			double distance = fillup.OdometerEnd - fillup.OdometerStart;
			double consumption = fillup.Volume*100.0/distance;
			if (distance > DISTANCE_TRESHOLD)
			{
				cancel = !_messageBox.Confirm("The distance seems to be too big (" + distance.FormatForDisplay(0) + "km), are you sure to save this fillup? ", "Please confirm");
				if (cancel)
					return true;
			}

			if (fillup.Volume > VOLUME_TRESHOLD)
			{
				cancel = !_messageBox.Confirm("The volume seems to be too big (" + fillup.Volume.FormatForDisplay(2) + "L), are you sure to save this fillup? ", "Please confirm");
				if (cancel)
					return true;
			}
			if (consumption> CONSUMPTION_TRESHOLD)
			{
				cancel = !_messageBox.Confirm("The comsumption seems to be too big (" + consumption.FormatForDisplay(2) + "L/100km), are you sure to save this fillup? ", "Please confirm");
				if (cancel)
					return true;
			}
			return false;
		}

		public void Cancel()
		{
			_log.Info("Cancel editing fillup and go back");
			_navigationService.GoBack();
		}

		private static void ValidateFillup(Fillup f)
		{
			if (f.OdometerStart >= f.OdometerEnd)
				throw new ValidationException("enter current odometer value - it should be greater than previous value");
		}

		public void AddFuelType()
		{
			_navigationService.UriFor<AddFuelTypeViewModel>().Navigate();
		}

		public void Handle(FuelTypeAddedEvent message)
		{
			if (message.FuelType == null)
				return;

			var fuelTypes = _historyItems.Select(i => i.Fillup.FuelType).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
			fuelTypes.Add(message.FuelType);
			FuelTypes = fuelTypes;
			FuelType = message.FuelType;
		}

		#endregion
	}
}
