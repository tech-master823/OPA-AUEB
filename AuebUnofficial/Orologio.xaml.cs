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
using Flurl.Http;
using System.Linq;
using Newtonsoft.Json;
using AuebUnofficial.Model;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Printing;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.Storage.Streams;

namespace AuebUnofficial
{

    public sealed partial class Orologio : Page
    {
        private PrintManager printMan;
        private PrintDocument printDoc;
        private IPrintDocumentSource printDocSource;
        private CBoxSource _cb = new CBoxSource();
        private string x1 = "", x2 = "";
        private int _PdfCurrentPage { get; set; }
        public Stream CurrentPageStream { get; private set; }
        public Windows.UI.Xaml.Controls.Image CurrentPageImage { get; private set; }
        
        public Orologio()
        {
            this.InitializeComponent();
            AddCB();
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
            var response = await client.GetAsync(new Uri("http://amoraitis.me/assets/programma.txt"));
            response.EnsureSuccessStatusCode();
            mystringtext = await response.Content.ReadAsStringAsync();
            //converts string to an array
            List<string> parts = mystringtext.Split('\n').Select(p => p.Trim()).ToList();
            //sets the urls
            x1 = JsonConvert.DeserializeObject<Day2Day>(await "http://auebunofficialapi.azurewebsites.net/Day2Day/Details/fall".GetAsync().ReceiveString()).Link;
            x2 = parts[1];            
            /////////////////////////////////////////////////////////////////////
            // Create an instance of HttpClient
            HttpClient httpClient = new HttpClient();

            // Get the PDF document in byte array
            contentBytesx1 = await httpClient.GetByteArrayAsync(x1);
            contentBytesx2 = await httpClient.GetByteArrayAsync(x2);
            // Load the Byte array
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytesx1);
            // Display the PDF document in PdfViewer
            pdfViewer.LoadDocument(loadedDocument);
            httpClient.Dispose();
            cb1.SelectedIndex = 0;
            cb2.SelectedIndex = 0;
            _PdfCurrentPage = 0;
            CurrentPageStream = await pdfViewer.ExportAsImage(0);
            //secures an exception thrown by clicking switchview && go! btn without the pdf downloaded
            switching.IsEnabled = true;
            go.IsEnabled = true;
            cb1.IsEnabled = true; cb2.IsEnabled = true; CommandBar.Width = pdfViewer.Width - 48;
        }

        

        //This method changes the Header text when necessary and loading the other pdf in the pdfviewer
        //TODO: this method should unload the documents from memory, will be fixed in the future
        private void Change_Frame(object sender, RoutedEventArgs e)
        {

            if (orologio.Text.Equals("Ωρολόγιο Πρόγραμμα"))
            {
                
                orologio.Text = "Πρόγραμμα Εξεταστικής";
                combost.Visibility = Visibility.Collapsed;
                datepick.Visibility = Visibility.Visible;
                cb1.SelectedIndex = 0;
                cb2.SelectedIndex = 0;
                pdfViewer.GotoPage(1);
                // Load the Byte array
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytesx2);

                // Display the PDF document in PdfViewer
                pdfViewer.LoadDocument(loadedDocument);
            }
            else
            {
                //closing
                orologio.Text = "Ωρολόγιο Πρόγραμμα";
                datepick.Visibility = Visibility.Collapsed;
                combost.Visibility = Visibility.Visible;
                exetastiki.Date = null;
                pdfViewer.GotoPage(1);
                // Load the Byte array
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(contentBytesx1);

                // Display the PDF document in PdfViewer
                pdfViewer.LoadDocument(loadedDocument);
                loadedDocument.Dispose();
            }
        }

        //This method adds items in the combobox pasing them through an "array" from CBoxSource class
        public void AddCB() {
            for (int i = 0; i < 8; i++)
            {
                ComboboxItem i1 = new ComboboxItem(_cb.getCBox1(i), i);
                cb1.Items.Add(i1);
            }
            for (int i = 0; i < 4; i++)
            {
                ComboboxItem i2 = new ComboboxItem(_cb.getCBox2(i), i);
                cb2.Items.Add(i2);
            }

        }

        //This method gets the ComboBoxItem selected from the user and changes the page in the PdfViewer
        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            int x = -1;
            ComboboxItem car1 = (ComboboxItem)cb1.SelectedItem;
            ComboboxItem car2 = (ComboboxItem)cb2.SelectedItem;
            x = 4 * (car1.Value) + (car2.Value);
            if (pdfViewer.SearchText("ΑΝΑΚΟΙΝΩΣΗ"))
            {
                pdfViewer.GotoPage(_cb.getTable(x, true));
            }
            
            
            pdfViewer.GotoPage(_cb.getTable(x,false));

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

        private void pdfViewer_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            
        }


        private async void pdfViewer_PageChanged(object sender, Syncfusion.Windows.PdfViewer.PageChangedEventArgs e)
        {
            _PdfCurrentPage = e.NewPageNumber;
            if (_PdfCurrentPage == 0)
            {

            }
            else
            {
                CurrentPageStream = await pdfViewer.ExportAsImage(_PdfCurrentPage - 1);
                CurrentPageImage = pdfViewer.GetPage(_PdfCurrentPage - 1);
            }
            
        }

        //Adding Data of type "DatationType" in array data----adding dates in the table
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
        private async void ExecuteCopyCommand(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            CopyBitmap(await SaveImageAsync(ApplicationData.Current.TemporaryFolder));
        }
        private void CopyBitmap(StorageFile imageFile)
        {
            
                var dataPackage = new DataPackage();                
                dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
                try
                {
                    Clipboard.SetContent(dataPackage);
                }
                catch (Exception ex)
                {
                    // Copying data to Clipboard can potentially fail - for example, if another application is holding Clipboard open
                }
        }
        private void ExecuteShareCommand(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

            DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            DataTransferManager.ShowShareUI();
        }
        private async void MainPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = "asdada";
            DataRequestDeferral deferral = args.Request.GetDeferral();
            try
            {
                StorageFile storageFile = await SaveImageAsync(ApplicationData.Current.LocalCacheFolder);
                List<IStorageItem> list = new List<IStorageItem>();
                list.Add(storageFile);
                args.Request.Data.SetStorageItems(list);
            }
            finally
            {
                deferral.Complete();
            }
            
            
        }
        private async void ExecuteSaveCommand(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await SaveImageAsync(KnownFolders.PicturesLibrary);
        }
        async Task<StorageFile> SaveImageAsync(StorageFolder storageFolder)
        {
            var fileName = "Program.png";
            var folder = storageFolder;
            var file = await folder.TryGetItemAsync(fileName);
            
            var storageFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(storageFile, ((MemoryStream)CurrentPageStream).GetWindowsRuntimeBuffer().ToArray());
            return storageFile;
        }
        private void CopyLink_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            
            Copy(x1);            
            
            
        }
        private void Copy(string text)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            if (text.Contains("http")) dataPackage.SetWebLink(new Uri(text));
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }

        private void Print_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            pdfViewer.Print();
            
        }

        private async void PrintPage_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Register for PrintTaskRequested event
            printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;

            // Build a PrintDocument and register for callbacks
            printDoc = new PrintDocument();
            printDocSource = printDoc.DocumentSource;
            printDoc.Paginate += Paginate;
            printDoc.GetPreviewPage += GetPreviewPage;
            printDoc.AddPages += AddPages;

            if (PrintManager.IsSupported())
            {
                try
                {
                    // Show print UI
                    await PrintManager.ShowPrintUIAsync();
                }
                catch
                {
                    // Printing cannot proceed at this time
                    ContentDialog noPrintingDialog = new ContentDialog()
                    {
                        Title = "Printing error",
                        Content = "\nSorry, printing can' t proceed at this time.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                }
            }
            else
            {
                // Printing is not supported on this device
                ContentDialog noPrintingDialog = new ContentDialog()
                {
                    Title = "Printing not supported",
                    Content = "\nSorry, printing is not supported on this device.",
                    PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            }
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            // Create the PrintTask.
            // Defines the title and delegate for PrintTaskSourceRequested
            var printTask = args.Request.CreatePrintTask("Print", PrintTaskSourceRequrested);

            // Handle PrintTask.Completed to catch failed print jobs
            printTask.Completed += PrintTaskCompleted;
        }

        private void PrintTaskSourceRequrested(PrintTaskSourceRequestedArgs args)
        {
            // Set the document source.
            args.SetSource(printDocSource);
        }

        private void Paginate(object sender, PaginateEventArgs e)
        {
            // As I only want to print one Rectangle, so I set the count to 1
            printDoc.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            // Provide a UIElement as the print preview.
            printDoc.SetPreviewPage(e.PageNumber, CurrentPageImage);
        }

        private void AddPages(object sender, AddPagesEventArgs e)
        {
            printDoc.AddPage(CurrentPageImage);

            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        private async void PrintTaskCompleted(PrintTask sender, PrintTaskCompletedEventArgs args)
        {
            // Notify the user when the print operation fails.
            if (args.Completion == PrintTaskCompletion.Failed)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    ContentDialog noPrintingDialog = new ContentDialog()
                    {
                        Title = "Printing error",
                        Content = "\nSorry, failed to print.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                });
            }
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
            
        }
    }
    //////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////
}