using Flurl.Http;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{

    public sealed partial class eclass_Nat : Page
    {
        private App obj = App.Current as App;
        private string eclassOutput = "_";
        public eclass_Nat()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.Loaded += Eclass_Nat_Loaded;
            CoreWindow.GetForCurrentThread().KeyDown+= PasswordKeyDown;
        }

        private void Eclass_Nat_Loaded(object sender, RoutedEventArgs e)
        {
            if (obj.eclassToken != null)
            {
                login.Text = obj.eclassUsername;
                pass.Password = obj.eclassPass;
                ((Frame)Window.Current.Content).Navigate(typeof(AnouncementsEclass));

            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            LoginBtn.IsEnabled = false; back.IsEnabled = false;
            if (this.Frame.CanGoBack) this.Frame.GoBack();
            LoginBtn.IsEnabled = true;  back.IsEnabled = true;
        }
        private void showPopupBtn_Click()
        {
            BlinkPopup.Begin();
            PopupTextBlock.Visibility = Visibility.Visible;
        }
        private async void Login()
        {
            LoginBtn.IsEnabled = false; back.IsEnabled = false;
            eclassOutput = await "https://eclass.aueb.gr/modules/mobile/mlogin.php"
                .PostUrlEncodedAsync(new { uname = login.Text, pass = pass.Password })
                .ReceiveString();
            if (eclassOutput != ("FAILED") && eclassOutput != ("_"))
            {
                obj.eclassToken = eclassOutput; obj.eclassUsername = login.Text; obj.eclassPass = pass.Password;
                ((Frame)Window.Current.Content).Navigate(typeof(AnouncementsEclass));
            }
            else
            {
                showPopupBtn_Click();
            }
            LoginBtn.IsEnabled = true; back.IsEnabled = true;
        }
        private void PasswordKeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Enter)
                Login();
        }
    }
    //Object for passing 2 parameters
    public class Parames
    {
        public string uname { get; set; }
        public string token { get; set; }
        public Parames() { }
    }
}
