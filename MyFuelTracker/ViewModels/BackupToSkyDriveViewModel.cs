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
        private bool _isSignedIn;
        private bool _exportToExcel;
        private LiveConnectSession _session;

        #endregion

        #region ctor

        public BackupToSkyDriveViewModel(IMessageBox messageBox, IFillupService fillupService)
        {
            _messageBox = messageBox;
            _fillupService = fillupService;
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

        #endregion

        #region Methods

        public void OnSessionChanged(LiveConnectSessionChangedEventArgs args)
        {
            if (args.Status == LiveConnectSessionStatus.Connected)
            {
                IsSignedIn = true;
                _session = args.Session;
            }
        }

        public async void DoBackup()
        {
            var fillupHistoryItems = await _fillupService.GetHistoryAsync();
            var fillups = fillupHistoryItems.Select(h => h.Fillup).ToArray();
            var fillupsJson = JsonConvert.SerializeObject(fillups, Formatting.Indented);
            var skyDriveManager = new SkyDriveManager(_session);
            string fileName = "fillups_" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + ".txt";

            try
            {
                await skyDriveManager.SaveTextFileAsync("MyFuelTracker", fileName, fillupsJson);
            }
            catch (Exception ex)
            {
                _messageBox.Show(ex.Message);                
            }
        }

        #endregion
    }
}
