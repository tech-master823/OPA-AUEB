using Flurl.Http;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RavinduL.LocalNotifications;
using AuebUnofficial.Core.Services;
using AuebUnofficial.Core.Interfaces;

namespace AuebUnofficial.Viewers
{

    public sealed partial class eclass_Nat : Page
    {
        private App obj;
        private string eclassOutput = "_";
        private Notifications.LNotifications localNotifications =
            new Notifications.LNotifications("Logged in successfully!", "Wrong Username or Password!");
        private LocalNotificationManager manager;
        private bool isGridCheckFocused=false;
        private IEclassService _eclassService;
        public eclass_Nat()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            obj = App.Current as App;
            this.Loaded += Eclass_Nat_Loaded;
            CoreWindow.GetForCurrentThread().KeyDown+= PasswordKeyDown;
            _eclassService = new EclassService();
        }

        private void Eclass_Nat_Loaded(object sender, RoutedEventArgs e)
        {
            if (obj.CurrentEclassUser != null)
            {
                if (obj.CurrentEclassUser.IsRememberEnabled || obj.eclassToken != null)
                {
                    ForeverCheckbox.IsChecked = true;
                    login.Text = obj.CurrentEclassUser.Username;
                    pass.Password = obj.CurrentEclassUser.Password;
                }
            }
            else
            {
                obj.CurrentEclassUser = new Core.Model.EclassUser();
            }
            manager = new LocalNotificationManager(popup);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (obj.eclassToken != null)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(AnouncementsEclass));
            }
            else
            {
                Login();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = false; back.IsEnabled = false;
            if (this.Frame.CanGoBack) this.Frame.GoBack();
            LoginBtn.IsEnabled = true;  back.IsEnabled = true;
        }
        private async void Login()
        {
            LoginBtn.IsEnabled = false; back.IsEnabled = false;
            eclassOutput = await _eclassService.LoginAsync(login.Text, pass.Password);
            if (eclassOutput != ("FAILED") && eclassOutput != ("_"))
            {
                obj.eclassToken = eclassOutput; obj.CurrentEclassUser.Username = login.Text; obj.CurrentEclassUser.Password = pass.Password;
                manager.Show(localNotifications.GetPositiveNotification(), LocalNotificationCollisionBehaviour.Replace);
                await Task.Delay(1500);
                ((Frame)Window.Current.Content).Navigate(typeof(AnouncementsEclass));
            }
            else
            {
                manager.Show(localNotifications.GetNegativeNotification(), LocalNotificationCollisionBehaviour.Replace);
            }
            LoginBtn.IsEnabled = true; back.IsEnabled = true;
        }
        private void PasswordKeyDown(CoreWindow sender, KeyEventArgs e)
        {
            
            if (e.VirtualKey == Windows.System.VirtualKey.Enter)
            {
                if (LoginBtn.IsFocusEngaged || pass.IsFocusEngaged)
                {
                    Login();
                }
            }
                
        }

        private void ForeverCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            obj.CurrentEclassUser.IsRememberEnabled = true;
        }

        private void ForeverCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            obj.CurrentEclassUser.IsRememberEnabled = false;
        }

        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            obj.eclassToken = null;
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
