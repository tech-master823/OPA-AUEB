using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.ApplicationModel.DataTransfer;
using AppStudio.DataProviders.Twitter;

namespace AuebUnofficial.Viewers.Socials
{

    public sealed partial class Twitter : Page
    {
        ObservableCollection<object> items = null;
        private TwitterSchema _CurrentTweet = null;
        private MenuFlyout menuFlyout;
        private string _CurrentTweetURL;
        private string twitterQueryParam="aueb";

        public TwitterDataProvider _twitterDataProvider { get; private set; }

        public Twitter()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = this;
            this.Loaded += Twitter_Loaded;
        }

        private async void Twitter_Loaded(object sender, RoutedEventArgs e)
        {
            if (items == null)
            {
                items = new ObservableCollection<object>();
                string consumerKey = "5zcs3Bp2kTlrsDUsMDv5BYfND",
                consumerSecret = "oZDbzyY6xmPJVskIUx0pyA4VlB6XdMQHPb4sjHDxUshgCStxzf",
                accessToken = "248216142-jMUS9hvL97gu8fwCpkD7XqkyDBfJypvvogNybchv",
                accessTokenSecret = "coDm0ePUoWIXlIH5v46jDJFUYANVG4SvGADrUL5Kf8aSQ";
                
                
                TwitterQueryType queryType = TwitterQueryType.Search;
                int maxRecordsParam = 20;


                _twitterDataProvider = new TwitterDataProvider(new TwitterOAuthTokens
                {
                    AccessToken = accessToken,
                    AccessTokenSecret = accessTokenSecret,
                    ConsumerKey = consumerKey,
                    ConsumerSecret = consumerSecret
                });

                var config = new TwitterDataConfig
                {
                    Query = twitterQueryParam,
                    QueryType = queryType
                };

                var results = await _twitterDataProvider.LoadDataAsync(config, maxRecordsParam);
                foreach (TwitterSchema item in results)
                {
                    items.Add(item);
                }

                // Initialize service
                
                // Search for a specific tag
                //(await TwitterService.Instance.SearchAsync("#exetastiki", 20)).ToList().ForEach(i => items.Add(i));
                //(await TwitterService.Instance.SearchAsync("@aueb", 50)).ToList().ForEach(i => items.Add(i));
                //items.OrderBy(t => t.CreationDate);
                //items.ToList().ForEach(t => t.CreatedAt = "Published at " + t.CreationDate.ToString("dd/MM/yyyy HH:mm"));
                TwitterFeed.ItemsSource = items;
            }
            AddFlyoutMenu();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                Frame.GoBack();
        }

        private void TwitterFeed_ItemClick(object sender, ItemClickEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(CommonWebView), (TwitterSchema)e.ClickedItem);
        }

        private void TwitterFeed_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            _CurrentTweet = (e.OriginalSource as FrameworkElement)?.DataContext as TwitterSchema;
            _CurrentTweetURL =_CurrentTweet.Url;
            menuFlyout.ShowAt(((FrameworkElement)sender), new Windows.Foundation.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
        }

        private void CopyLink_Click(object sender, RoutedEventArgs e)
        {
            Copy(_CurrentTweetURL);
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Copy(_CurrentTweet.Text);
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += Twitter_DataRequested;
            DataTransferManager.ShowShareUI();
        }        

        private void TwitterFeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _CurrentTweet = (TwitterSchema)e.AddedItems[0];
        }
        private void Copy(string text)
        {
            DataPackage dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            if (text.Contains("http")) dataPackage.SetWebLink(new Uri(text));
            dataPackage.SetText(text);        
            Clipboard.SetContent(dataPackage);
        }
        private void Twitter_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = "Tweet from @" + _CurrentTweet.UserName;
            args.Request.Data.SetWebLink(new Uri(_CurrentTweetURL));
            args.Request.Data.SetText(_CurrentTweet.Text);
        }
        private void AddFlyoutMenu()
        {
            menuFlyout = new MenuFlyout();
            MenuFlyoutItem share = new MenuFlyoutItem() { Text = "Share", Icon = new SymbolIcon(Symbol.Send) };
            share.Click += Share_Click;
            menuFlyout.Items.Add(share);
            MenuFlyoutItem copy = new MenuFlyoutItem() { Text = "Copy", Icon = new SymbolIcon(Symbol.Copy) };
            copy.Click += Copy_Click;
            menuFlyout.Items.Add(copy);
            MenuFlyoutItem copyLink = new MenuFlyoutItem() { Text = "Copy Link", Icon = new SymbolIcon(Symbol.Link) };
            copyLink.Click += CopyLink_Click;
            menuFlyout.Items.Add(copyLink);
            
        }

    }
}
