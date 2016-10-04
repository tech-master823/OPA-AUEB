using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AuebUnofficial
{
    public sealed partial class Navig : UserControl
    {
        public Navig()
        {
            this.InitializeComponent();
        }
        private void ShowSliptView(object sender, RoutedEventArgs e)
        {
            SamplesSplitView.IsPaneOpen = !SamplesSplitView.IsPaneOpen;
        }
        private void NavigateToOptimizedGrid(object sender, RoutedEventArgs e)
        {
           ((Frame)Window.Current.Content).Navigate(typeof(RssViewer));
        }

        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
        }
        private void NavigateToApergies(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Strikes));
        }

        private void NavigateToProgram(object sender, RoutedEventArgs e)
        {
            //((Frame)Window.Current.Content).Navigate(typeof(Programpf));

        }
    }
}
