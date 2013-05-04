using System.Diagnostics;
using System.Windows.Input;
using Caliburn.Micro;
using Caliburn.Micro.BindableAppBar;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class MainViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<FillupItemChangedEvent>
	{
		#region fields

		private readonly IEventAggregator _eventAggregator;
		private object _selectedItem;

		#endregion

		#region properties

		public SummaryViewModel SummaryViewModel { get; set; }
		public HistoryViewModel HistoryViewModel { get; set; }

		public object SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (Equals(value, _selectedItem)) return;
				_selectedItem = value;
				NotifyOfPropertyChange(() => SelectedItem);
			}
		}

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
			base.OnInitialize();

			Items.Add(SummaryViewModel);
			Items.Add(HistoryViewModel);

			ActivateItem(SummaryViewModel);

			AppBarConductor.Mixin(this);

			_eventAggregator.Publish(new FillupHistoryChangedEvent());
		}
	
		public void Handle(FillupItemChangedEvent message)
		{
			SelectedItem = HistoryViewModel;
		}

		#endregion
	}
}
