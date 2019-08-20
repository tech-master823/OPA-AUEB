using AppStudio.DataProviders.Twitter;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using EclassApi.Models;

namespace AuebUnofficial.Viewers
{
    public sealed partial class CommonWebView : Page
    {
        private Uri _uri;
        private Announcement _announcementParam = null;
        private TwitterSchema _tweetParam = null;
        public CommonWebView()
        {
            this.InitializeComponent();
            this.Loaded += CommonWebView_Loaded;
        }

        private void CommonWebView_Loaded(object sender, RoutedEventArgs e)
        {
            SiteView.Navigate(_uri);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parseurl = "";
            if (e.Parameter?.GetType() == typeof(TwitterSchema))
            {
                _tweetParam = (TwitterSchema)e.Parameter; parseurl="twitter.com"; Website.Text = parseurl;
                _uri = new Uri(_tweetParam.Url);
            }else if (e.Parameter?.GetType() == typeof(Announcement))
            {
                _announcementParam = (Announcement)e.Parameter; parseurl = "eclass.aueb.gr"; Website.Text = parseurl; Header.Text = _announcementParam.Title;
                _uri = _announcementParam.Link;
            }
            else
            {
                GoBack();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
