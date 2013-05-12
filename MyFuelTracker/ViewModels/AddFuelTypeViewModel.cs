using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class AddFuelTypeViewModel : Screen
	{
		private readonly INavigationService _navigationService;
		private readonly IEventAggregator _eventAggregator;
		private string _fuelType;
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