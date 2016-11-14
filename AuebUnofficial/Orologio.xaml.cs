using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WindowsStateTriggers;

namespace AuebUnofficial
{

    public sealed partial class Orologio : Page
    {
        CBoxSource cb=new CBoxSource();
        public Orologio()
        {
           
            this.InitializeComponent();
            addCB();            

        }
        
        public void addCB(){            
            for (int i=0; i<8; i++)
            {                
                ComboboxItem i1=new ComboboxItem(cb.getCBox1(i),i);
                cb1.Items.Add(i1);                             
            }
            for(int i=0; i<4; i++)
            {
                ComboboxItem i2 = new ComboboxItem(cb.getCBox2(i), i);
                cb2.Items.Add(i2);
            }
            
        }


        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ComboboxItem car1 = (ComboboxItem)cb1.SelectedItem;
            ComboboxItem car2 = (ComboboxItem)cb2.SelectedItem;                        
            int x = 4 * (car1.Value) + (car2.Value);
            pdfViewer.GotoPage(cb.getTable(x));
            
        }

        private async void ReportBug(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("mailto:?to=anas.moraitis@gmail.com&subject=BugAtOrologio"));
            mail.Text = "\uE8C3";
        }        
    }
    public class ComboboxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public ComboboxItem(string name,int value)
        {
            Text = name;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
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
            docStream = assembly.GetManifestResourceStream("AuebUnofficial.Assets.progr.pdf");
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged(this, e);
        }
    }    
}