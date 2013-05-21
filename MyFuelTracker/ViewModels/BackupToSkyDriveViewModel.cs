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
            var liveConnectClient = new LiveConnectClient(_session);
            var result = await liveConnectClient.GetAsync("me/skydrive/files");
            dynamic res = result.Result;
            string folderId = null;
            foreach (dynamic folder in res.data)
            {
                if (folder.name == "MyFuelTracker")
                {
                    folderId = folder.id;
                    break;
                }
            }
            if (folderId == null)
            {
                _messageBox.Show("Folder MyFuelTracker not found");
                return;
            }
            var fillupHistoryItems = await _fillupService.GetHistoryAsync();
            var fillups = fillupHistoryItems.Select(h => h.Fillup).ToArray();
            var serializeObject = JsonConvert.SerializeObject(fillups);

            using (var ms = new MemoryStream())
            using (var w = new StreamWriter(ms))
            {
                w.Write(serializeObject);
                w.Flush();
                ms.Position = 0;

                var liveOperationResult = await liveConnectClient.UploadAsync(folderId, "fillups_" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + ".txt", ms, OverwriteOption.Overwrite);
                _messageBox.Show("backup uploaded");
            }




        }

        #endregion

    }
}
