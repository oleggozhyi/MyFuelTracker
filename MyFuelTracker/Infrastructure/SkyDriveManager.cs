using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Live;
using MyFuelTracker.Core;

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

	    public async Task<FillupsHolder[]> GetAllFillupsAsync()
	    {
			var skyDriveClient = new LiveConnectClient(_session);
			var serializer = new FillupsSerializer();

			var liveOperationResult = await skyDriveClient.GetAsync("me/skydrive/files");
			var folderContents = (IEnumerable)liveOperationResult.Result["data"];
			string folderId = FindFolder("MyFuelTracker", folderContents);
		    if (folderId == null)
			    return new FillupsHolder[0];

			liveOperationResult = await skyDriveClient.GetAsync(folderId + "/files");
			folderContents = (IEnumerable)liveOperationResult.Result["data"];

			var backupFiles = new List<FillupsHolder>();
			foreach (dynamic folderObject in folderContents)
			{

				if (folderObject.type == "file" && ((string)folderObject.name).StartsWith("fillups"))
				{
					var downloadOperationResult = await skyDriveClient.DownloadAsync((string)folderObject.id + "/content");
					try
					{
						var fileContent = new StreamReader(downloadOperationResult.Stream).ReadToEnd();
						var fillupsHolder = serializer.Deserialize(fileContent);
						backupFiles.Add(fillupsHolder);
					}
					catch (Exception)
					{
					}
					finally
					{
						downloadOperationResult.Stream.Dispose();
					}
				}
			}
			return backupFiles.ToArray();
	    }


	    #endregion
    }
}
