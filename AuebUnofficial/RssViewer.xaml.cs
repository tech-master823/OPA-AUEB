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
        public RssViewer()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

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
    }
}
