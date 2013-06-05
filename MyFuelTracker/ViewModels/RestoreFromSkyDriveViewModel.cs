using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.Live;
using Microsoft.Live.Controls;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
	public class RestoreFromSkyDriveViewModel : Screen
	{
		#region Fields

		private IMessageBox _messageBox;
		private IFillupService _fillupService;
		private readonly IEventAggregator _eventAggregator;
		private readonly IProgressIndicatorService _progressIndicatorService;
		private int _hideStoryboardFrom;
		private bool _isSignedIn;
		private LiveConnectSession _session;
		private bool _restoreSourceAvailable;
		private bool _backupDownloaded;
		private BackupViewModel _restoreSource;
		private bool _canRestore;
		private string _userName;

		#endregion

		#region ctor

		public RestoreFromSkyDriveViewModel(IMessageBox messageBox, 
											IFillupService fillupService,
											IEventAggregator eventAggregator,
											IProgressIndicatorService progressIndicatorService)
		{
			_messageBox = messageBox;
			_fillupService = fillupService;
			_eventAggregator = eventAggregator;
			_progressIndicatorService = progressIndicatorService;
			_progressIndicatorService.AttachIndicatorToView();
			_progressIndicatorService.ShowIndeterminate("signing in...");
			HideStoryboardFrom = 2000;
		}

		#endregion

		#region Properties

		public BackupViewModel RestoreSource
		{
			get { return _restoreSource; }
			set
			{
				if (Equals(value, _restoreSource)) return;
				_restoreSource = value;
				NotifyOfPropertyChange(() => RestoreSource);
			}
		}

		public bool RestoreSourceAvailable
		{
			get { return _restoreSourceAvailable; }
			set
			{
				if (value.Equals(_restoreSourceAvailable)) return;
				_restoreSourceAvailable = value;
				NotifyOfPropertyChange(() => RestoreSourceAvailable);
			}
		}

		public int HideStoryboardFrom
		{
			get { return _hideStoryboardFrom; }
			set
			{
				if (value == _hideStoryboardFrom) return;
				_hideStoryboardFrom = value;
				NotifyOfPropertyChange(() => HideStoryboardFrom);
			}
		}

		public bool IsSignedIn
		{
			get { return _isSignedIn; }
			set
			{
				if (value.Equals(_isSignedIn)) return;
				_isSignedIn = value;
				NotifyOfPropertyChange(() => IsSignedIn);
			}
		}

		public bool BackupDownloaded
		{
			get { return _backupDownloaded; }
			set
			{
				if (value.Equals(_backupDownloaded)) return;
				_backupDownloaded = value;
				NotifyOfPropertyChange(() => BackupDownloaded);
			}
		}

		public bool CanRestore
		{
			get { return _canRestore; }
			set
			{
				if (value.Equals(_canRestore)) return;
				_canRestore = value;
				NotifyOfPropertyChange(() => CanRestore);
			}
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				if (value == _userName) return;
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
			}
		}

		#endregion

		#region Methods

		public async void Restore()
		{
			var restore = _messageBox.Confirm("Are you sure you want to restore? \n\n" 
						+ "Tap OK to continue", "Restore");
			if(!restore)
				return;

			try
			{
				_progressIndicatorService.ShowIndeterminate("Restoring...");
				CanRestore = false;
				await _fillupService.RestoreDataAsync(RestoreSource.FillupsHolder.Fillups);
				_messageBox.Info("Your fillups data was successfully restored", "restore");

				_eventAggregator.Publish(new FillupHistoryChangedEvent());
			}
			catch (Exception ex)
			{
				_messageBox.Error(ex.Message);
			}
			finally
			{
				CanRestore = true;
				_progressIndicatorService.Stop();
			}
		}

		public async void OnSessionChanged(LiveConnectSessionChangedEventArgs args)
		{
			_progressIndicatorService.Stop();
			if (args.Status == LiveConnectSessionStatus.Connected)
			{
				_session = args.Session;
				dynamic userInfo = (await new LiveConnectClient(_session).GetAsync("me")).Result;
				UserName = userInfo.name;
				HideStoryboardFrom = 0;
				IsSignedIn = true;
				LoadLastBackup();
			}
			else
			{
				IsSignedIn = false;
				_session = null;
			}
		}

		

		private async void LoadLastBackup()
		{
			try
			{
				_progressIndicatorService.ShowIndeterminate("gettings latest backup info...");
				var skyDriveManager = new SkyDriveManager(_session);
				var fillupsHolder = await skyDriveManager.GetLatestBackupAsync();
				if (fillupsHolder != null)
				{
					RestoreSource = new BackupViewModel {FillupsHolder = fillupsHolder};
					RestoreSourceAvailable = true;
					CanRestore = true;
				}
				BackupDownloaded = true;
			}
			catch (Exception ex)
			{
				_messageBox.Error(ex.Message);
			}
			finally
			{
				_progressIndicatorService.Stop();
			}
		}

		#endregion

	}
}
