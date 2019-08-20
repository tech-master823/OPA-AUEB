using Microsoft.Toolkit.Extensions;
using AuebUnofficial.Viewers.Notifications;
using EclassApi;
using RavinduL.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Akavache;
using AuebUnofficial.Core.Converters;
using AuebUnofficial.Helpers;
using EclassApi.Models;
using Newtonsoft.Json;
using RavinduL.LocalNotifications.Notifications;

namespace AuebUnofficial.Viewers.Eclass
{
    public sealed partial class AnnouncementsEclass : Page
    {
        private Announcement _currentAnnouncement;
        public ObservableCollection<CourseViewModel> Courses { get; set; } = new ObservableCollection<CourseViewModel>();
        private readonly App _currentApp = App.Current as App;
        private MenuFlyout _menuFlyout;
        private EclassUser _eclassSession;
        private LocalNotificationManager _notificationManager;
        private LNotifications _lNotifications = new LNotifications("Courses loaded successfully!", "Logged out successfully!");
        public AnnouncementsEclass()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.Loaded += AnnouncementsEclass_Loaded;

        }

        private void AnnouncementsEclass_Loaded(object sender, RoutedEventArgs e)
        {
            this._eclassSession = GetStoredSession();
            if (_eclassSession == null)
            {
                _notificationManager.Show(new SimpleNotification()
                {
                    Text = "Couldn't fetch data",
                    Background = new SolidColorBrush(Colors.DarkOrange),
                    Foreground = new SolidColorBrush(Colors.Black)
                }, LocalNotificationCollisionBehaviour.Replace);

                return;
            }
            UnameTblock.Text = "Username: " + _eclassSession?.Username;
            AddFlyoutMenu();
            //If this page is in the NavigationStack return, else go
            if (Courses == null)
            {
                CoursesViewer.Visibility = Visibility.Collapsed;
                CoursesViewer.IsHitTestVisible = false;
                ProgressUpdate.IsActive = true;
                ProgressUpdate.Visibility = Visibility.Visible;
                _eclassSession.UserCourses.ForEach(c => Courses.Add(new CourseViewModel(c)));

                ProgressUpdate.Visibility = Visibility.Collapsed;
                ProgressUpdate.IsActive = false;
                CoursesViewer.IsHitTestVisible = true;
                CoursesViewer.Visibility = Visibility.Visible;

            }
            else
            {
                CoursesViewer.IsHitTestVisible = true;
                ProgressUpdate.IsActive = false;
                ProgressUpdate.Visibility = Visibility.Collapsed;
                CoursesViewer.Visibility = Visibility.Visible;
                return;
            }
            if (_currentApp.CurrentEclassUser.Uid == null)
            {
                _currentApp.CurrentEclassUser.Uid = _eclassSession.Uid;
            }
        }

        private EclassUser GetStoredSession()
        {
            try
            {
                var jsonSession = BlobCache.InMemory.GetObject<string>("eclassData").GetAwaiter().GetResult();

                var result =
                        JsonConvert.DeserializeObject<EclassUser>(jsonSession, new ToolItemConverter());

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.Message);
                return null;
            }
        }


        #region Buttons
        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            _notificationManager = new LocalNotificationManager(DoneNotification);
            _notificationManager.Show(_lNotifications.GetNegativeNotification(), LocalNotificationCollisionBehaviour.Replace);
            await _eclassSession.DestroySessionAsync();
            _currentApp.eclassToken = null;
            Courses = null;
            ((Frame)Window.Current.Content).Navigate(typeof(EclassNat));

        }

        private void AnnouncementsListView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            _currentAnnouncement = (e.OriginalSource as FrameworkElement)?.DataContext as Announcement;
            _menuFlyout.ShowAt((FrameworkElement)sender, new Windows.Foundation.Point(e.GetPosition(this).X, e.GetPosition(this).Y));
        }
        private void AnnouncementsListView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            _currentAnnouncement = (e.OriginalSource as FrameworkElement)?.DataContext as Announcement;
            ((Frame)Window.Current.Content).Navigate(typeof(CommonWebView), _currentAnnouncement);
        }

        private void CopyLink_Click(object sender, RoutedEventArgs e)
        {
            Copy(_currentAnnouncement.Link.ToString());
            _lNotifications = new LNotifications("Link copied successfully!");
            _notificationManager.Show(_lNotifications.GetPositiveNotification());
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
            Copy(_currentAnnouncement.Title + Environment.NewLine + _currentAnnouncement.Description);
            _lNotifications = new LNotifications("Announcement copied successfully!");
            _notificationManager.Show(_lNotifications.GetPositiveNotification(), LocalNotificationCollisionBehaviour.Replace);
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += AnnouncementsEclass_DataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void AnnouncementsEclass_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = _currentAnnouncement.Title;
            args.Request.Data.SetWebLink(_currentAnnouncement.Link);
            args.Request.Data.SetText(_currentAnnouncement.Title + Environment.NewLine + _currentAnnouncement.Description);
        }
        #endregion Buttons
        #region Utilities

        private void AddFlyoutMenu()
        {
            _menuFlyout = new MenuFlyout();
            var share = new MenuFlyoutItem() { Text = "Share", Icon = new SymbolIcon(Symbol.Send) };
            share.Click += Share_Click;
            _menuFlyout.Items?.Add(share);
            var copy = new MenuFlyoutItem() { Text = "Copy", Icon = new SymbolIcon(Symbol.Copy) };
            copy.Click += Copy_Click;
            _menuFlyout.Items?.Add(copy);
            var copyLink = new MenuFlyoutItem() { Text = "Copy Link", Icon = new SymbolIcon(Symbol.Link) };
            copyLink.Click += CopyLink_Click;
            _menuFlyout.Items?.Add(copyLink);
        }
        #endregion Utilities
    }
}
namespace AuebUnofficial
{

    public class CourseViewModel : BindableBase
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public ObservableCollection<Announcement> Announcements { get; set; }
        public int AnnouncementsNumber { get; set; }
        public string LastAnnouncementDate { get; set; }
        public CourseViewModel(Course course)
        {
            Name = course.Name;
            Id = course.ID;
            Announcements = new ObservableCollection<Announcement>(course.ToolViewModel.Tools.OfType<Announcement>());
            foreach (var announcement in Announcements)
            {
                announcement.Description = announcement.Description.DecodeHtml();
            }

            AnnouncementsNumber = Announcements?.Count() ?? 0;
            LastAnnouncementDate = Announcements?.FirstOrDefault()?.DatePublished ?? string.Empty;
        }
    }
}
