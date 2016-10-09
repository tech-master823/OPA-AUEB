using System;
using System.Collections.ObjectModel;


namespace AuebUnofficial
{
    class SymbolsDataSource : ObservableCollection<Symbols>
    {
        public void Adding()
        {
            
            string[] names = { "Home", "Rss", "Calendar", "Strikes" };
            string[] syms = { "&#xE10F;","&#xE8FD;", "&#xE787;", "&#xEB4D;" };
            for(int i=0; i<names.Length; i++)
            {
                Symbols s = new Symbols();
                s.Name = names[i];
                s.Symbol = syms[i];
                AddSymb(s);
            }
        }
        public void AddSymb(Symbols s)
        {
            this.Add(s);
        }
        public SymbolsDataSource()
        {
            Adding();
        }
    }
    
}
public class Symbols
{
    string symbol;
    public String Symbol
    {
        get { return symbol; }
        set { symbol = value; }
    }
    string name;
    public String Name
    {
        get { return name; }
        set { name = value; }
    }
    public Symbols() { }
}