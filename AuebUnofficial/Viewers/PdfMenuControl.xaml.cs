using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Windows.Graphics.Display;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AuebUnofficial.Viewers
{
    public sealed partial class PdfMenuControl : UserControl
    {
        List<IStorageItem> list = new List<IStorageItem>();
        private string SaveName { get; set; }
        public PdfMenuControl()
        {
            this.InitializeComponent();
            this.Loaded += PdfMenuControl_Loaded;
        }
        private void PdfMenuControl_Loaded(object sender, RoutedEventArgs e)
        {
            SaveName = "Save Page";
        }
        private void ExecuteCopyCommand(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            var page = ((Orologio)((Frame)Window.Current.Content).Content).CurrentPageStream;
            Debug.WriteLine("Copy button pressed!");
            
        }
        private void ExecuteShareCommand(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            DataTransferManager.GetForCurrentView().DataRequested += MainPage_DataRequested;
            DataTransferManager.ShowShareUI();
        }
        private void MainPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            
            args.Request.Data.Properties.Title = "asdada";
            //args.Request.Data.SetStorageItems(list);
        }
        private void ExecuteSaveCommand(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var page = ((Orologio)((Frame)Window.Current.Content).Content).CurrentPageImage;
            Debug.WriteLine("Save button pressed!");
        }
        private void ExecuteCancelCommand(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Cancel button pressed!");
        }


        
    }
}
