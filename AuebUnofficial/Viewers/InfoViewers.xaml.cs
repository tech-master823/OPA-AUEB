using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{

    public sealed partial class InfoViewers : Page
    {
        Uri uriCall;
        PivotdItem pi;
        public InfoViewers()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            pi = (PivotdItem)e.Parameter;
            Title.Text = pi.Header;
            p1.Text = pi.P1.ToString(); p2.Text = pi.P2.ToString(); p3.Text = pi.P3.ToString(); mtext.Text = pi.Mail;
            if (pi.P3.Equals(0))
            {
                p3rel.Visibility = Visibility.Collapsed;
            }
        }



        private void moreP_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            if (isVisible<StackPanel>(relP))
            {
                relP.Visibility = Visibility.Collapsed;
            }
            else
            {
                relP.Visibility = Visibility.Visible;
                if (pi.P3.Equals(0))
                {
                    p3rel.Visibility = Visibility.Collapsed;
                }
            }

        }
        private void moreM(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {           

            if (isVisible<Grid>(relM))
            {
               relM.Visibility = Visibility.Collapsed;
            }
            else
            {
               relM.Visibility = Visibility.Visible;
            }
        }
        
        private void Pr_Click(object sender, RoutedEventArgs e)
        {
            
            if (isVisible<StackPanel>(pr))
            {
                pr.Visibility = Visibility.Collapsed;
            }
            else
            {
                pr.Visibility = Visibility.Visible;
            }
        }

        private void Phones1_Click(object sender, RoutedEventArgs e)
        {
            uriCall = new Uri(@"ms-people:savetocontact?PhoneNumber=" + p1.Text + "&ContactName=Γραμματεία");
            makecall();
        }
        private void Phones2_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            uriCall = new Uri(@"ms-people:savetocontact?PhoneNumber=" + p1.Text + "&ContactName=Γραμματεία");
            makecall();
        }
        private void Phones3_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            uriCall = new Uri(@"ms-people:savetocontact?PhoneNumber=" + p1.Text + "&ContactName=Γραμματεία");
            makecall();
        }
        private async void Mail_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:" + mtext.Text));
            //mai.Text += mai.Text;            
        }

        private async void makecall()
        {
            if (uriCall != null)
            {
                var success = await Windows.System.Launcher.LaunchUriAsync(uriCall);
            }
        }
        
        private bool isVisible<T>(DependencyObject p)
        {
            return (p as UIElement).Visibility == Visibility.Visible;
        }
        
        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Classes));
        }

        private void Spudes_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Viewers.SpoudesViewer),pi);
        }
    }
}
