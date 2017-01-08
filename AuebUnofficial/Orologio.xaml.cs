using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Linq;

namespace AuebUnofficial
{

    public sealed partial class Orologio : Page
    {
        CBoxSource cb = new CBoxSource();
        string x1 = "", x2 = "";
        public Orologio()
        {
            this.InitializeComponent();
            addCB();
            addDates();
        }
        List<DatationType> dates = new List<DatationType>();
        // Create an instance of HttpClient
        HttpClient httpClient = new HttpClient();

        // Get the PDF document in byte array
        Byte[] contentBytesx1;
        Byte[] contentBytesx2;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string mystringtext = "";
            base.OnNavigatedTo(e);
            exetastiki.MinDate = new DateTime(2017, 1, 16);
            exetastiki.MaxDate = new DateTime(2017, 2, 09);
            ///////////////////////////////////////////////////////////////////////
            //gettingResponseFromTxtAsString
            var handler = new HttpClientHandler { AllowAutoRedirect = true };
            var client = new HttpClient(handler);
            var response = await client.GetAsync(new Uri("https://amoraitis.github.io/Portfolio/assets/programma.txt"));
            response.EnsureSuccessStatusCode();
            mystringtext = await response.Content.ReadAsStringAsync();
            //converts string to an array
            List<string> parts = mystringtext.Split('\n').Select(p => p.Trim()).ToList();
            //sets the url
            x1 = parts[0];
            x2 = parts[1];            
            /////////////////////////////////////////////////////////////////////
            // Create an instance of HttpClient
            HttpClient httpClient = new HttpClient();

            // Get the PDF document in byte array
            contentBytesx1 = await httpClient.GetByteArrayAsync(x1);
            contentBytesx2 = await httpClient.GetByteArrayAsync(x2);
            //secures an exception thrown by clicking switchview without the pdf downloaded
            switching.IsHitTestVisible = true;
            // Load the Byte array
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytesx1);

            // Display the PDF document in PdfViewer
            pdfViewer.LoadDocument(loadedDocument);
        }
        //This method changes the Header text when necessary and loading the other pdf in the pdfviewer
        //this method should unload the documents from memory, will be fixed in the future
        private void Change_Frame(object sender, RoutedEventArgs e)
        {

            if (orologio.Text.Equals("Ωρολόγιο Πρόγραμμα"))
            {
                orologio.Text = "Πρόγραμμα Εξεταστικής";
                combost.Visibility = Visibility.Collapsed;
                datepick.Visibility = Visibility.Visible;
                cb1.SelectedItem = null;
                cb2.SelectedItem = null;
                pdfViewer.GotoPage(1);
                // Load the Byte array
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytesx2);

                // Display the PDF document in PdfViewer
                pdfViewer.LoadDocument(loadedDocument);
            }
            else
            {
                orologio.Text = "Ωρολόγιο Πρόγραμμα";
                datepick.Visibility = Visibility.Collapsed;
                combost.Visibility = Visibility.Visible;
                exetastiki.Date = null;
                pdfViewer.GotoPage(1);
                // Load the Byte array
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytesx1);

                // Display the PDF document in PdfViewer
                pdfViewer.LoadDocument(loadedDocument);
            }
        }

        //This method adds items in the combobox pasing them through an "array" from CBoxSource class
        public void addCB() {
            for (int i = 0; i < 8; i++)
            {
                ComboboxItem i1 = new ComboboxItem(cb.getCBox1(i), i);
                cb1.Items.Add(i1);
            }
            for (int i = 0; i < 4; i++)
            {
                ComboboxItem i2 = new ComboboxItem(cb.getCBox2(i), i);
                cb2.Items.Add(i2);
            }

        }

        //This method gets the ComboBoxItem selected from the user and changes the page in the PdfViewer
        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ComboboxItem car1 = (ComboboxItem)cb1.SelectedItem;
            ComboboxItem car2 = (ComboboxItem)cb2.SelectedItem;
            int x = 4 * (car1.Value) + (car2.Value);
            pdfViewer.GotoPage(cb.getTable(x));

        }

        //This method gets the Date selected from user and going to a page based on the selection
        private void exetastiki_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            foreach (DatationType dateit in dates)
            {
                if (exetastiki.Date == dateit.Dt)
                    pdfViewer.GotoPage(dateit.Pa);
            }
        }


        private async void ReportBug(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:?to=anas.moraitis@gmail.com&subject=BugAtOrologio"));
            mail.Text = "\uE8C3";
        }
        //Adding Data of type "DatationType" in array data----adding fates in the table
        private void addDates()
        {
            dates.Insert(0, new DatationType(new DateTime(2017, 1, 16), 1));
            dates.Insert(1, new DatationType(new DateTime(2017, 1, 17), 2));
            dates.Insert(2, new DatationType(new DateTime(2017, 1, 18), 3));
            dates.Insert(3, new DatationType(new DateTime(2017, 1, 19), 4));
            dates.Insert(4, new DatationType(new DateTime(2017, 1, 20), 5));
            dates.Insert(5, new DatationType(new DateTime(2017, 1, 23), 6));
            dates.Insert(6, new DatationType(new DateTime(2017, 1, 24), 7));
            dates.Insert(7, new DatationType(new DateTime(2017, 1, 25), 8));
            dates.Insert(8, new DatationType(new DateTime(2017, 1, 26), 9));
            dates.Insert(9, new DatationType(new DateTime(2017, 1, 27), 10));
            dates.Insert(10, new DatationType(new DateTime(2017, 1, 31), 11));
            dates.Insert(11, new DatationType(new DateTime(2017, 2, 1), 12));
            dates.Insert(12, new DatationType(new DateTime(2017, 2, 2), 13));
            dates.Insert(13, new DatationType(new DateTime(2017, 2, 3), 14));
            dates.Insert(14, new DatationType(new DateTime(2017, 2, 6), 15));
            dates.Insert(15, new DatationType(new DateTime(2017, 2, 7), 16));
            dates.Insert(16, new DatationType(new DateTime(2017, 2, 8), 17));
            dates.Insert(17, new DatationType(new DateTime(2017, 2, 9), 18));
        }
    }   

    //This type represents a Date and contains the DateType and the page (in the pdf) 
    class DatationType
    {
        //Dt for datetime in the calendar
        public DateTime Dt { get; set; }

        //Pa for page int pdf
        public int Pa { get; set; }
        
        public DatationType(DateTime dt, int pa)
        {
            this.Dt = dt;
            this.Pa = pa;
        }
    }
    
    //This type represents a ComboBox Item with each values
    public class ComboboxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public ComboboxItem(string name, int value)
        {
            Text = name;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }


    //////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////
    //This section copied from Syncfusion pdfviewer documentation and used to control the pdfviewer control
    //////////////////////////////////////////////////////////
    class PdfReport : INotifyPropertyChanged
    {
        private Stream docStream;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Stream object to be bound to the ItemsSource of the PDF Viewer
        /// </summary>
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
            Assembly assembly = typeof(Orologio).GetTypeInfo().Assembly;
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged(this, e);
        }
    }
    //////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////
}