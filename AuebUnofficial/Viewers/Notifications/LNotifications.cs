using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using RavinduL.LocalNotifications.Notifications;
using Windows.UI.Popups;

namespace AuebUnofficial.Viewers.Notifications
{
    public class LNotifications
    {
        private SimpleNotification positiveNotification, negativeNotification;
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
            
            negativeNotification = new SimpleNotification
            {
                 TransitionDuration = TimeSpan.FromSeconds(1),
                 Text = NegativeText,
                 Action = async () => await new MessageDialog(NegativeText).ShowAsync(),
                Background = new SolidColorBrush(Colors.DarkRed),
                Foreground = new SolidColorBrush(Colors.White)
            };
        }

        private void SetPositiveNotification()
        {

            positiveNotification = new SimpleNotification()
            {
                TransitionDuration = TimeSpan.FromSeconds(1),
                 Text = PositiveText,
                 Action = async () => await new MessageDialog(PositiveText).ShowAsync(),          
                Background = new SolidColorBrush(Colors.DarkGreen),
                Foreground = new SolidColorBrush(Colors.White)
            };
        }
        public SimpleNotification GetPositiveNotification()
        {
            return this.positiveNotification;
        }
        public SimpleNotification GetNegativeNotification()
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