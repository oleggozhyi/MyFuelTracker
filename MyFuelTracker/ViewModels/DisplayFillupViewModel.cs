﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class DisplayFillupViewModel : Screen, IHandle<FillupHistoryChangedEvent>
	{
		private readonly INavigationService _navigationService;
		private readonly IFillupService _fillupService;
		private readonly IMessageBox _messageBox;
		private readonly IEventAggregator _eventAggregator;
		private readonly IStatisticsService _statisticsService;
		private FillupHistoryItemViewModel _details;
		private FillupHistoryItem _fillupHistoryItem;
		private bool _deleted;

		public string FillupId { get; set; }

		public DisplayFillupViewModel(INavigationService navigationService,
		                              IFillupService fillupService,
									  IMessageBox messageBox,
									  IEventAggregator eventAggregator,
		                              IStatisticsService statisticsService)
		{
			_navigationService = navigationService;
			_fillupService = fillupService;
			_messageBox = messageBox;
			_eventAggregator = eventAggregator;
			_statisticsService = statisticsService;
			_eventAggregator.Subscribe(this);
		}


		public void Cancel()
		{
			_navigationService.GoBack();
		}

		public void EditFillup()
		{
			_navigationService.UriFor<EditFillupViewModel>()
							  .WithParam(e => e.FillupId, FillupId)
							  .Navigate();
		}

		public async void DeleteFillup()
		{
			bool proceedWithDeletion = _messageBox.Confirm("are you sure to delete fillup on " + Details.Date + "?");
			if (!proceedWithDeletion)
				return;
			await _fillupService.DeleteFillupAsync(_fillupHistoryItem.Fillup);
			_deleted = true;
			_eventAggregator.Publish(new FillupHistoryChangedEvent());
			_navigationService.GoBack();
		}


		protected override void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			if (FillupId == null)
				throw new InvalidOperationException("Fillup ID should be passed");

			UpdateAsync();
		}

		public async void UpdateAsync()
		{
			if(_deleted )
				return;
			var historyItems = await _fillupService.GetHistoryAsync();
			var fuelConsumptionStatistics = await _statisticsService.CalculateStatisticsAsync(historyItems);
			_fillupHistoryItem = historyItems.Single(h => h.Fillup.Id.ToString() == FillupId);
			Details = new FillupHistoryItemViewModel(_fillupHistoryItem, fuelConsumptionStatistics);
		}

		public FillupHistoryItemViewModel Details
		{
			get { return _details; }
			set
			{
				if (Equals(value, _details)) return;
				_details = value;
				NotifyOfPropertyChange(() => Details);
			}
		}

		public void Handle(FillupHistoryChangedEvent message)
		{
			UpdateAsync();
		}
	}
}