using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Events;

namespace MyFuelTracker.ViewModels
{
	public class MainViewModel : Conductor<IScreen>.Collection.OneActive
	{
		#region fields

		private readonly IEventAggregator _eventAggregator;
		private object _selectedItem;

		#endregion

		#region properties

		public SummaryViewModel SummaryViewModel { get; set; }
		public HistoryViewModel HistoryViewModel { get; set; }
		public object SelectedItem { get; set; }


		#endregion

		#region ctors

		public MainViewModel() { /* for design time support */ }

		public MainViewModel(SummaryViewModel summaryViewModel,
							 HistoryViewModel historyViewModel,
							 IEventAggregator eventAggregator)
		{
			_eventAggregator = eventAggregator;
			Debug.WriteLine("MainViewModel created");

			SummaryViewModel = summaryViewModel;
			HistoryViewModel = historyViewModel;
			_eventAggregator.Subscribe(this);
		}

		#endregion

		#region methods

		protected override void OnInitialize()
		{
			Debug.WriteLine("MainViewModel OnInitialize");
			base.OnInitialize();

			Items.Add(SummaryViewModel);
			Items.Add(HistoryViewModel);

			ActivateItem(SummaryViewModel);
		}

		protected override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			Debug.WriteLine("MainViewModel OnViewLoaded");
			_eventAggregator.Publish(new FillupHistoryChangedEvent());
		}

		public void OnBackKeyPress(CancelEventArgs args)
		{
			if (this.SelectedItem == HistoryViewModel)
			{
				HistoryViewModel.OnNavigating(args);
			}
		}

		#endregion
	}
}
