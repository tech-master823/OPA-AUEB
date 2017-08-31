using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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


namespace AuebUnofficial.Viewers
{
    
    public sealed partial class SpoudesViewer : Page
    {
        PivotdItem pi;
        public SpoudesViewer()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            pi = (PivotdItem)e.Parameter;
            Title.Text = pi.Header; 
            if (pi.Spudes.StartsWith("http")){
                // Create an instance of HttpClient
                HttpClient httpClient = new HttpClient();

                // Get the PDF document in byte array
                Byte[] contentBytes = await httpClient.GetByteArrayAsync(pi.Spudes);

                // Load the Byte array
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytes);

                // Display the PDF document in PdfViewer
                pdfViewer.LoadDocument(loadedDocument);
                pdfViewer.Visibility = Visibility.Visible;
            }
            else if (pi.Spudes.StartsWith("AuebUnofficial"))
            {
                Stream s=this.GetType().GetTypeInfo().Assembly.GetManifestResourceStream(pi.Spudes);
                pdfViewer.LoadDocument(s);
                pdfViewer.Visibility = Visibility.Visible;
            }
            else if (pi.Spudes == "0")
            {
                pdfViewer.IsThumbnailViewEnabled = false;
                pdfViewer.PdfProgressRing.Visibility = Visibility.Collapsed;
                zero.Visibility = Visibility.Visible;
            }
           
        }
        private void Spudes_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(Viewers.SpoudesViewer), pi);
        }
        private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {            
            if (Frame.CanGoBack)
            {
                pdfViewer.Unload();
                Frame.GoBack();
            }
        }

        private async void Maila_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:?to="+pi.Mail+"&subject=Οδηγός Σπουδών"));
        }
    }



    class PdfReport : INotifyPropertyChanged
    {
        private Stream docStream;
        public event PropertyChangedEventHandler PropertyChanged;
        public Stream DocumentStream
        {
            get
            {
                return docStream;
            }
            set
            {
                docStream = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DocumentStream"));
            }
        }

        public PdfReport()
        {
            //Loads the stream from the embedded resource.
            Assembly assembly = typeof(SpoudesViewer).GetTypeInfo().Assembly;
            
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged(this, e);
        }
    }
    
}
