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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AuebUnofficial.Viewers
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class SpoudesViewer : Page
    {
        public string oditist;
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
            if (pi.Spudes.Contains("www")){
                // Create an instance of HttpClient
                HttpClient httpClient = new HttpClient();

                // Get the PDF document in byte array
                Byte[] contentBytes = await httpClient.GetByteArrayAsync(pi.Spudes);

                // Load the Byte array
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytes);

                // Display the PDF document in PdfViewer
                pdfViewer.LoadDocument(loadedDocument);
            }else if (pi.Spudes.Contains("Assets"))
            {
                Assembly assembly = typeof(SpoudesViewer).GetTypeInfo().Assembly;
                pdfViewer.DocumentStream = assembly.GetManifestResourceStream(pi.Spudes);
            }
            else if (pi.Spudes == "0")
            {

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
                Frame.GoBack();
            }
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
        public PdfReport(string oditisp)
        {
            //Loads the stream from the embedded resource.
            Assembly assembly = typeof(SpoudesViewer).GetTypeInfo().Assembly;
            docStream = assembly.GetManifestResourceStream(oditisp);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged(this, e);
        }
    }
    
}
