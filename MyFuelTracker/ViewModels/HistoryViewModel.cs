using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class HistoryViewModel : Screen, IHandle<FillupHistoryChangedEvent>
	{
		#region fields

		private readonly IFillupService _fillupService;
		private readonly IStatisticsService _statisticsService;
		private readonly IMessageBox _messageBox;
		private readonly INavigationService _navigationService;
		private IEnumerable<FillupHistoryItemViewModel> _items;

		#endregion

		#region ctors

		public HistoryViewModel() { /* for design time support */ }

		public HistoryViewModel(IFillupService fillupService, 
								IStatisticsService statisticsService, 
								IEventAggregator eventAggregator,
								IMessageBox messageBox,
								INavigationService navigationService)
		{
			Debug.WriteLine("HistoryViewModel created");
			DisplayName = "history";
			_fillupService = fillupService;
			_statisticsService = statisticsService;
			_messageBox = messageBox;
			_navigationService = navigationService;
			eventAggregator.Subscribe(this);
		}

		#endregion

		#region properties

		public IEnumerable<FillupHistoryItemViewModel> Items
		{
			get { return _items; }
			set
			{
				if (Equals(value, _items)) return;
				_items = value;
				NotifyOfPropertyChange(() => Items);
			}
		}

		#endregion

		#region methods

		public void GoToAddFillup()
		{
			_navigationService.UriFor<EditFillupViewModel>().Navigate();
		}

		public void GoToSettings()
		{
			_messageBox.Show("not implemented");
		}

		public async Task UpdateAsync()
		{
			var historyItems = await _fillupService.GetHistoryAsync();
			var statistics = await _statisticsService.CalculateStatisticsAsync(historyItems);
			Items = historyItems.Select(i => new FillupHistoryItemViewModel(i, statistics.AllTimeAvgConsumption)).ToArray();
		}

		public async void Handle(FillupHistoryChangedEvent message)
		{
			await UpdateAsync();
		}

		#endregion
	}
}
