using AuebUnofficial.Helpers;
using Flurl.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{
    public sealed partial class AnouncementsEclass : Page
    {
        private int count = 0;
        Model.AnnouncementToken announcementToken;
        private Course[] Ycourses;
        private string eclassUID="", courseCodeRequested;
        private App obj = App.Current as App;
        private EclassRssParser c;
        public AnouncementsEclass()
        {
            this.InitializeComponent();
            this.Loaded += An_LoadedAsync;
        }

        private async void correctCourses()
        {
            if (obj.eclassUID == null)
            {              
                obj.eclassUID = eclassUID;
            }
            foreach (Course course in Ycourses)
            {
                if (course.MyAnnouncements.Count == 0)
                {
                    try
                    {

                        var url = "http://auebunofficialapi.azurewebsites.net/Announcements/Details/" + course.Id;
                        Model.AnnouncementToken announcement;
                        var response = await url.GetAsync().ReceiveString();
                        announcement = JsonConvert.DeserializeObject<Model.AnnouncementToken>(response);
                        course.Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + course.Id + "&uid=" + obj.eclassUID + "&token=" + announcement.Token);
                        course.MyAnnouncements = c.Announcements;
                    }
                    catch (FlurlHttpException)
                    {
                        courseCodeRequested = course.Id;
                        var url = "https://eclass.aueb.gr/modules/announcements/?course=" +course.Id;
                        count = 2;
                        ResponceCode.Navigate(new Uri(url));
                        await Task.Delay(1400);
                        course.Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + announcementToken.ID + "&uid=" + obj.eclassUID + "&token=" + announcementToken.Token);
                        course.MyAnnouncements = c.Announcements;
                        var urlCreate = "http://auebunofficialapi.azurewebsites.net/Announcements/Create/";
                        var response = await urlCreate.PostUrlEncodedAsync(new { ID = announcementToken.ID, Token = announcementToken.Token });
                    }
                }
            }
            CoursesViewer.IsHitTestVisible = true;
            ProgressUpdate.IsActive = false;
            ProgressUpdate.Visibility = Visibility.Collapsed;
            CoursesViewer.Visibility = Visibility.Visible;
        }
         
        private async void An_LoadedAsync(object sender, RoutedEventArgs d)
        {
            await FilCoursesAsync();
            ResponceCode.Navigate(new Uri("https://eclass.aueb.gr"));

        }
        private async Task FilCoursesAsync()
        {
            UnameTblock.Text = "Username: " + obj.eclassUsername;
            string getit = await "https://eclass.aueb.gr/modules/mobile/mcourses.php"
               .PostUrlEncodedAsync(new { token = obj.eclassToken })
               .ReceiveString();
            XDocument coursex = XDocument.Load(GenerateStreamFromString(getit));
            Ycourses = coursex.Root
                 .Elements("coursegroup").Elements("course")
                 .Select(x => new Course
                 {
                     Id = ((string)x.Attribute("code").Value.Replace(@"\", string.Empty)),
                     Name = (string)x.Attribute("title"),
                     Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + ((string)x.Attribute("code").Value.Replace(@"\", string.Empty))),
                     MyAnnouncements = c.Announcements
                     //LU2D=c.Announcements.ElementAt(0).DatePub,
                     //NoAn=c.Announcements.Count
                 })
                 .ToArray();
            courseCodeRequested = Ycourses.Last().Id;
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
        private async void ResponceCode_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (count == 0)
            {
                string functionString = String.Format("document.getElementById('uname').innerText = '{0}';", obj.eclassUsername);
                await ResponceCode.InvokeScriptAsync("eval", new string[] { functionString });
                functionString = String.Format("document.getElementById('pass').innerText = '{0}';", obj.eclassPass);
                await ResponceCode.InvokeScriptAsync("eval", new string[] { functionString });
                functionString = String.Format("document.getElementsByTagName('button')[document.getElementsByTagName('button').length-1].click();");
                await ResponceCode.InvokeScriptAsync("eval", new[] { functionString });
                count++;
            }
            else if (count == 1)
            {
                ResponceCode.Navigate(new Uri("https://eclass.aueb.gr/modules/announcements/?course=" + courseCodeRequested));
                count++;
            }
            else if (count == 2)
            {
                var scode = await ResponceCode.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" });
                var doc = new HtmlDocument();
                doc.LoadHtml(scode);
                var value = doc.DocumentNode.Descendants("a").Where(x => x.Attributes.Contains("href"));
                var myval = value.Where(y => y.Attributes["href"].Value.Contains("/modules/announcements/rss.php"));
                if (eclassUID.Equals(""))
                {
                    eclassUID = (myval.First().Attributes["href"].Value.Split('&').GetValue(1).ToString()).Split('=').Last().ToString();
                    await StoreUserIdAsync(eclassUID);
                }
                else
                {
                    announcementToken = new Model.AnnouncementToken() { ID = courseCodeRequested, Token = (myval.First().Attributes["href"].Value.Split('&').GetValue(2).ToString()).Split('=').Last().ToString() };
                }           

                count++;
                ResponceCode.Navigate(new Uri("https://eclass.aueb.gr/"));
            }
            else if(count==3)
            {
                if(!eclassUID.Equals(""))correctCourses();
                
                count++;
            }
        }

        private void ResponceCode_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var url = e.Value;
        }
        public async Task<string> StoreUserIdAsync(string eclassUID)
        {
            var fileName = "user_id";
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.TryGetItemAsync(fileName);
            if (file == null || obj.eclassUID==null)
            {
                //if file does not exist we create a new guid
                var storageFile = await folder.CreateFileAsync(fileName);
                await FileIO.WriteTextAsync(storageFile, eclassUID);
                return eclassUID;
            }
            else
            {
                return "";
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            this.Content = null;
        }
    }
    

    
}
namespace AuebUnofficial
    {
        public class Course : BindableBase
    {
        private ObservableCollection<Announcement> myAnnouncements;
        private EclassRssParser ans;
        public string Id { get; set; }
        public string Name { get; set; }
        public EclassRssParser Ans
        {
            get { return ans; }
            set { this.SetProperty(ref this.ans, value); }
        }
        public ObservableCollection<Announcement> MyAnnouncements
        {
            get { return myAnnouncements; }
            set { this.SetProperty(ref this.myAnnouncements, value); }
        }
        public string LU2D { get; set; }
        public int NoAn { get; set; }
    }
}
