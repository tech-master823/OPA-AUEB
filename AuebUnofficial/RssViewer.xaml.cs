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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AuebUnofficial
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RssViewer : Page
    {
        private int x = 0;
        ArticlesDataSource articles;
        ArticlesDataSource articles1;
        public RssViewer()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Loaded += MainPage_Loades1;

        }
        private void ShowSliptView(object sender, RoutedEventArgs e)
        {
            MySamplesPane.SamplesSplitView.IsPaneOpen = !MySamplesPane.SamplesSplitView.IsPaneOpen;
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            articles = new ArticlesDataSource("http://www.aueb.gr/pages/news/RSS/anakoinoseis_akad.xml");

            
            ListView.ItemsSource = articles;
        }
        void MainPage_Loades1(object sender, RoutedEventArgs e)
        {
            articles1 = new ArticlesDataSource("http://aueb.gr/pages/news/RSS/anakoinoseis_pryt.xml");
            ListView1.ItemsSource = articles1;
        }

        private async void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {

            var uriBing = new Uri(articles[x].Link.ToString());

            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(uriBing);

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
            x++;
        }
        private async void StackPanel_Tapped1(object sender, TappedRoutedEventArgs e)
        {

            var uriBing = new Uri(articles1[x].Link.ToString());

            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(uriBing);

            if (success)
            {
                
            }
            else
            {
                // URI launch failed
            }
            x++;
        }
    }
}
