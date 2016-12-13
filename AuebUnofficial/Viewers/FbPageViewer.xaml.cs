using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AppStudio.DataProviders.Facebook;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

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
            string appId = "248058472262288";
            string appSecret = "d74d68d717ff1a0c45dc3fbad0899d26";
            string FacebookQueryParam = "625038207649339";
            int MaxRecordsParam = 12;
            Items.Clear();

            _facebookDataProvider = new FacebookDataProvider(new FacebookOAuthTokens { AppId = appId, AppSecret = appSecret });
            var config = new FacebookDataConfig
            {
                UserId = FacebookQueryParam
            };

            var items = await _facebookDataProvider.LoadDataAsync(config, MaxRecordsParam);

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
