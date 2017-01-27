using Flurl.Http;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{
    public sealed partial class AnouncementsEclass : Page
    {
        public AnouncementsEclass()
        {
            this.InitializeComponent();
            this.Loaded += An_Loaded;
        }
        private async void An_Loaded(object sender, RoutedEventArgs d)
        {
            EclassRssParser c;
            var obj = App.Current as App;
            UnameTblock.Text = "Username: " + obj.eclassUsername;
            string getit = await "https://eclass.aueb.gr/modules/mobile/mcourses.php"
               .PostUrlEncodedAsync(new { token = obj.eclassToken })
               .ReceiveString();
            XDocument coursex = XDocument.Load(GenerateStreamFromString(getit));
            var Ycourses = coursex.Root
                 .Elements("coursegroup").Elements("course")
                 .Select(x => new Course
                 {
                     Id = ((string)x.Attribute("code").Value.Replace(@"\", string.Empty)),
                     Name = (string)x.Attribute("title"),
                     Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + ((string)x.Attribute("code").Value.Replace(@"\", string.Empty)))
                     MyAnnouncements = c.Announcements,
                     //LU2D=c.Announcements.ElementAt(0).DatePub,
                     //NoAn=c.Announcements.Count
                 })
                 .ToArray();
            
           CoursesViewer.ItemsSource = Ycourses;
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
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public EclassRssParser Ans { get; set; }
        public ObservableCollection<Announcement> MyAnnouncements { get; set;}
        public string LU2D { get; set; }
        public int NoAn { get; set; }
    }
}
