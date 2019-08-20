using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppStudio.DataProviders.Facebook;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using System;

namespace AuebUnofficial.Viewers
{

    public sealed partial class FbPageViewer : Page
    {
        private FacebookDataProvider _facebookDataProvider;
        public FbPageViewer()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty
            .Register(nameof(Items), typeof(ObservableCollection<object>), typeof(FbPageViewer), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>();
            GetItems();
        }

        public async void GetItems()
        {
            string appId = (App.Current as App).AppSettings.FacebookAppId;
            string appSecret = (App.Current as App).AppSettings.FacebookAppSecret;
            string FacebookQueryParam = "625038207649339";
            int MaxRecordsParam = 20;
            Items.Clear();

            _facebookDataProvider = new FacebookDataProvider(new FacebookOAuthTokens { AppId = appId, AppSecret = appSecret });
            var config = new FacebookDataConfig
            {
                UserId = FacebookQueryParam
            };

            var items = await _facebookDataProvider.LoadDataAsync(config, MaxRecordsParam);


            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.ImageUrl)) item.ImageUrl = "https://eclass.aueb.gr/courses/theme_data/9/eclass_aueb_logo.png";
                Items.Add(item);
            }

            pring.IsActive = false;
            pring.Visibility = Visibility.Collapsed;
            FeedFb.Visibility = Visibility.Visible;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void FeedFb_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FacebookSchema fbitem = (FacebookSchema)FeedFb.SelectedItem;
            var success=await Windows.System.Launcher.LaunchUriAsync(new Uri(fbitem.FeedUrl));
        }
    }
}