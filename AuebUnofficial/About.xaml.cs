using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace AuebUnofficial
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        DefPack dpack;
        public About()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            dpack = new DefPack();
            GridView.ItemsSource = dpack;
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var uriBing = new Uri("https://amoraitis.github.io/Portfolio/");

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
        }

        private async void GitHub(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var uriBing = new Uri("https://github.com/amoraitis");

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
        }
        private async void Lin(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var uriBing = new Uri("https://www.linkedin.com/in/anamoraitis");

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
        }
    }
}
