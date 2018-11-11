using AuebUnofficial.Viewers;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Navig()
        {
            this.InitializeComponent();
            hamburgerMenuControl.MenuItemsSource = MenuItem.GetMainItems();
            NavFooterList.ItemsSource = MenuItem.GetOptionsItems();
            // Add handler for ContentFrame navigation.
            ContentFrame.Navigate(MenuItem.GetMainItems().First().PageType);
        }

        private void hamburgerMenuControl_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            NavigationItemClicked(
                            args.InvokedItem as MenuItem);
        }


        private void NavFooterList_ItemClick(object sender, ItemClickEventArgs e)
        {
            NavigationItemClicked(
                e.ClickedItem as MenuItem);
        }

        private async void NavigationItemClicked(MenuItem menuItem)
        {
            if (menuItem.PageType == typeof(ReviewOnStore))
            {
                ReviewOnStore review = new ReviewOnStore();
                await review.ShowAsync();

            }
            else
            {
                ContentFrame.Navigate(menuItem.PageType, ContentFrame);
            }
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
                new MenuItem() { Icon = login.Symbol, Name = "Eclass", PageType = typeof(Viewers.eclass_Nat) }
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