using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Xml;
using System.ServiceModel;
using Windows.Web.Syndication;
using AuebUnofficial;

public class EclassRssParser : ObservableCollection<Announcements>
{
    public EclassRssParser(string struri)
    {
        loada(struri);
    }
    private async void loada(string struri)
    {
        var mystringtext = "";
        var handler = new HttpClientHandler { AllowAutoRedirect = true };
        var client = new HttpClient(handler);
        var response = await client.GetAsync(new Uri(struri));
        if (response.IsSuccessStatusCode)
        {
            response.EnsureSuccessStatusCode();
            mystringtext = await response.Content.ReadAsStringAsync();
        }

        if (mystringtext.Equals(""))
        {

        }
        else
        {
            SyndicationFeed feed = new SyndicationFeed();
            feed.Load(mystringtext);
            if (feed != null)
            {
                foreach (SyndicationItem item in feed.Items)
                {
                    Announcements an = new Announcements();
                    an.Title = item.Title.Text;
                    an.Description = item.Summary.Text;
                    an.DatePub = item.PublishedDate.DateTime.ToString("MM/dd/yyyy HH:mm");
                    //an.Link = item.Links[0].Uri;
                    this.loadData(an);
                }
            }
        }
    }   
   
    public void loadData(Announcements announce)
    {
        this.Add(announce);
    }
}

namespace AuebUnofficial{
    public class Announcements
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
        public string Description { get; set; }
        public string DatePub { get; set; }
        public Announcements() { }
        public override string ToString()
        {
            return Title + "\n" + Link + "\n" + Description;
        }
    }
}