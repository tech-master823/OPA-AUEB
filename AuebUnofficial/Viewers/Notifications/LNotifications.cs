using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using RavinduL.LocalNotifications.Presenters;
using Windows.UI.Popups;

namespace AuebUnofficial.Viewers.Notifications
{
    public class LNotifications
    {
        private SimpleNotificationPresenter positiveNotification, negativeNotification;
        public string PositiveText { get; set; }
        public string NegativeText { get; set; }
        

        public LNotifications(string positive, string negative)
        {
            PositiveText = positive;
            NegativeText = negative;
            SetPositiveNotification();
            SetNegativeNotification();
        }
        public LNotifications(string positive)
        {
            PositiveText = positive;
            SetPositiveNotification();
   
        }

        private void SetNegativeNotification()
        {
            
            negativeNotification = new SimpleNotificationPresenter
             (
                 TimeSpan.FromSeconds(3),
                 text: NegativeText,
                 action: async () => await new MessageDialog(NegativeText).ShowAsync()
             )
            {
                Background = new SolidColorBrush(Colors.DarkRed),
                Foreground = new SolidColorBrush(Colors.White)
            };
        }

        private void SetPositiveNotification()
        {

            positiveNotification = new SimpleNotificationPresenter
             (
                 TimeSpan.FromSeconds(3),
                 text: PositiveText,
                 action: async () => await new MessageDialog(PositiveText).ShowAsync()
             )
            {
                Background = new SolidColorBrush(Colors.DarkGreen),
                Foreground = new SolidColorBrush(Colors.White)
            };
        }
        public SimpleNotificationPresenter GetPositiveNotification()
        {
            return this.positiveNotification;
        }
        public SimpleNotificationPresenter GetNegativeNotification()
        {
            return this.negativeNotification;
        }
        public SolidColorBrush GetSuccesColor()
        {
            return new SolidColorBrush(Colors.DarkGreen);
        }
        public SolidColorBrush GetNegativeColor()
        {
            return new SolidColorBrush(new Color()
            {
                A = 0,
                B = 79,
                G = 83,
                R = 217
            });
        }

    }
}