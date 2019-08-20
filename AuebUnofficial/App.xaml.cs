using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using System.Diagnostics;
using Akavache;
using AuebUnofficial.Core.Model;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Push;

namespace AuebUnofficial
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public EclassUser CurrentEclassUser { get; set; }
        public string eclassToken { get; set; }
        public AppSettings AppSettings { get; set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            Suspending += async (s, a) =>
            {
                //When user navigated at Eclass client page in this session, save the data!
                if (CurrentEclassUser != null)
                {
                    await SaveChanges(); // "I really like my data and want it for later too"
                }
                await BlobCache.Shutdown();
            };
            this.Suspending += OnSuspending;
        }



        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(Navig), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            };
            Registrations.Start("AuebUnofficial.UWP");
            BlobCache.EnsureInitialized();
            await SetAppSettingsAsync();
            if (AppSettings == null) App.Current.Exit();
#if !DEBUG
            AppCenter.Start(AppSettings.AppCenter, typeof(Analytics), typeof(Push));
            Push.CheckLaunchedFromNotification(e);
#endif
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppSettings.SyncfusionLisenceKey);
            CurrentEclassUser = await GetUserAsync();

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //await SaveChanges();
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            
            deferral.Complete();
        }

        private async Task<string> SaveChanges()
        {
            var fileName = "eclass_user";
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.TryGetItemAsync(fileName);
                if (!CurrentEclassUser.IsRememberEnabled)
                {
                    CurrentEclassUser.Username = null;
                    CurrentEclassUser.Password = null;
                }
                //if file does not exist we create a new guid
                var storageFile = await folder.CreateFileAsync(fileName,CreationCollisionOption.ReplaceExisting);
            try
            {
                await FileIO.WriteTextAsync(storageFile, JsonConvert.SerializeObject(CurrentEclassUser));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return CurrentEclassUser.Uid;
        }

        public async Task<EclassUser> GetUserAsync()
        {
            var fileName = "eclass_user";
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.TryGetItemAsync(fileName);
            if (file == null)
            {
                return new EclassUser();
            }
            else
            {
                //else we return the already exising uid
                var storageFile = await folder.GetFileAsync(fileName);
                var readText = await FileIO.ReadTextAsync(storageFile);
                return JsonConvert.DeserializeObject<EclassUser>(readText);
            }
        }
        private async Task SetAppSettingsAsync()
        {
            try
            {
                var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var dataFolder = await assetsFolder.GetFolderAsync("Data");
                var file = await dataFolder.GetFileAsync(@"appsettings.json");
                String appsetingsString = await FileIO.ReadTextAsync(file);
                AppSettings = JsonConvert.DeserializeObject<AppSettings>(appsetingsString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
