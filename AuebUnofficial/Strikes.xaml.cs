using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial
{
    public sealed partial class Strikes : Page
    {
        
        public Strikes()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mmm.Text.Equals("Απεργίες"))
            {
                mmm.Text = "Μ.Μ.Μ.";
                strikes.Source = new System.Uri("http://amoraitis.me/auebUnof.html");
            }
            else                 
            {
                mmm.Text = "Απεργίες";
                strikes.Source = new System.Uri("http://www.apergia.gr/q/");
            }
            
        }      
    }
}
