using System;
using System.Collections;
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
    public class HistoryViewModel : Screen, IHandle<FillupHistoryChangedEvent>, IDynamycButtonsProvider
    {
        #region fields

        private readonly DynamicAppBarButton _addFillupButton;
        private readonly DynamicAppBarButton _viewMoreButton = new DynamicAppBarButton { IconUri = Icons.ViewMore, Text = "view more" };
        private readonly DynamicAppBarButton _viewLessButton = new DynamicAppBarButton { IconUri = Icons.ViewLess, Text = "view less" };


        private readonly IFillupService _fillupService;
        private readonly IStatisticsService _statisticsService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageBox _messageBox;
        private readonly INavigationService _navigationService;
        private IEnumerable _items;
        private FillupHistoryItemViewModel[] _fullHistory;
        private bool _showAllFillups;

        #endregion

        #region ctors

        public HistoryViewModel() { /* for design time support */ }

        public HistoryViewModel(IFillupService fillupService,
                                IStatisticsService statisticsService,
                                IEventAggregator eventAggregator,
                                IMessageBox messageBox,
                                INavigationService navigationService,
                                SummaryViewModel summaryViewModel)
        {
            Debug.WriteLine("HistoryViewModel created");
            DisplayName = "history";
            _fillupService = fillupService;
            _statisticsService = statisticsService;
            _eventAggregator = eventAggregator;
            _messageBox = messageBox;
            _navigationService = navigationService;
            eventAggregator.Subscribe(this);

            _addFillupButton = summaryViewModel.AddFillupButton;
            _viewMoreButton.OnClick = () => ToggleView(true);
            _viewLessButton.OnClick = () => ToggleView(false); ;
        }

        #endregion

        #region properties

        public IEnumerable<DynamicAppBarButton> Buttons
        {
            get
            {
                if (ShowAllFillups)
                    return new[] { _addFillupButton, _viewLessButton };

                return new[] { _addFillupButton, _viewMoreButton };
            }
        }

        public event EventHandler ButtonsChanged;


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

        protected virtual void OnButtonsChanged()
        {
            EventHandler handler = ButtonsChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void DisplayFillupDetails(FillupHistoryItemViewModel viewModel)
        {
            _navigationService.UriFor<DisplayFillupViewModel>()
                              .WithParam(e => e.FillupId, viewModel.HistoryItem.Fillup.Id.ToString())
                              .Navigate();
        }

        public async void EditFillup(FillupHistoryItemViewModel viewModel)
        {
            //await Task.Delay(500);
            _navigationService.UriFor<EditFillupViewModel>()
                              .WithParam(e => e.FillupId, viewModel.HistoryItem.Fillup.Id.ToString())
                              .Navigate();
        }

        public async void DeleteFillup(FillupHistoryItemViewModel viewModel)
        {
            bool proceedWithDeletion = _messageBox.Confirm("are you sure to delete fillup on " + viewModel.Date + "?");
            if (!proceedWithDeletion)
                return;
            await _fillupService.DeleteFillupAsync(viewModel.HistoryItem.Fillup);
            _eventAggregator.Publish(new FillupHistoryChangedEvent());
        }

        public void ToggleView(bool more)
        {
            ShowAllFillups = more;
            SetItemsSource();
            OnButtonsChanged();
        }


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
            _fullHistory = historyItems.Select(i => new FillupHistoryItemViewModel(i, statistics)).ToArray();

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

        #endregion
    }
}
