using AppStudio.DataProviders.Twitter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AuebUnofficial.Viewers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CommonWebView : Page
    {
        string baseurl = "";
        Uri uri;
        Announcement announcementParam = null;
        TwitterSchema tweetparam = null;
        public CommonWebView()
        {
            this.InitializeComponent();
            this.Loaded += CommonWebView_Loaded;
        }

        private void CommonWebView_Loaded(object sender, RoutedEventArgs e)
        {
            SiteView.Navigate(uri);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var baseurl = "";
            if (e.Parameter.GetType() == typeof(TwitterSchema))
            {
                tweetparam = (TwitterSchema)e.Parameter; baseurl="twitter.com"; Website.Text = baseurl;
                uri = new Uri(tweetparam.Url);
            }else if (e.Parameter.GetType() == typeof(Announcement))
            {
                announcementParam = (Announcement)e.Parameter; baseurl = "eclass.aueb.gr"; Website.Text = baseurl; Header.Text = announcementParam.Title;
                uri = announcementParam.Link;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
