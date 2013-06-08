using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Events;
using MyFuelTracker.Resources;

namespace MyFuelTracker.ViewModels
{
    public class AddFuelTypeViewModel : Screen, IAppBarItemsProvider
    {
        private readonly DynamicAppBarButton _saveFuelTypeButton = new DynamicAppBarButton { IconUri = Icons.Save, Text = AppResources.AppBar_Save_Fuel_Type };
		private readonly DynamicAppBarButton _cancelButton = new DynamicAppBarButton { IconUri = Icons.Back, Text = AppResources.AppBar_Go_Back };
        private readonly DynamicAppBarButton[] _appButtons;

        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private string _fuelType;

        public IEnumerable<DynamicAppBarButton> Buttons { get { return _appButtons; } }
		public IEnumerable<DynamicAppBarItem> MenuItems { get { return Enumerable.Empty<DynamicAppBarItem>(); } }

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

        public AddFuelTypeViewModel(INavigationService navigationService,
                                  IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _appButtons = new[] { _saveFuelTypeButton, _cancelButton };
            _saveFuelTypeButton.OnClick = SaveFuelType;
            _cancelButton.OnClick = Cancel;
        }

        public void SaveFuelType()
        {
            _eventAggregator.Publish(new FuelTypeAddedEvent { FuelType = FuelType });
            _navigationService.GoBack();
        }

        public void Cancel()
        {
            _navigationService.GoBack();
        }
    }
}