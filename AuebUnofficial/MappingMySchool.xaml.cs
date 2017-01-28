using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial
{
    public sealed partial class MappingMySchool : Page
    {
        public MappingMySchool()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Specify a known location.
            BasicGeoposition auebPosition = new BasicGeoposition() { Latitude = 37.994083, Longitude = 23.732421 };
            Geopoint auebCenter = new Geopoint(auebPosition);

            //// Set the map location.
            AuebMap.Center = auebCenter;
            AuebMap.ZoomLevel = 15.5;
            AuebMap.LandmarksVisible = true;
            displayPOI();
        }

        public void displayPOI()
        {             
            Poi myPoi = new Poi(new BasicGeoposition() { Latitude= 37.995912, Longitude= 23.739618 },
                "Κέντρο Μεταπτυχιακών Σπουδών και Έρευνας");
            AuebMap.MapElements.Add(myPoi.getMI());
            myPoi = new Poi(new BasicGeoposition() { Latitude = 37.999265, Longitude = 23.732774 }, "ΕΛΚΕ & ACEin");
            AuebMap.MapElements.Add(myPoi.getMI());
            myPoi = new Poi(new BasicGeoposition() { Latitude = 37.995609, Longitude = 23.733104 }, "Κτήριο επί της οδού Κοδριγκτώνος");
            AuebMap.MapElements.Add(myPoi.getMI());
            myPoi = new Poi(new BasicGeoposition() { Latitude = 37.996121, Longitude = 23.737238 }, "Κτήριο επί της οδού Ύδρας");
            AuebMap.MapElements.Add(myPoi.getMI());
            myPoi = new Poi(new BasicGeoposition() { Latitude = 37.994716, Longitude = 23.732289 }, "Κτήριο επί της οδού Δεριγνύ");
            AuebMap.MapElements.Add(myPoi.getMI());
        }
    }
    public class Poi {
        BasicGeoposition SnPosition { set; get; }
        string Title { get; set; }
        public Poi(BasicGeoposition snPotision, string title)
        {
            SnPosition = snPotision;
            Title = title;                
            mapicon.Location = new Geopoint(SnPosition);
            mapicon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapicon.Title = Title;
            mapicon.ZIndex = 0;
        }
        MapIcon mapicon = new MapIcon();
        public MapIcon getMI()
        {
            return this.mapicon; 
        }
    }
}