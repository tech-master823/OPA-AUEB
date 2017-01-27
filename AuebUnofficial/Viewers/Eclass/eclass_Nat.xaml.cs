using Flurl.Http;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{

    public sealed partial class eclass_Nat : Page
    {
        private string eclassOutput = "_";
        public eclass_Nat()
        {
            this.InitializeComponent();            
        }

        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            Login.IsEnabled = false; back.IsEnabled = false;
            eclassOutput = await "https://eclass.aueb.gr/modules/mobile/mlogin.php"
                .PostUrlEncodedAsync(new { uname = login.Text, pass = pass.Password })
                .ReceiveString();
            if (eclassOutput != ("FAILED") && eclassOutput!=("_"))
            {
                ((Frame)Window.Current.Content).Navigate(typeof(Viewers.AnouncementsEclass),eclassOutput);
            }else
            {
                showPopupBtn_Click();
            }
            Login.IsEnabled = true; back.IsEnabled = true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Login.IsEnabled = false; back.IsEnabled = false;
            if (this.Frame.CanGoBack) this.Frame.GoBack();
            Login.IsEnabled = true;  back.IsEnabled = true;
        }
        private void showPopupBtn_Click()
        {
            BlinkPopup.Begin();
            PopupTextBlock.Visibility = Visibility.Visible;
        }
    }
}
