using AuebUnofficial.Viewers;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace AuebUnofficial
{
    /**
     * TODO: Some recources are not shown, check back button triger in some pages should show the backbutton 
     */
    public sealed partial class Navig : Page
    {
        App myapp = App.Current as App;
        private DataTransferManager dataTransferManager;
        public Navig()
        {
            this.InitializeComponent();
            // TODO: use footer nav
            hamburgerMenuControl.MenuItemsSource = MenuItem.GetAllItems();
            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private void hamburgerMenuControl_Loaded(object sender, RoutedEventArgs e)
        {
            hamburgerMenuControl.IsPaneOpen = false;
            hamburgerMenuControl.IsBackButtonVisible = muxc.NavigationViewBackButtonVisible.Collapsed;
            ContentFrame.Navigated += On_Navigated;
            // Add handler for ContentFrame navigation.
            ContentFrame.Navigate(MenuItem.GetMainItems().First().PageType);
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            hamburgerMenuControl.IsBackEnabled = ContentFrame.CanGoBack;
            hamburgerMenuControl.IsBackButtonVisible = muxc.NavigationViewBackButtonVisible.Visible;
            if (ContentFrame.SourcePageType != null || ContentFrame.SourcePageType != typeof(ReviewOnStore))
            {
                var item = MenuItem.GetAllItems().FirstOrDefault(p => p.PageType == e.SourcePageType);
            }
        }

        private void hamburgerMenuControl_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            NavigationItemClicked(
                            args.InvokedItem as MenuItem, args.RecommendedNavigationTransitionInfo);
        }

        // Footer item clicked, currently not used
        private void NavFooterList_ItemClick(object sender, ItemClickEventArgs e)
        {
            NavigationItemClicked(
                e.ClickedItem as MenuItem, null);
        }

        private async void NavigationItemClicked(MenuItem menuItem, NavigationTransitionInfo transitionInfo)
        {

            if (menuItem.PageType == typeof(ReviewOnStore))
            {
                ReviewOnStore review = new ReviewOnStore();
                await review.ShowAsync();

            }
            else if (menuItem.PageType == null) //Check null because of 'share' item
            {
                DataTransferManager.ShowShareUI();
            }
            else
            {
                // Get the page type before navigation so you can prevent duplicate
                // entries in the backstack.
                var preNavPageType = ContentFrame.CurrentSourcePageType;
                // Only navigate if the selected page isn't currently loaded.
                if (!(menuItem.PageType is null) && !Type.Equals(preNavPageType, menuItem.PageType))
                {
                    if (transitionInfo != null)
                        ContentFrame.Navigate(menuItem.PageType, null, transitionInfo);
                    else
                        ContentFrame.Navigate(menuItem.PageType);
                }
                ContentFrame.Navigate(menuItem.PageType, ContentFrame);
            }
        }
        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;

            request.Data.Properties.Title = "Share the App";
            request.Data.Properties.Description = "This App in Windows Store";
            request.Data.SetWebLink(new Uri("https://www.microsoft.com/p/ΟΠΑ-aueb/9nblggh5384p"));
        }

        private void hamburgerMenuControl_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
            On_BackRequested();
        }
        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            //// Don't go back if the nav pane is overlayed.
            //if (hamburgerMenuControl.IsPaneOpen &&
            //    (hamburgerMenuControl.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
            //     hamburgerMenuControl.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
            //    return false;

            ContentFrame.GoBack();
            return true;
        }
    }
    public class MenuItem
    {
        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }

        public static List<MenuItem> GetMainItems()
        {
            SymbolIcon train = new SymbolIcon
            {
                Symbol = (Symbol)0xE7C0
            };
            SymbolIcon login = new SymbolIcon
            {
                Symbol = (Symbol)0xE8D4
            };
            var items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Home, Name = "Home", PageType = typeof(MainPage) },
                new MenuItem() { Icon = Symbol.Bullets, Name = "Ανακοινώσεις", PageType = typeof(RssViewer) },
                new MenuItem() { Icon = train.Symbol, Name = "Μ.Μ.Μ.", PageType = typeof(Strikes) },
                new MenuItem() { Icon = Symbol.Calendar, Name = "Ωρολόγιο", PageType = typeof(Orologio) },
                new MenuItem() { Icon = Symbol.ContactInfo, Name = "Πληροφορίες τμημάτων", PageType = typeof(Classes) },
                //items.Add(new MenuItem() { Icon = Symbol.Map, Name = "Map", PageType = typeof(MappingMySchool) });
                new MenuItem() { Icon = login.Symbol, Name = "Eclass", PageType = typeof(Viewers.Eclass.EclassNat) }
            };
            return items;
        }
        //public static List<MenuItem> GetSubClassItems()
        //{
        //    SymbolIcon empty = new SymbolIcon();
        //    var subClassItems = new List<MenuItem>();
        //    subClassItems.Add(new MenuItem() {Icon= empty.Symbol, Name="DET", PageType=typeof(Classes) });

        //    return subClassItems;
        //}
        public static List<MenuItem> GetOptionsItems()
        {
            SymbolIcon about = new SymbolIcon
            {
                Symbol = (Symbol)0xE946
            };
            var items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Like, Name = "Review", PageType = typeof(ReviewOnStore) },
                new MenuItem() { Icon = about.Symbol, Name = "About", PageType = typeof(About) }
            };
            return items;
        }

        public static List<MenuItem> GetAllItems()
        {
            var items = new List<MenuItem>();
            items.AddRange(GetMainItems());
            items.Add(new MenuItem { Icon = Symbol.Share, PageType = null });
            items.AddRange(GetOptionsItems());
            return items;
        }
    }

    [ContentProperty(Name = "ItemTemplate")]
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ViewModelTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            return item is MenuItem
            ? ViewModelTemplate
            : ItemTemplate;
        }
    }

}