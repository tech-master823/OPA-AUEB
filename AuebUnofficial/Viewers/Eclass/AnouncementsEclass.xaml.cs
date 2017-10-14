using AuebUnofficial.Helpers;
using AuebUnofficial.Viewers.Socials;
using Flurl.Http;
using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.Services.Twitter;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{
    public sealed partial class AnouncementsEclass : Page
    {
        private Announcement _CurrentAnnouncement;
        private int count = 0;
        Model.AnnouncementToken announcementToken;
        private ObservableCollection<Course> Ycourses;
        private string eclassUID="", courseCodeRequested;
        private App obj = App.Current as App;
        private EclassRssParser c;
        private MenuFlyout menuFlyout;
        public AnouncementsEclass()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.Loaded += An_LoadedAsync;
        }

        private async void correctCourses()
        {
            CoursesViewer.IsHitTestVisible = false;
            ProgressUpdate.IsActive = true;
            ProgressUpdate.Visibility = Visibility.Visible;
            CoursesViewer.Visibility = Visibility.Collapsed;
            if (obj.CurrentEclassUser.Uid == null)
            {              
                obj.CurrentEclassUser.Uid = eclassUID;
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
                        course.Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + course.Id + "&uid=" + obj.CurrentEclassUser.Uid + "&token=" + announcement.Token);
                        course.MyAnnouncements = c.Announcements;
                    }
                    catch (FlurlHttpException)
                    {
                        courseCodeRequested = course.Id;
                        var url = "https://eclass.aueb.gr/modules/announcements/?course=" +course.Id;
                        count = 2;
                        ResponceCode.Navigate(new Uri(url));
                        await Task.Delay(1400);
                        course.Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + announcementToken.ID + "&uid=" + obj.CurrentEclassUser.Uid + "&token=" + announcementToken.Token);
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
            AddFlyoutMenu();
            if (Ycourses == null) Ycourses = new ObservableCollection<Course>();
            else return;
            await FilCoursesAsync();
            ResponceCode.Navigate(new Uri("https://eclass.aueb.gr"));

        }

        private void AddFlyoutMenu()
        {
            menuFlyout = new MenuFlyout();
            MenuFlyoutItem share = new MenuFlyoutItem() { Text = "Share", Icon = new SymbolIcon(Symbol.Send) };
            share.Click += Share_Click;
            menuFlyout.Items.Add(share);
            MenuFlyoutItem copy = new MenuFlyoutItem() { Text = "Copy", Icon = new SymbolIcon(Symbol.Copy) };
            copy.Click += Copy_Click;
            menuFlyout.Items.Add(copy);
            MenuFlyoutItem copyLink = new MenuFlyoutItem() { Text = "Copy Link", Icon = new SymbolIcon(Symbol.Link) };
            copyLink.Click += CopyLink_Click;
            menuFlyout.Items.Add(copyLink);
        }

        private void CopyLink_Click(object sender, RoutedEventArgs e)
        {
            Copy(_CurrentAnnouncement.Link.ToString());
        }
        private void Copy(string text)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            if (text.Contains("http")) dataPackage.SetWebLink(new Uri(text));
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Copy(_CurrentAnnouncement.Title + Environment.NewLine + _CurrentAnnouncement.Description);
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += AnouncementsEclass_DataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void AnouncementsEclass_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = _CurrentAnnouncement.Title;
            args.Request.Data.SetWebLink(_CurrentAnnouncement.Link);
            args.Request.Data.SetText(_CurrentAnnouncement.Title + Environment.NewLine + _CurrentAnnouncement.Description);
        }

        private async Task FilCoursesAsync()
        {
            UnameTblock.Text = "Username: " + obj.CurrentEclassUser.Username;
            string getit = await "https://eclass.aueb.gr/modules/mobile/mcourses.php"
               .PostUrlEncodedAsync(new { token = obj.eclassToken })
               .ReceiveString();
            XDocument coursex = XDocument.Load(GenerateStreamFromString(getit));
            coursex.Root
                 .Elements("coursegroup").Elements("course")
                 .Select(x => new Course
                 {
                     Id = ((string)x.Attribute("code").Value.Replace(@"\", string.Empty)),
                     Name = (string)x.Attribute("title"),
                     Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + ((string)x.Attribute("code").Value.Replace(@"\", string.Empty))),
                     MyAnnouncements = c.Announcements
                     //LU2D=c.Announcements.ElementAt(0).DatePub,
                     //NoAn=c.Announcements.Count
                 }).ToList().ForEach(course => this.Ycourses.Add(course));
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
                string functionString = String.Format("document.getElementById('uname').innerText = '{0}';", obj.CurrentEclassUser.Username);
                await ResponceCode.InvokeScriptAsync("eval", new string[] { functionString });
                functionString = String.Format("document.getElementById('pass').innerText = '{0}';", obj.CurrentEclassUser.Password);
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
                    obj.CurrentEclassUser.Uid = eclassUID;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if(count!=0)correctCourses();
        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            var logout = await "https://eclass.aueb.gr/modules/mobile/mlogin.php?logout"
                .PostUrlEncodedAsync(new { token = obj.eclassToken })
                .ReceiveString();
            obj.eclassToken = null;
            if (this.Frame.CanGoBack) this.Frame.GoBack();
        }

        private void AnouncList_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            _CurrentAnnouncement = (e.OriginalSource as FrameworkElement)?.DataContext as Announcement;
            menuFlyout.ShowAt((FrameworkElement)sender, new Windows.Foundation.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
        }

        //private void AnouncList_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    _CurrentAnnouncement = (e.OriginalSource as FrameworkElement)?.DataContext as Announcement;

        //    ((Frame)Window.Current.Content).Navigate(typeof(CommonWebView), _CurrentAnnouncement);
        //}

        private void AnouncList_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            _CurrentAnnouncement = (e.OriginalSource as FrameworkElement)?.DataContext as Announcement;
            ((Frame)Window.Current.Content).Navigate(typeof(CommonWebView), _CurrentAnnouncement);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
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
