using System;
using System.Collections.ObjectModel;
using Windows.Web.Syndication;


public class ArticlesDataSource : ObservableCollection<Article>
{

    
    private async void loada(Uri uri)
    {

        SyndicationClient client = new SyndicationClient();
        
        SyndicationFeed feed = await client.RetrieveFeedAsync(uri);
        if (feed != null)
        {
            foreach (SyndicationItem item in feed.Items)
            {
                Article ar = new Article();
                ar.Title = item.Title.Text;
                ar.Description = item.Summary.Text;
                ar.PubDate = item.PublishedDate.DateTime;
                ar.Link = item.Links[0].Uri;
                ar.Thesis = this.Count;
                loadData(ar);
                 
            }
        }
    }
    public ArticlesDataSource(string uri)
    {
        loada(new Uri(uri));
           

    }
   
    public void loadData(Article article)
    {
        this.Add(article);
    }
}

public class Article
    {
        private string title;
        private string description;
        private Uri link;
        private DateTime pubDate;
    private int thesis;

    public int Thesis
    {
        get { return thesis; }
        set { thesis = value; }
    }

        public DateTime PubDate
        {
            get { return pubDate; }
            set { pubDate = value; }
        }        
        public Uri Link
        {
            get { return link; }
            set { link = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    public Article() { }
        public Article(string title, string description)
        {
            this.title = title;
            this.description = description;
        }
    }




