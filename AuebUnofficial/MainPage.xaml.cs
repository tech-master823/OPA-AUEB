using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AuebUnofficial
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();           
        }        
        private async void ReportBug(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:?to=anas.moraitis@gmail.com&subject=BugAtAuebUWPApp"));
            mail.Text = "\uE8C3";
        }

        private void FBButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectionHelper.IsInternetAvailable == true)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(Viewers.FbPageViewer));
            }else if (ConnectionHelper.IsInternetAvailable == false)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(RssViewer));
            }

        }
        private void LiButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Viewers.LinkedinAueb));
        }
        private void TwiButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Viewers.Socials.Twitter));
        }

        private void auebgr_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            auebgr_pbar.IsActive = false;
            auebgr_pbar.Visibility = Visibility.Collapsed;
            auebgr.Visibility = Visibility.Visible;
        }

        private void auebgr_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            auebgr.Visibility = Visibility.Collapsed;      
            auebgr.Navigate(new Uri("http://www.aueb.gr"));
        }
    }
}
