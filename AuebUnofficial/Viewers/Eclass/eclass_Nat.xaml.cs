using Flurl.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AuebUnofficial.Viewers
{

    public sealed partial class eclass_Nat : Page
    {
        public eclass_Nat()
        {
            this.InitializeComponent();
        }



        private async void ButtonClick(object sender, RoutedEventArgs e)
        {
            output.Text = await "https://eclass.aueb.gr/modules/mobile/mlogin.php"
                .PostUrlEncodedAsync(new { uname = login.Text, pass = pass.Password })
                .ReceiveString();
            if (output.Text != ("FAILED"))
            {
                ((Frame)Window.Current.Content).Navigate(typeof(Viewers.AnouncementsEclass),output.Text);
            }else
            {
                output.Text = "wrong inputs!!!";
            }
        }
    }
}
