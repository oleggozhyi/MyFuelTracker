using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.Live;
using Microsoft.Live.Controls;
using MyFuelTracker.Core;
using MyFuelTracker.Infrastructure;
using Newtonsoft.Json;

namespace MyFuelTracker.ViewModels
{
    public class BackupToSkyDriveViewModel : Screen
    {
        #region

        private readonly IMessageBox _messageBox;
        private readonly IFillupService _fillupService;
        private readonly IProgressIndicatorService _progressIndicatorService;
        private bool _isSignedIn;
        private bool _exportToExcel;
        private LiveConnectSession _session;
        private bool _canBackup;

        #endregion

        #region ctor

        public BackupToSkyDriveViewModel(IMessageBox messageBox, IFillupService fillupService, IProgressIndicatorService progressIndicatorService)
        {
            _messageBox = messageBox;
            _fillupService = fillupService;
            _progressIndicatorService = progressIndicatorService;
            _progressIndicatorService.AttachIndicatorToView();
            _progressIndicatorService.ShowIndeterminate("signing in...");
        }

        #endregion

        #region Properties

        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set
            {
                if (value.Equals(_isSignedIn)) return;
                _isSignedIn = value;
                NotifyOfPropertyChange(() => IsSignedIn);
                CanBackup = IsSignedIn;
            }
        }

        public bool ExportToExcel
        {
            get { return _exportToExcel; }
            set
            {
                if (value.Equals(_exportToExcel)) return;
                _exportToExcel = value;
                NotifyOfPropertyChange(() => ExportToExcel);
            }
        }

        public bool CanBackup
        {
            get { return _canBackup; }
            set
            {
                if (value.Equals(_canBackup)) return;
                _canBackup = value;
                NotifyOfPropertyChange(() => CanBackup);
            }
        }

        #endregion

        #region Methods

        public void OnSessionChanged(LiveConnectSessionChangedEventArgs args)
        {
            _progressIndicatorService.Stop();
            if (args.Status == LiveConnectSessionStatus.Connected)
            {
                IsSignedIn = true;
                _session = args.Session;
                _progressIndicatorService.Stop();
            }
        }

        public async void DoBackup()
        {
            try
            {
                CanBackup = false;
                _progressIndicatorService.ShowIndeterminate("backing up to SkyDrive...");
                var fillupHistoryItems = await _fillupService.GetHistoryAsync();
                var fillups = fillupHistoryItems.Select(h => h.Fillup).ToArray();
                var fillupsJson = JsonConvert.SerializeObject(fillups, Formatting.Indented);
                var skyDriveManager = new SkyDriveManager(_session);
                string fileName = "fillups_" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + ".txt";


                await skyDriveManager.SaveTextFileAsync("MyFuelTracker", fileName, fillupsJson);
                _messageBox.Info("data was successfully backed up to SkyDrive");
            }
            catch (Exception ex)
            {
                _messageBox.Error(ex.Message);
            }
            finally
            {
                CanBackup = true;
                _progressIndicatorService.Stop();
            }
        }

        #endregion
    }
}
