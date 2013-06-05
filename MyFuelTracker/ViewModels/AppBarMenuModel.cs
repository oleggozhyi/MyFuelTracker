using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.UiServices;

namespace MyFuelTracker.ViewModels
{
	public class AppBarMenuModel
	{
		#region Fields

		private readonly IMessageBox _messageBox;
		private readonly INavigationService _navigationService;

		private readonly DynamicAppBarItem _backupToSkydriveMenuItem = new DynamicAppBarItem { Text = "backup to skydrive" };
		private readonly DynamicAppBarItem _restoreFromSkydriveMenuItem = new DynamicAppBarItem { Text = "restore from skydrive" };
		private readonly DynamicAppBarItem _settingsMenuItem = new DynamicAppBarItem { Text = "settings" };

		#endregion

		#region ctor

		public AppBarMenuModel(IMessageBox messageBox, INavigationService navigationService)
		{
			_messageBox = messageBox;
			_navigationService = navigationService;
			_backupToSkydriveMenuItem.OnClick = BackupToSkyDrive;
			_restoreFromSkydriveMenuItem.OnClick = RestoreFromSkyDrive;
			_settingsMenuItem.OnClick = Settings;
			MenuItems = new[] { _backupToSkydriveMenuItem, _restoreFromSkydriveMenuItem, _settingsMenuItem };
		}

		#endregion

		#region Properties

		public DynamicAppBarItem[] MenuItems { get; private set; }

		#endregion

		private void BackupToSkyDrive()
		{
			_navigationService.UriFor<BackupToSkyDriveViewModel>().Navigate();
		}

		private void RestoreFromSkyDrive()
		{
			_navigationService.UriFor<RestoreFromSkyDriveViewModel>().Navigate();
		}

		private void Settings()
		{
			_navigationService.UriFor<SettingsViewModel>().Navigate();
		}
	}
}
