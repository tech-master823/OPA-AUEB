using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AuebUnofficial
{
    public sealed partial class Navig : UserControl
    {
        public Navig()
        {
            this.InitializeComponent();
            hamburgerMenuControl.ItemsSource = MenuItem.GetMainItems();
            hamburgerMenuControl.OptionsItemsSource = MenuItem.GetOptionsItems();
            
        }
        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            ((Frame)Window.Current.Content).Navigate(menuItem.PageType);       
        }
        
    }
    public class MenuItem
    {
        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }

        public static List<MenuItem> GetMainItems()
        {
            SymbolIcon train = new SymbolIcon();
            train.Symbol = (Symbol)0xE7C0;
            
            var items = new List<MenuItem>();
            items.Add(new MenuItem() { Icon = Symbol.Home, Name = "Home", PageType = typeof(MainPage) });
            items.Add(new MenuItem() { Icon = Symbol.Bullets, Name = "Ανακοινώσεις", PageType = typeof(RssViewer) });
            items.Add(new MenuItem() { Icon = train.Symbol, Name = "Απεργίες", PageType = typeof(Strikes) });
            items.Add(new MenuItem() { Icon = Symbol.Calendar, Name = "Ωρολογιο", PageType = typeof(Orologio) });           
            items.Add(new MenuItem() { Icon = Symbol.Accept, Name = "Πληροφοριες", PageType = typeof(Classes) });
            items.Add(new MenuItem() { Icon = Symbol.Map, Name = "Map", PageType = typeof(MappingMySchool) });
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
            SymbolIcon about = new SymbolIcon();
            about.Symbol = (Symbol)0xE946;
            var items = new List<MenuItem>();
            items.Add(new MenuItem() { Icon = about.Symbol, Name = "About", PageType = typeof(About) });
            return items;
        }
    }
}
