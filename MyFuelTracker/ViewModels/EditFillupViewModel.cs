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
using MyFuelTracker.Infrastructure.Events;
using MyFuelTracker.Infrastructure.Helpers;
using MyFuelTracker.Infrastructure.UiServices;
using MyFuelTracker.Resources;

namespace MyFuelTracker.ViewModels
{
    public class EditFillupViewModel : Screen, IHandle<FuelTypeAddedEvent>, IAppBarItemsProvider
    {
        #region Fields
		private readonly DynamicAppBarButton _goBackButton = new DynamicAppBarButton { IconUri = Icons.Back, Text = AppResources.AppBar_Go_Back };
		private readonly DynamicAppBarButton _saveFillupButton = new DynamicAppBarButton { IconUri = Icons.Save, Text = AppResources.AppBar_Save_Fillup };
        private readonly DynamicAppBarButton[] _appBarButtons;


        private const double DISTANCE_TRESHOLD = 2000.0;
        private const double VOLUME_TRESHOLD = 1000.0;

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
	    private readonly Units _currentUnits;

	    #endregion

        #region ctor

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public EditFillupViewModel(INavigationService navigationService,
                                    ILog log,
                                    IMessageBox messageBox,
                                    IFillupService fillupService,
                                    IEventAggregator eventAggregator,
									IUserSetttingsManager userSetttingsManager)
        {
            _navigationService = navigationService;
            _log = log;
            _messageBox = messageBox;
            _fillupService = fillupService;
            _eventAggregator = eventAggregator;
			_currentUnits = userSetttingsManager.GetCurrentUnits();

	        _eventAggregator.Subscribe(this);
            FuelTypes = new List<string>();

            _saveFillupButton.OnClick = SaveFillup;
            _goBackButton.OnClick = Cancel;
            _appBarButtons = new[] { _saveFillupButton, _goBackButton };
        }

        #endregion

        #region Properties

		public IEnumerable<DynamicAppBarItem> MenuItems { get { return Enumerable.Empty<DynamicAppBarItem>(); } }

        public IEnumerable<DynamicAppBarButton> Buttons { get { return _appBarButtons; } }

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
	            DisplayName = AppResources.EditFillup_Edit_Title;
            }
            else
            {
                _fillup = await _fillupService.CreateNewFillupAsync();
				DisplayName = AppResources.EditFillup_Add_Title;
            }

            _historyItems = await _fillupService.GetHistoryAsync();
            Date = _fillup.Date;
            IsPartial = _fillup.IsPartial;
	        OdometerEnd = _fillup.OdometerEnd > 0 ? _fillup.OdometerEnd.FormatForDisplay(0) : String.Empty;
			OdometerStart = _fillup.OdometerStart > 0 ? _fillup.OdometerStart.FormatForDisplay(0) : String.Empty;
	        Volume = _fillup.Volume > 0 ? _fillup.Volume.FormatForDisplay(2) : String.Empty;
			Price = _fillup.Price > 0 ? _fillup.Price.FormatForDisplay(2) : String.Empty;
	        
            var fuelTypes = _historyItems.Select(i => i.Fillup.FuelType).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            FuelTypes = fuelTypes;
            FuelType = _fillup.FuelType;
        }

        public async void SaveFillup()
        {
            try
            {
                _fillup.Date = Date;
                _fillup.FuelType = FuelType;
                _fillup.IsPartial = IsPartial;
				_fillup.OdometerStart = OdometerStart.GetPositiveDoubleFor(AppResources.EditFillup_Previous_Odometer);
				_fillup.Price = Price.GetPositiveDoubleFor(AppResources.EditFillup_Fuel_Price);
                _fillup.OdometerEnd = OdometerEnd.GetPositiveDoubleFor(AppResources.EditFillup_Current_Odometer);
				_fillup.Volume = Volume.GetPositiveDoubleFor(AppResources.EditFillup_Volume);
                ValidateFillup(_fillup);
                bool userCancelled = CheckForExtremalValues(_fillup);
                if (userCancelled)
                    return;
            }
            catch (ValidationException ex)
            {
                _messageBox.Error(ex.Message, AppResources.EditFillup_Message_Cant_Save);
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
            if (distance > DISTANCE_TRESHOLD)
            {
                cancel = !_messageBox.Confirm(
											String.Format(AppResources.EditFillup_Message_Big_Distance,
															distance.FormatForDisplay(0), _currentUnits.Distance), 
											AppResources.Confirm);
                if (cancel)
                    return true;
            }

            if (fillup.Volume > VOLUME_TRESHOLD)
            {
                cancel = !_messageBox.Confirm(
									String.Format(AppResources.EditFillup_Message_Big_Volume,
										fillup.Volume.FormatForDisplay(0),_currentUnits.Volume), 
									AppResources.Confirm);
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
                throw new ValidationException(AppResources.EditFillup_Message_Enter_Odometer);
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
