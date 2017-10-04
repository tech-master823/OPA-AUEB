using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Services;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Services.Twitter;
using Windows.UI.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Windows.UI;

namespace AuebUnofficial.Viewers.Socials
{
    
    public sealed partial class Twitter : Page
    {
        ObservableCollection<Tweet> items = null;
        public Twitter()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = this;            
            this.Loaded += Twitter_LoadedAsync;
        }

        private async void Twitter_LoadedAsync(object sender, RoutedEventArgs e)
        {
            if (items == null)
            {
                items = new ObservableCollection<Tweet>();
                // Initialize service
                TwitterService.Instance.Initialize("5zcs3Bp2kTlrsDUsMDv5BYfND", "oZDbzyY6xmPJVskIUx0pyA4VlB6XdMQHPb4sjHDxUshgCStxzf", "http://auebunofficialapi.azurewebsites.net/");

                // Search for a specific tag
                (await TwitterService.Instance.SearchAsync("aueb", 50)).ToList().ForEach(i => items.Add(i));
                (await TwitterService.Instance.SearchAsync("#exetastiki", 50)).ToList().ForEach(i => items.Add(i));
                items.OrderBy(t => t.CreationDate);
                TwitterFeed.ItemsSource = items;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                Frame.GoBack(); 
        }

        private void TwitterFeed_ItemClick(object sender, ItemClickEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(CommonWebView), (Tweet)e.ClickedItem);
            
    }
}
