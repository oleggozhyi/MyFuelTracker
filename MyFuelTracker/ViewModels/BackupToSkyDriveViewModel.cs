using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.Live;
using Microsoft.Live.Controls;
using MyFuelTracker.Core;
using MyFuelTracker.Core.Models;
using MyFuelTracker.Infrastructure;
using MyFuelTracker.Infrastructure.Helpers;
using MyFuelTracker.Infrastructure.UiServices;
using Newtonsoft.Json;

namespace MyFuelTracker.ViewModels
{
    public class BackupToSkyDriveViewModel : Screen
    {
        #region

        private readonly IMessageBox _messageBox;
        private readonly IFillupService _fillupService;
	    private readonly IFillupsSerializer _fillupsSerializer;
	    private readonly IProgressIndicatorService _progressIndicatorService;
        private bool _isSignedIn;
        private bool _exportToExcel;
        private LiveConnectSession _session;
        private bool _canBackup;
        private int _hideStoryboardFrom;
	    private string _userName;

	    #endregion

        #region ctor

        public BackupToSkyDriveViewModel(IMessageBox messageBox, 
										IFillupService fillupService, 
										IFillupsSerializer fillupsSerializer,
										IProgressIndicatorService progressIndicatorService)
        {
            _messageBox = messageBox;
            _fillupService = fillupService;
	        _fillupsSerializer = fillupsSerializer;
	        _progressIndicatorService = progressIndicatorService;
            _progressIndicatorService.AttachIndicatorToView();
            _progressIndicatorService.ShowIndeterminate("signing in...");
            HideStoryboardFrom = 2000;
        }

        #endregion

        #region Properties

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
				
            }
            else 
            {
                IsSignedIn = false;
                _session = null;
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
	            var fillupsJson = _fillupsSerializer.Serialize(fillups);
                var skyDriveManager = new SkyDriveManager(_session);
				string fileName = "fillups_" + DateTime.Now.ToString(SkyDriveManager.BACKUP_DATE_FORMAT) + ".txt";

				await skyDriveManager.SaveTextFileAsync("MyFuelTracker", fileName, fillupsJson);
				await skyDriveManager.SaveTextFileAsync("MyFuelTracker", SkyDriveManager.LATEST_BACKUP_FILE, fillupsJson, true);
                if (ExportToExcel)
                {
                    var csv = CreateTabulatedText(fillupHistoryItems);
					fileName = "Export_" + DateTime.Now.ToString(SkyDriveManager.BACKUP_DATE_FORMAT) + ".txt";
                    await skyDriveManager.SaveTextFileAsync("MyFuelTracker", fileName, csv);
                }
                _messageBox.Info("data was successfully backed up to SkyDrive", "BACKUP");
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

        private string CreateTabulatedText(IEnumerable<FillupHistoryItem> fillupHistoryItems)
        {
            var builder = new StringBuilder();
            var header = string.Join("\t", new[]
                {
                    "Date", "Previous odometer", "Current odometer", "Distance", "Volume", "Fuel type", "Fuel price", "Fillup cost", "Is partial", "fuel economy"
                });
            builder.AppendLine(header);
            foreach (var item in fillupHistoryItems )
            {
                var line = string.Join("\t", new[]
                {
                   item.Fillup.Date.ToString("dd/MM/yyyy"),
                   item.Fillup.OdometerStart.FormatForDisplay(0),
                   item.Fillup.OdometerEnd.FormatForDisplay(0),
                   (item.Fillup.OdometerEnd - item.Fillup.OdometerStart).FormatForDisplay(0),
                   item.Fillup.Volume.FormatForDisplay(2),
                   item.Fillup.FuelType,
                   item.Fillup.Price.FormatForDisplay(2),
                   (item.Fillup.Volume*item.Fillup.Price).FormatForDisplay(2),
                   item.Fillup.IsPartial ? "yes": "no",
                   item.FuelEconomy.HasValue? item.FuelEconomy.Value.FormatForDisplay(2) : ""
                });
                builder.AppendLine(line);
            }
            return builder.ToString();
        }

        #endregion
    }
}
