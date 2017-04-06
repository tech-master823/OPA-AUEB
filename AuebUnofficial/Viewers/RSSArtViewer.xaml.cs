using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{
    public sealed partial class RSSArtViewer : Page
    {

        public Article article { get; set; }
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
        }

        private void BButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(RssViewer));
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            DataTransferManager.ShowShareUI();
        }
        private void MainPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            
            args.Request.Data.SetText(article.Title);
            args.Request.Data.SetWebLink(article.Link);            
            args.Request.Data.Properties.Title = article.Title;
            args.Request.Data.Properties.Description = article.Description;
        }
    }
}
