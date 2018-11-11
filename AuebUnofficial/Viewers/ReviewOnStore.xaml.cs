using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Fake dialog to review the app.

namespace AuebUnofficial.Viewers
{
    public sealed partial class ReviewOnStore : ContentDialog
    {
        public ReviewOnStore()
        {
            this.InitializeComponent();
            this.Loaded += ReviewOnStore_Loaded;
        }

        private async void ReviewOnStore_Loaded(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store://review/?ProductId={0}", Windows.ApplicationModel.Package.Current.Id.FamilyName)));
            Hide();
        }
    }
}
