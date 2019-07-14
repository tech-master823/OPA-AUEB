using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Windows.Web.Syndication;
using AuebUnofficial;
using Flurl.Http;

public class EclassRssParser
{
    const string HTML_TAG_PATTERN = "<.*?>";
    public string LastU2Date { get; set;}
    public int RangeOfCourses { get; set; }//Number of Announcements in the course

    private ObservableCollection<EclassAnnouncement> items;
    public ObservableCollection<EclassAnnouncement> Announcements
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
        Announcements = new ObservableCollection<EclassAnnouncement>();
        LoadAnnouncements(struri);
    }
    private async void LoadAnnouncements(string struri)
    {
        this.RangeOfCourses = 0;
        var mystringtext = "";

        try
        {
            mystringtext = await struri.GetAsync().ReceiveString();
            SyndicationFeed feed = new SyndicationFeed();
            feed.Load(mystringtext);
            if (feed != null)
            {
                this.LastU2Date = feed.LastUpdatedTime.DateTime.ToString("MM/dd/yyyy HH:mm");
                foreach (SyndicationItem item in feed.Items)
                {
                    EclassAnnouncement an = new EclassAnnouncement
                    {
                        Title = StripHTML(item.Title.Text),
                        Description = StripHTML(item.Summary.Text),
                        DatePub = item.PublishedDate.DateTime.ToString("MM/dd/yyyy HH:mm"),
                        Link = item.Links[0].Uri
                    };
                    this.AddAnnouncement(an);
                }
                this.RangeOfCourses = items.Count;
            }
        }
        catch(FlurlHttpException)
        {
            Announcements = null;
        }
    }   
   
    public void AddAnnouncement(EclassAnnouncement announce)
    {
        Announcements.Add(announce);
    }
}

namespace AuebUnofficial{
    public class EclassAnnouncement
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
        public string Description { get; set; }
        public string DatePub { get; set; }
        public EclassAnnouncement() { }
        public EclassAnnouncement(string title, string description)
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