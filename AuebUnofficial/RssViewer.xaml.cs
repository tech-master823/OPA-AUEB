using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace AuebUnofficial
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RssViewer : Page
    {
        ArticlesDataSource articles;
        ArticlesDataSource articles1;
        Article art;
        public RssViewer()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
            this.Loaded += MainPage_Loades1;

        }
        
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            articles = new ArticlesDataSource("https://www.aueb.gr/el/taxonomy/term/701/feed");
            ListView.ItemsSource = articles;
        }
        void MainPage_Loades1(object sender, RoutedEventArgs e)
        {
            articles1 = new ArticlesDataSource("https://www.aueb.gr/el/aueb_news_feed.xml");
            ListView1.ItemsSource = articles1;
        }
        public void setArt(ListView l)
        {
            if (l == ListView)
            {
                art = (Article)ListView.SelectedItem;
            } else if (l == ListView1)
            {
                art = (Article)ListView1.SelectedItem;
            }
        }
        
        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            setArt(ListView);
            
            ((Frame)Window.Current.Content).Navigate(typeof(Viewers.RSSArtViewer),art);
        }
        private void StackPanel_Tapped1(object sender, TappedRoutedEventArgs e)
        {
            setArt(ListView1);
            ((Frame)Window.Current.Content).Navigate(typeof(Viewers.RSSArtViewer),art);
        }
    }      
}
