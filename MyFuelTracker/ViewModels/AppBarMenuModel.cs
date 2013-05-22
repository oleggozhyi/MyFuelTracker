using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MyFuelTracker.Infrastructure;

namespace MyFuelTracker.ViewModels
{
    public class AppBarMenuModel
    {
        #region Fields

        private readonly IMessageBox _messageBox;
        private readonly INavigationService _navigationService;

        private readonly DynamicAppBarItem _backupToSkydriveMenuItem = new DynamicAppBarItem { Text = "backup to skydrive" };
        private readonly DynamicAppBarItem _restoreFromSkydriveMenuItem = new DynamicAppBarItem { Text = "restore to skydrive" };

        #endregion

        #region ctor

        public AppBarMenuModel(IMessageBox messageBox, INavigationService navigationService)
        {
            _messageBox = messageBox;
            _navigationService = navigationService;
            _backupToSkydriveMenuItem.OnClick = BackupToSkyDrive;
            _restoreFromSkydriveMenuItem.OnClick = RestoreFromSkyDrive;
            MenuItems = new[] { _backupToSkydriveMenuItem, _restoreFromSkydriveMenuItem };
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
            _messageBox.Info("restore");
        }
    }
}
