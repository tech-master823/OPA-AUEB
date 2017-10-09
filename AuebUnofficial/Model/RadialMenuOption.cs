using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AuebUnofficial.Model
{
    public class RadialMenuOption
    {
        public string Name { get; set; }
        public Symbol Symbol { get; set; }
        public ICommand Command { get; set; }
    }
}
