using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class AddPetrolViewModel : Screen
	{
		private readonly INavigationService _navigationService;
		private readonly IEventAggregator _eventAggregator;
		private string _petrolName;
		public string PetrolName
		{
			get { return _petrolName; }
			set
			{
				if (value == _petrolName) return;
				_petrolName = value;
				NotifyOfPropertyChange(() => PetrolName);
			}
		}

		public AddPetrolViewModel(INavigationService navigationService,
								  IEventAggregator eventAggregator)
		{
			_navigationService = navigationService;
			_eventAggregator = eventAggregator;
		}

		public void SavePetrol()
		{
			_eventAggregator.Publish(new PetrolAddedEvent { PetrolName = PetrolName });
			_navigationService.GoBack();
		}

		public void Cancel()
		{
			_navigationService.GoBack();
		}
	}
}