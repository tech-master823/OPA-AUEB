using AuebUnofficial.Core.Interfaces;
using AuebUnofficial.Core.Services;
using AuebUnofficial.Helpers;
using AuebUnofficial.Viewers.Notifications;
using Flurl.Http;
using HtmlAgilityPack;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using RavinduL.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{
    public sealed partial class AnouncementsEclass : Page
    {
        private EclassAnnouncement _CurrentAnnouncement;
        Core.Model.AnnouncementToken announcementToken;
        private ObservableCollection<Course> Ycourses;
        private string eclassUID = "", courseCodeRequested;
        private App _CurrentApp = App.Current as App;
        private EclassRssParser c;
        private MenuFlyout menuFlyout;
        private object p;
        private LocalNotificationManager notificationManager;
        private LNotifications lNotifications = new LNotifications("Courses loaded succedfully!", "Logged out succesfully!");
        private IEclassService _eclassService;
        public AnouncementsEclass()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            _eclassService = new EclassService();
            this.Loaded += An_LoadedAsync;
        }

        public AnouncementsEclass(object p)
        {
            this.p = p;
        }

        private async void An_LoadedAsync(object sender, RoutedEventArgs d)
        {

            AddFlyoutMenu();
            //If this page is in the NavigationStack return, else go
            if (Ycourses == null)
            {
                CoursesViewer.Visibility = Visibility.Collapsed;
                CoursesViewer.IsHitTestVisible = false;
                ProgressUpdate.IsActive = true;
                ProgressUpdate.Visibility = Visibility.Visible;
                Ycourses = new ObservableCollection<Course>();
            }
            else
            {
                CoursesViewer.IsHitTestVisible = true;
                ProgressUpdate.IsActive = false;
                ProgressUpdate.Visibility = Visibility.Collapsed;
                CoursesViewer.Visibility = Visibility.Visible;
                return;
            }
            //
            await FilCoursesAsync(_CurrentApp.eclassToken);
            if (_CurrentApp.CurrentEclassUser.Uid == null)
            {
                _CurrentApp.CurrentEclassUser.Uid = eclassUID = GetUid(_CurrentApp.eclassToken);
            }
            CorrectClosedCourses();

        }
        private async Task FilCoursesAsync(string tokenSeq)
        {
            UnameTblock.Text = "Username: " + _CurrentApp.CurrentEclassUser.Username;
            string coursesXML = await "https://eclass.aueb.gr/modules/mobile/mcourses.php"
               .PostUrlEncodedAsync(new { token = tokenSeq })
               .ReceiveString();
            XDocument coursesXDocument = XDocument.Load(GenerateStreamFromString(coursesXML));
            coursesXDocument.Root
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
            CoursesViewer.ItemsSource = Ycourses;
        }

        private async Task CorrectClosedCourses()
        {


            foreach (Course course in Ycourses)
            {
                if (course.MyAnnouncements.Count == 0)
                {
                    try
                    {

                        var url = "http://auebunofficialapi.azurewebsites.net/Announcements/Details/" + course.Id;
                        Core.Model.AnnouncementToken announcement;
                        var response = await url.GetAsync().ReceiveString();
                        announcement = JsonConvert.DeserializeObject<Core.Model.AnnouncementToken>(response);
                        course.Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + course.Id + "&uid=" + _CurrentApp.CurrentEclassUser.Uid + "&token=" + announcement.Token);
                        course.MyAnnouncements = c.Announcements;
                    }
                    catch (FlurlHttpException)
                    {
                        courseCodeRequested = course.Id;
                        var url = "https://eclass.aueb.gr/modules/announcements/?course=" + course.Id;
                        announcementToken = GetToken(_CurrentApp.eclassToken);
                        course.Ans = c = new EclassRssParser("https://eclass.aueb.gr/modules/announcements/rss.php?c=" + announcementToken.ID + "&uid=" + _CurrentApp.CurrentEclassUser.Uid + "&token=" + announcementToken.Token);
                        course.MyAnnouncements = c.Announcements;
                        var urlCreate = "http://auebunofficialapi.azurewebsites.net/Announcements/Create/";
                        try
                        {
                            var response = await urlCreate.PostUrlEncodedAsync(new { ID = announcementToken.ID, Token = announcementToken.Token });
                        }
                        catch (Exception) { Analytics.TrackEvent("Cannot upload to API"); }

                    }
                }
            }
            CoursesViewer.IsHitTestVisible = true;
            ProgressUpdate.IsActive = false;
            ProgressUpdate.Visibility = Visibility.Collapsed;
            CoursesViewer.Visibility = Visibility.Visible;
            notificationManager = new LocalNotificationManager(DoneNotification);
            notificationManager.Show(lNotifications.GetPositiveNotification(),LocalNotificationCollisionBehaviour.Replace);
        }


    
        #region Buttons
        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            notificationManager = new LocalNotificationManager(DoneNotification);
            notificationManager.Show(lNotifications.GetNegativeNotification(), LocalNotificationCollisionBehaviour.Replace);
            var logout = await _eclassService.LogoutAsync(_CurrentApp.eclassToken);
            await Task.Delay(1000);
            _CurrentApp.eclassToken = null;
            Ycourses = null;            
            ((Frame)Window.Current.Content).Navigate(typeof(eclass_Nat));

        }

        private void AnouncList_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            _CurrentAnnouncement = (e.OriginalSource as FrameworkElement)?.DataContext as EclassAnnouncement;
            menuFlyout.ShowAt((FrameworkElement)sender, new Windows.Foundation.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
        }
        private void AnouncList_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            _CurrentAnnouncement = (e.OriginalSource as FrameworkElement)?.DataContext as EclassAnnouncement;
            ((Frame)Window.Current.Content).Navigate(typeof(CommonWebView), _CurrentAnnouncement);
        }

        private void CopyLink_Click(object sender, RoutedEventArgs e)
        {
            Copy(_CurrentAnnouncement.Link.ToString());
            lNotifications = new LNotifications("Link copied succesfully!");
            notificationManager.Show(lNotifications.GetPositiveNotification());
        }
        private void Copy(string text)
        {
            DataPackage dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            if (text.Contains("http")) dataPackage.SetWebLink(new Uri(text));
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);            
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Copy(_CurrentAnnouncement.Title + Environment.NewLine + _CurrentAnnouncement.Description);
            lNotifications = new LNotifications("Announcement copied succesfully!");
            notificationManager.Show(lNotifications.GetPositiveNotification());
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
        #endregion Buttons
        #region Utilities
        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
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
        #endregion Utilities
        #region Helpers(Scrappers)
        public Core.Model.AnnouncementToken GetToken(string tokenSeq)
        {
            var CourseAnnouncementsHtml = "";
            Task.Run(async () => { CourseAnnouncementsHtml = await ("https://eclass.aueb.gr/modules/announcements/?course="+courseCodeRequested).PostUrlEncodedAsync(new { token = tokenSeq }).ReceiveString(); }).GetAwaiter().GetResult();
            var doc = new HtmlDocument(); doc.LoadHtml(CourseAnnouncementsHtml);
            

            var value = doc.DocumentNode.Descendants("a").Where(x => x.Attributes.Contains("href"));
            var myval = value.Where(y => y.Attributes["href"].Value.Contains("/modules/announcements/rss.php"));
            announcementToken = new Core.Model.AnnouncementToken() { ID = courseCodeRequested, Token = (myval.First().Attributes["href"].Value.Split('&').GetValue(2).ToString()).Split('=').Last().ToString() };
            return announcementToken;
        }
        public string GetUid(string tokenSeq)
        {
            string portfolioHTML = "";
            List<string> hrefs = new List<string>();
            Task.Run(async () => { portfolioHTML = await "https://eclass.aueb.gr/main/portfolio.php".PostUrlEncodedAsync(new { token = tokenSeq }).ReceiveString(); }).GetAwaiter().GetResult();
            HtmlDocument portfolioDocumentPage = new HtmlDocument(); portfolioDocumentPage.LoadHtml(portfolioHTML);
            portfolioDocumentPage.DocumentNode.SelectNodes("//a[@href]").ToList().ForEach(node => hrefs.Add(node.Attributes["href"].Value));
            return hrefs.Where(href => href.Contains("uid")).ToList().First().Split("&amp;".ToCharArray()).Last().Split('=').Last();
        }
        #endregion Helpers(Scrappers)
    }    
}
namespace AuebUnofficial
{
    public class Course : BindableBase
    {
        private ObservableCollection<EclassAnnouncement> myAnnouncements;
        private EclassRssParser ans;
        public string Id { get; set; }
        public string Name { get; set; }
        public EclassRssParser Ans
        {
            get { return ans; }
            set { this.SetProperty(ref this.ans, value); }
        }
        public ObservableCollection<EclassAnnouncement> MyAnnouncements
        {
            get { return myAnnouncements; }
            set { this.SetProperty(ref this.myAnnouncements, value); }
        }
        public string LU2D { get; set; }
        public int NoAn { get; set; }
    }
}
