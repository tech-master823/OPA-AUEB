using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.Web.Syndication;
using AuebUnofficial;

public class EclassRssParser
{
    const string HTML_TAG_PATTERN = "<.*?>";
    public string LastU2Date { get; set;}
    public int RangeOCourses { get; set; }//Number of Announcements in the course

    private ObservableCollection<Announcement> items;
    public ObservableCollection<Announcement> Announcements
    {
        get { return items; }
        set { items = value; }
    }    
    static string StripHTML(string inputString)
    {
        string tmp = inputString;
        tmp =Regex.Replace
          (tmp, HTML_TAG_PATTERN, string.Empty);
        tmp=Regex.Replace(tmp, "&#", string.Empty);
        tmp = Regex.Replace(tmp, "160;", string.Empty);
        return tmp;
    }
    public EclassRssParser(string struri)
    {
        Announcements = new ObservableCollection<Announcement>();
        loada(struri);
    }
    private async void loada(string struri)
    {
        this.RangeOCourses = 0;
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
            Announcements=null;
        }
        else
        {
            SyndicationFeed feed = new SyndicationFeed();
            feed.Load(mystringtext);
            if (feed != null)
            {
                this.LastU2Date = feed.LastUpdatedTime.DateTime.ToString("MM/dd/yyyy HH:mm");
                foreach (SyndicationItem item in feed.Items)
                {
                    Announcement an = new Announcement();
                    an.Title = StripHTML(item.Title.Text);
                    an.Description = StripHTML(item.Summary.Text);
                    an.DatePub = item.PublishedDate.DateTime.ToString("MM/dd/yyyy HH:mm");
                    an.Link = item.Links[0].Uri;
                    this.loadData(an);
                }
                this.RangeOCourses = items.Count;
            }
        }
    }   
   
    public void loadData(Announcement announce)
    {
        Announcements.Add(announce);
    }
}

namespace AuebUnofficial{
    public class Announcement
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
        public string Description { get; set; }
        public string DatePub { get; set; }
        public Announcement() { }
        public Announcement(string title, string description)
        {
            Title = title;
            Description = description;
        }
        public override string ToString()
        {
            return Title + "\n" + Link + "\n" + Description;
        }
    }
}