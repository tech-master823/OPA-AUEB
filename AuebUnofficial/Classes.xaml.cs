using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.Contacts;
using System.Collections.Generic;
using Syncfusion.UI.Xaml.Controls.Navigation;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial
{

    public sealed partial class Classes : Page
    {
        public Classes()
        {
            this.InitializeComponent();
            LoadPivData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void LoadPivData()
        {
            AddingInfos a = new AddingInfos();
            DataContext = a;
        }

        private void control_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            setItem();

        }
        public void setItem()
        {
            PivotdItem menu = (PivotdItem)control.SelectedItem;
            this.Frame.Navigate(typeof(Viewers.InfoViewers), menu);
        }

    }
}









