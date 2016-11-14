using System.Collections.ObjectModel;
using Windows.ApplicationModel;

public class DefPack : ObservableCollection<Packge>
{
    private void fill()
    {

        Package package = Package.Current;
        PackageId packageId = package.Id;
        PackageVersion version = packageId.Version;
        string t = "This is an unofficial app for all windows 10 users-students of Athens Uni of Economics and Business."; ;
        string v = "Version: " + string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        string dev = "Developed by: " + package.PublisherDisplayName;
        Packge pack = new Packge(package.DisplayName,v, "Assets\\Square310x310.png", dev, t);
        this.Add(pack);       
    }
    public DefPack()
    {
        fill();
    }

}
public class Packge
{
    private string name;
    private string version;
    private string imgsrc;
    private string devName;
    private string descriptio;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Version
    {
        get { return version; }
        set { version = value; }
    }
    public string Imgsrc
    {
        get { return imgsrc; }
        set { imgsrc = value; }
    }
    public string DevName
    {
        get { return devName; }
        set { devName = value; }
    }
    public string Descriptio
    {
        get { return descriptio; }
        set { descriptio = value; }
    }
    public Packge() { }
    public Packge(string name,string version, string imgsrc, string devName, string descriptio)
    {
        this.name = name;
        this.version = version;
        this.imgsrc = imgsrc;
        this.devName = devName;
        this.descriptio = descriptio;
    }
}