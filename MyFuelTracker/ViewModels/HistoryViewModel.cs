using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Events;
using MyFuelTracker.Infrastructure.Helpers;
using MyFuelTracker.Infrastructure.UiServices;
using MyFuelTracker.Resources;
using MyFuelTracker.Views;

namespace MyFuelTracker.ViewModels
{
	public class HistoryViewModel : Screen, IHandle<FillupHistoryChangedEvent>, IDynamicAppBarItemsProvider
	{
		#region fields

		private readonly DynamicAppBarButton _addFillupButton;
		private readonly DynamicAppBarButton _viewMoreButton = new DynamicAppBarButton { IconUri = Icons.ViewMore, Text = AppResources.AppBar_View_More };
		private readonly DynamicAppBarButton _viewLessButton = new DynamicAppBarButton { IconUri = Icons.ViewLess, Text = AppResources.AppBar_View_Less };
		private readonly DynamicAppBarButton _selectButton = new DynamicAppBarButton { IconUri = Icons.ListCheck, Text = AppResources.AppBar_Select };
		private readonly DynamicAppBarButton _deleteSelectedButton = new DynamicAppBarButton { IconUri = Icons.Delete, Text = AppResources.AppBar_Delete_Selected };


		private readonly IFillupService _fillupService;
		private readonly IEventAggregator _eventAggregator;
		private readonly IMessageBox _messageBox;
		private readonly INavigationService _navigationService;
		private readonly AppBarMenuModel _appBarMenuModel;
		private readonly IUserSetttingsManager _userSetttingsManager;
		private IEnumerable _items;
		private FillupHistoryItemViewModel[] _fullHistory;
		private bool _showAllFillups;
		private bool _historyEmpty = true;
		private bool _isSelectionModeEnabled;
		private ISelection _selection;
		private bool _enforceIsSelectionModeEnabled;

		#endregion

		#region ctors

		public HistoryViewModel() { /* for design time support */ }

		public HistoryViewModel(IFillupService fillupService,
								IEventAggregator eventAggregator,
								IMessageBox messageBox,
								INavigationService navigationService,
								AppBarMenuModel appBarMenuModel,
								StatisticsViewModel statisticsViewModel,
								IUserSetttingsManager userSetttingsManager)
		{
			Debug.WriteLine("HistoryViewModel created");
			DisplayName = AppResources.History_Title;
			_fillupService = fillupService;
			_eventAggregator = eventAggregator;
			_messageBox = messageBox;
			_navigationService = navigationService;
			_appBarMenuModel = appBarMenuModel;
			_userSetttingsManager = userSetttingsManager;
			eventAggregator.Subscribe(this);
			_addFillupButton = statisticsViewModel.AddFillupButton;
			_viewMoreButton.OnClick = () => ToggleView(true);
			_viewLessButton.OnClick = () => ToggleView(false);
			_selectButton.OnClick = ChangeSelectionMode;
			_deleteSelectedButton.OnClick = DeleteSelected;
		}

		#endregion

		#region properties

		public bool EnforceIsSelectionModeEnabled
		{
			get { return _enforceIsSelectionModeEnabled; }
			set
			{
				if (value.Equals(_enforceIsSelectionModeEnabled)) return;
				_enforceIsSelectionModeEnabled = value;
				NotifyOfPropertyChange(() => EnforceIsSelectionModeEnabled);
			}
		}

		public bool IsSelectionModeEnabled
		{
			get { return _isSelectionModeEnabled; }
			set
			{
				if (value.Equals(_isSelectionModeEnabled)) return;
				_isSelectionModeEnabled = value;
				NotifyOfPropertyChange(() => IsSelectionModeEnabled);
				RaiseAppBarChangedChanged();
			}
		}

		public bool HistoryEmpty
		{
			get { return _historyEmpty; }
			set
			{
				if (value.Equals(_historyEmpty)) return;
				_historyEmpty = value;
				NotifyOfPropertyChange(() => HistoryEmpty);
			}
		}

		public IEnumerable<DynamicAppBarButton> Buttons
		{
			get
			{
				var buttons = new List<DynamicAppBarButton>
					{
						_addFillupButton,
						ShowAllFillups ? _viewLessButton : _viewMoreButton,
						IsSelectionModeEnabled ? _deleteSelectedButton : _selectButton
					};

				return buttons;
			}
		}

		public IEnumerable<DynamicAppBarItem> MenuItems
		{
			get { return _appBarMenuModel.MenuItems; }
		}

		public event EventHandler AppBarChanged;


		public bool ShowAllFillups
		{
			get { return _showAllFillups; }
			set
			{
				if (value.Equals(_showAllFillups)) return;
				_showAllFillups = value;
				NotifyOfPropertyChange(() => ShowAllFillups);
			}
		}

		public IEnumerable Items
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

		private async void DeleteSelected()
		{
			if(_selection.SelectedItems.Count == 0)
				return;

			bool proceedWithDeletion = _messageBox.Confirm(string.Format(AppResources.History_Confirms_Delete_Selected, _selection.SelectedItems.Count));
			if (!proceedWithDeletion)
				return;

			EnforceIsSelectionModeEnabled = false;
			foreach (FillupHistoryItemViewModel fillupViewModel in _selection.SelectedItems)
			{
				await _fillupService.DeleteFillupAsync(fillupViewModel.HistoryItem.Fillup);	
			}
			_eventAggregator.Publish(new FillupHistoryChangedEvent());
			
			RaiseAppBarChangedChanged();
		}

		private void ChangeSelectionMode()
		{
			EnforceIsSelectionModeEnabled = true;
		}

		public void OnNavigating(CancelEventArgs e)
		{
			if (IsSelectionModeEnabled)
			{
				IsSelectionModeEnabled = false;
				e.Cancel = true;
				RaiseAppBarChangedChanged();
			}
		}

		protected virtual void RaiseAppBarChangedChanged()
		{
			EventHandler handler = AppBarChanged;
			if (handler != null) handler(this, EventArgs.Empty);
		}

		public void DisplayFillupDetails(FillupHistoryItemViewModel viewModel)
		{
			_navigationService.UriFor<DisplayFillupViewModel>()
							  .WithParam(e => e.FillupId, viewModel.HistoryItem.Fillup.Id.ToString())
							  .Navigate();
		}

		public void EditFillup(FillupHistoryItemViewModel viewModel)
		{
			_navigationService.UriFor<EditFillupViewModel>()
							  .WithParam(e => e.FillupId, viewModel.HistoryItem.Fillup.Id.ToString())
							  .Navigate();
		}

		public async void DeleteFillup(FillupHistoryItemViewModel viewModel)	
		{
			bool proceedWithDeletion = _messageBox.Confirm(String.Format(AppResources.History_Confirms_Delete_Fillup, viewModel.Date));
			if (!proceedWithDeletion)
				return;
			await _fillupService.DeleteFillupAsync(viewModel.HistoryItem.Fillup);
			_eventAggregator.Publish(new FillupHistoryChangedEvent());
		}

		public void ToggleView(bool more)
		{
			ShowAllFillups = more;
			SetItemsSource();
			RaiseAppBarChangedChanged();
		}


		public void GoToAddFillup()
		{
			_navigationService.UriFor<EditFillupViewModel>().Navigate();
		}

		public void GoToSettings()
		{
			_messageBox.Info("not implemented");
		}

		public async Task UpdateAsync()
		{
			var historyItems = await _fillupService.GetHistoryAsync();
			if (!historyItems.Any())
			{
				HistoryEmpty = true;
				return;
			}

			HistoryEmpty = false;
			var statistics = await _fillupService.GetStatisticsAsync();
			var currentUnits = _userSetttingsManager.GetCurrentUnits();
			_fullHistory = historyItems.Select(i => new FillupHistoryItemViewModel(i, statistics, currentUnits)).ToArray();
			SetItemsSource();
		}

		public async void Handle(FillupHistoryChangedEvent message)
		{
			await UpdateAsync();
		}

		private void SetItemsSource()
		{
			Items = ShowAllFillups
						? (IEnumerable)FillupsGroupingHelper.GroupFillups(_fullHistory)
						: (IEnumerable)_fullHistory.Take(5).ToArray();
		}

		protected override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			_selection = view as ISelection;
			_selection.SelectionChanged += (s, e) => OnSelectionChanged();
		}

		private void OnSelectionChanged()
		{
			if (_selection.SelectedItems.Count == 0)
			{
				EnforceIsSelectionModeEnabled = false;
			}
		}

		#endregion
	}
}
