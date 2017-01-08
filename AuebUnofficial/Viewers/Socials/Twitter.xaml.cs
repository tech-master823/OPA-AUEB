using Windows.UI.Xaml.Controls;
using AppStudio.DataProviders.Twitter;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers.Socials
{

    public sealed partial class Twitter : Page
    {
        private TwitterDataProvider _twitterDataProvider;
        public Twitter()
        {
            this.InitializeComponent(); this.DataContext = this;
        }

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(Twitter), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>();
            GetItems();
        }

        public async void GetItems()
        {
            string consumerKey = "5zcs3Bp2kTlrsDUsMDv5BYfND";
            string consumerSecret = "oZDbzyY6xmPJVskIUx0pyA4VlB6XdMQHPb4sjHDxUshgCStxzf";
            string accessToken = "	248216142-jMUS9hvL97gu8fwCpkD7XqkyDBfJypvvogNybchv";
            string accessTokenSecret = "coDm0ePUoWIXlIH5v46jDJFUYANVG4SvGADrUL5Kf8aSQ";
            string twitterQueryParam = "WindowsAppStudio";
            TwitterQueryType queryType = TwitterQueryType.Search;
            int maxRecordsParam = 12;

            Items.Clear();

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
            
            var items = await _twitterDataProvider.LoadDataAsync(config, maxRecordsParam);
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        private async void GetMoreItems()
        {
            var items = await _twitterDataProvider.LoadMoreDataAsync();

            foreach (var item in items)
            {
                Items.Add(item);
            }
        }
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
