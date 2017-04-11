using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace StartUpTask
{
    public class AnalyticsTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private string _guid;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            _guid = await GetUserIdAsync();

            _deferral.Complete();
        }

        public async Task<string> GetUserIdAsync()
        {
            var fileName = "user id";
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.TryGetItemAsync(fileName);
            if (file != null)
            {
                var storageFile = await folder.CreateFileAsync(fileName);
                var newId = Guid.NewGuid().ToString();
                await FileIO.WriteTextAsync(storageFile, newId);
                return newId;
            }
            else
            {
                var storageFile = await folder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(storageFile);
            }
        }
    }
}
