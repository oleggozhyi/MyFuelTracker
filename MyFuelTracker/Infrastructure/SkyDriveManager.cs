using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Live;

namespace MyFuelTracker.Infrastructure
{
    public class SkyDriveManager
    {
        #region Fields

        private readonly LiveConnectSession _session;

        #endregion


        #region ctor

        public SkyDriveManager(LiveConnectSession session)
        {
            _session = session;
        }

        #endregion

        #region Methods

        public async Task SaveTextFileAsync(string folder, string fileName, string fileConent)
        {
            var skyDriveClient = new LiveConnectClient(_session);
            var folderId = await PreparePathAndReturnFolderId(skyDriveClient, folder);
            using (var stream = CreateStream(fileConent))
            {
                var liveOperationResult = await skyDriveClient.UploadAsync(folderId, fileName, stream, OverwriteOption.Rename);
            }
        }

        private Stream CreateStream(string fileContent)
        {
            var ms = new MemoryStream();
            var w = new StreamWriter(ms);
            w.Write(fileContent);
            w.Flush();
            ms.Position = 0;
            return ms;
        }

        private async Task<string> PreparePathAndReturnFolderId(LiveConnectClient skyDriveClient, string folder)
        {
            string currentFolderId = "me/skydrive";
            if (String.IsNullOrWhiteSpace(folder))
                return currentFolderId;

            var liveOperationResult = await skyDriveClient.GetAsync(currentFolderId + "/files");
            var folderContents = (IEnumerable)liveOperationResult.Result["data"];
            string folderId = FindFolder(folder, folderContents);
            if (folderId != null)
                return folderId;

            var folderData = new Dictionary<string, object>();
            folderData.Add("name", folder);
            folderData.Add("description", "Folder containing backups and exports for 'My Fuel Tracker' app");

            dynamic operationResult = await skyDriveClient.PostAsync("me/skydrive", folderData);
            return operationResult.id;
        }

        private string FindFolder(string folder, IEnumerable folderContents)
        {
            foreach (dynamic folderObject in folderContents)
            {
                if (folderObject.name == folder)
                {
                    if (folderObject.type == "folder")
                        return folderObject.id;

                    throw new InvalidOperationException("Error!");
                }
            }
            return null;
        }


        /*
         *  var liveConnectClient = new LiveConnectClient(_session);
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


            using (var ms = new MemoryStream())
            using (var w = new StreamWriter(ms))
            {
                w.Write(serializeObject);
                w.Flush();
                ms.Position = 0;

                var liveOperationResult = await liveConnectClient.UploadAsync(folderId, "fillups_" + DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + ".txt", ms, OverwriteOption.Overwrite);
                _messageBox.Show("backup uploaded");
            }



         */
        #endregion


    }
}
