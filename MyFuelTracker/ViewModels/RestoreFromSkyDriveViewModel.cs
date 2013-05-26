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
		private readonly IProgressIndicatorService _progressIndicatorService;
		private int _hideStoryboardFrom;
		private bool _isSignedIn;
		private LiveConnectSession _session;
		private bool _backupsAvailable;
		private bool _fillupsDownloaded;
		private IEnumerable<BackupViewModel> _backups;

		#endregion

		#region ctor

		public RestoreFromSkyDriveViewModel(IMessageBox messageBox, IFillupService fillupService,
											IProgressIndicatorService progressIndicatorService)
		{
			_messageBox = messageBox;
			_fillupService = fillupService;
			_progressIndicatorService = progressIndicatorService;
			_progressIndicatorService.AttachIndicatorToView();
			_progressIndicatorService.ShowIndeterminate("signing in...");
			HideStoryboardFrom = 2000;
		}

		#endregion

		#region Properties

		public IEnumerable<BackupViewModel> Backups
		{
			get { return _backups; }
			set
			{
				if (Equals(value, _backups)) return;
				_backups = value;
				NotifyOfPropertyChange(() => Backups);
			}
		}

		public bool BackupsAvailable
		{
			get { return _backupsAvailable; }
			set
			{
				if (value.Equals(_backupsAvailable)) return;
				_backupsAvailable = value;
				NotifyOfPropertyChange(() => BackupsAvailable);
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

		public bool FillupsDownloaded
		{
			get { return _fillupsDownloaded; }
			set
			{
				if (value.Equals(_fillupsDownloaded)) return;
				_fillupsDownloaded = value;
				NotifyOfPropertyChange(() => FillupsDownloaded);
			}
		}

		#endregion

		#region Methods

		public void OnSessionChanged(LiveConnectSessionChangedEventArgs args)
		{
			_progressIndicatorService.Stop();
			if (args.Status == LiveConnectSessionStatus.Connected)
			{
				HideStoryboardFrom = 0;
				IsSignedIn = true;
				_session = args.Session;
				LoadBackups();
			}
			else
			{
				IsSignedIn = false;
				_session = null;
			}
		}

		private async void LoadBackups()
		{
			try
			{
				_progressIndicatorService.ShowIndeterminate("gettings backups info...");
				var skyDriveManager = new SkyDriveManager(_session);
				FillupsHolder[] fillupsHolders = await skyDriveManager.GetAllFillupsAsync();
				FillupsDownloaded = true;
				if (!fillupsHolders.Any())
				{
					BackupsAvailable = false;
					return;
				}
				BackupsAvailable = true;
				Backups = fillupsHolders.Select(f => new BackupViewModel { FillupsHolder = f }).ToArray();
			}
			finally
			{

				_progressIndicatorService.Stop();
			}

		}

		#endregion

	}

	public class BackupViewModel
	{
		public string BackupDate
		{
			get { return FillupsHolder.Timestamp.ToString("dd MMM yyyy HH:mm:ss"); }
		}

		public string LastFillupDate
		{
			get { return FillupsHolder.Fillups.First().Date.ToString("dd MMM yyyy"); }
		}

		public string LastOdometer
		{
			get { return FillupsHolder.Fillups.First().OdometerEnd.FormatForDisplay(0); }
		}
		public FillupsHolder FillupsHolder { get; set; }
	}
}
