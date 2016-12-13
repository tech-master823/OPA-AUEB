using Flurl.Http;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AuebUnofficial.Viewers
{
    public sealed partial class AnouncementsEclass : Page
    {
        ObservableCollection<Course> mycourses;
        public AnouncementsEclass()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            anoun.Text = await "https://eclass.aueb.gr/modules/mobile/mcourses.php"
               .PostUrlEncodedAsync(new { token = (string)e.Parameter })
               .ReceiveString();

            var getit = anoun.Text;
            XDocument coursex = XDocument.Load(GenerateStreamFromString(getit));
            var ycourses = coursex.Root
                  .Elements("course")
                  .Select(x => new Course
                  {
                      Id = (string)x.Attribute("code"),
                      Name = (string)x.Attribute("title")

                  })
                  .ToArray();
            foreach (Course course in ycourses)
            {
                mycourses.Add(course);
            }
            ListView1.ItemsSource = mycourses;
            
        }

        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
    class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
