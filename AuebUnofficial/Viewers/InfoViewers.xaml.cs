using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{

    public sealed partial class InfoViewers : Page
    {
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
        }



        private void moreP_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            if (isVisibleSP(relP))
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

            if (isVisibleRP(relM))
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
            
            if (isVisibleSP(pr))
            {
                pr.Visibility = Visibility.Collapsed;
            }
            else
            {
                pr.Visibility = Visibility.Visible;
            }
        }

        private void Phones1_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {



        }
        private void Phones2_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
        private void Phones3_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
        private async void Mail_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:" + mtext.Text));
            //mai.Text += mai.Text;            
        }

        
        private bool isVisibleRP(RelativePanel p)
        {
            return p.Visibility == Visibility.Visible;
        }
        private bool isVisibleSP(StackPanel s)
        {
            return s.Visibility == Visibility.Visible;
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
