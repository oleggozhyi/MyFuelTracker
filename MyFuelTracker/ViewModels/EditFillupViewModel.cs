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
	public class EditFillupViewModel : Screen, IHandle<PetrolAddedEvent>
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
		private string _petrol;
		private List<string> _petrols;
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
			Petrols = new List<string>();
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

		public string Petrol
		{
			get { return _petrol; }
			set
			{
				if (value == _petrol) return;
				_petrol = value;
				NotifyOfPropertyChange(() => Petrol);
			}
		}

		public List<string> Petrols
		{
			get { return _petrols; }
			set
			{
				if (value == _petrols) return;
				_petrols = value;
				NotifyOfPropertyChange(() => Petrols);
			}
		}

		#endregion

		#region methods

		protected async override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			_fillup = await _fillupService.CreateNewFillupAsync();
			_historyItems = await _fillupService.GetHistoryAsync();
			Date = _fillup.Date;
			IsPartial = _fillup.IsPartial;
			OdometerEnd = _fillup.OdometerEnd.FormatForDisplay(0);
			OdometerStart = _fillup.OdometerStart.FormatForDisplay(0);

			var petrols = _historyItems.Select(i => i.Fillup.Petrol).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

			Petrols = petrols;
			Price = _fillup.Price.FormatForDisplay(2);
			Petrol = _fillup.Petrol;
		}

		public async void SaveFillup()
		{
			try
			{
				_fillup.Date = Date;
				_fillup.Petrol = Petrol;
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
				throw new ValidationException("enter current odometer value - it should be greater than previous value");
		}

		public void AddPetrol()
		{
			_navigationService.UriFor<AddPetrolViewModel>().Navigate();
		}

		public void Handle(PetrolAddedEvent message)
		{
			if (message.PetrolName == null)
				return;

			var petrols = _historyItems.Select(i => i.Fillup.Petrol).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
			petrols.Add(message.PetrolName);
			Petrols = petrols;
			Petrol = message.PetrolName;
		}

		#endregion

	}
}
