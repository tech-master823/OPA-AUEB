using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using EclassApi.Extensions;
using RavinduL.LocalNotifications;
using System.Reactive.Linq;
using Akavache;
using Newtonsoft.Json;

namespace AuebUnofficial.Viewers.Eclass
{
    public sealed partial class EclassNat : Page
    {
        private readonly App _app;
        bool signedIn;

        private readonly Notifications.LNotifications localNotifications =
            new Notifications.LNotifications("Logged in successfully!", "Wrong Username or Password!");

        private LocalNotificationManager _manager;
        private bool isGridCheckFocused = false;
        private readonly EclassApi.EclassUser _eclassSession;

        public EclassNat()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            _app = App.Current as App;
            this.Loaded += Eclass_Nat_LoadedAsync;

            _eclassSession = new EclassApi.EclassUser("aueb");
        }

        private void Eclass_Nat_LoadedAsync(object sender, RoutedEventArgs e)
        {
            if (_app.CurrentEclassUser != null)
            {
                if (_app.CurrentEclassUser.IsRememberEnabled || _app.eclassToken != null)
                {
                    ForeverCheckbox.IsChecked = true;
                    login.Text = _app.CurrentEclassUser.Username;
                    pass.Password = _app.CurrentEclassUser.Password;
                }
            }
            else
            {
                _app.CurrentEclassUser = new Core.Model.EclassUser();
            }

            _manager = new LocalNotificationManager(popup);
            CoreWindow.GetForCurrentThread().KeyDown += PasswordKeyDown;
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (_app.eclassToken != null)
            {
                ((Frame) Window.Current.Content).Navigate(typeof(AnnouncementsEclass));
            }
            else
            {
                await LoginAsync();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = false;
            back.IsEnabled = false;
            if (this.Frame.CanGoBack) this.Frame.GoBack();
            LoginBtn.IsEnabled = true;
            back.IsEnabled = true;
        }

        private async Task LoginAsync()
        {
            LoginBtn.IsEnabled = false;
            back.IsEnabled = false;
            signedIn = await _eclassSession.StartAsync(login.Text, pass.Password);
            if (signedIn)
            {
                _manager.Show(localNotifications.GetPositiveNotification(), LocalNotificationCollisionBehaviour.Replace);
                _app.eclassToken = _eclassSession.SessionToken;
                _app.CurrentEclassUser.Username = login.Text;
                _app.CurrentEclassUser.Password = pass.Password;
                _eclassSession.AddCourses();
                await _eclassSession.UserCourses.AddToolsAsync();
                await _eclassSession.AddAnnouncementsAsync();
                var jsonSession = JsonConvert.SerializeObject(_eclassSession, Formatting.Indented,
                    new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.All});
                await BlobCache.InMemory.InvalidateAll();
                await BlobCache.InMemory.Vacuum();
                await BlobCache.InMemory.InsertObject("eclassData", jsonSession, TimeSpan.FromHours(1));
                ((Frame) Window.Current.Content).Navigate(typeof(AnnouncementsEclass));
            }
            else
            {
                _manager.Show(localNotifications.GetNegativeNotification(),
                    LocalNotificationCollisionBehaviour.Replace);
            }

            LoginBtn.IsEnabled = true;
            back.IsEnabled = true;
        }

        private void PasswordKeyDown(CoreWindow sender, KeyEventArgs e)
        {

            if (e.VirtualKey == Windows.System.VirtualKey.Enter)
            {
                if (LoginBtn.IsFocusEngaged || pass.IsFocusEngaged)
                {
                    LoginAsync().GetAwaiter().GetResult();
                }
            }

        }

        private void ForeverCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            _app.CurrentEclassUser.IsRememberEnabled = true;
        }

        private void ForeverCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            _app.CurrentEclassUser.IsRememberEnabled = false;
        }

        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            _app.eclassToken = null;
        }

        private void ForeverCheckbox_FocusEngaged(Control sender, FocusEngagedEventArgs args)
        {
            isGridCheckFocused = true;
        }

        private void ForeverCheckbox_FocusDisengaged(Control sender, FocusDisengagedEventArgs args)
        {
            isGridCheckFocused = false;
        }
    }

}