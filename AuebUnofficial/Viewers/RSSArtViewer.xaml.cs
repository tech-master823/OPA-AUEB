using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{
    public sealed partial class RSSArtViewer : Page
    {
        
        Article article;
        public RSSArtViewer()
        {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            article = (Article)e.Parameter;
            title.Text = article.Title;
            pub.Text = article.PubDate;
            des.Text = article.Description;
        }

        private void BButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null && Frame.CanGoBack) Frame.GoBack();

        }
    }
}
