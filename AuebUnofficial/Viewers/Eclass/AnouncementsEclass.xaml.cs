using Flurl.Http;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;

namespace AuebUnofficial.Viewers
{
    public sealed partial class AnouncementsEclass : Page
    {
        public AnouncementsEclass()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string getit = await "https://eclass.aueb.gr/modules/mobile/mcourses.php"
               .PostUrlEncodedAsync(new { token = (string)e.Parameter })
               .ReceiveString();
            XDocument coursex = XDocument.Load(GenerateStreamFromString(getit));
            var ycourses = coursex.Root
                  .Elements("coursegroup").Elements("course")
                  .Select(x => new Course
                  {
                      Id = (string)x.Attribute("code"),
                      Name = (string)x.Attribute("title"),
                      Ans = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + ((string)x.Attribute("code").Value.Replace(@"\", string.Empty)))
                  })
                  .ToArray();
            CoursesViewer.ItemsSource = ycourses;
            //////////////////////////////////////////////////////////
            
            //////////////////////////////////////////////////////////
        }

       
        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
    class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Announcements> Ans { get; set; }
    }
}
