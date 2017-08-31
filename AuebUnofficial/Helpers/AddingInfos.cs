using System.Collections.ObjectModel;

public class AddingInfos
{
    string[] heders = { "Τμήμα Διεθνών και Ευρωπαϊκών Οικονομικών Σπουδών", "Τμήμα Οικονομικής Επιστήμης", "Tμήμα Διοικητικής Επιστήμης & Τεχνολογίας", "Τμήμα Οργάνωσης και Διοίκησης Επιχειρήσεων", "Τμήμα Λογιστικής & Χρηματοοικονομικής", "Τμήμα Μάρκετινγκ & Επικοινωνίας", "Τμήμα Πληροφορικής", "Τμήμα Στατιστικής" };
    int[,] numbers = new int[,] { { 2108203106, 2108203107, 2108203108 }, { 2108203303, 2108203302, 2108203301 }, 
        { 2108203110, 2108203129,0}, 
        {2108203308 , 2118203309,2128203310 }, 
        { 2108203300, 2108203303, 2108203322}, 
        { 2108203101, 2108203102,2108203103 }, 
        {2108203314, 2108203315,2108203316},
        { 2108203111,2108203112 ,2108203113 } };
    string[] mails = new string[] { "deossecr@aueb.gr", "econ@aueb.gr","dmst@aueb.gr","ode@aueb.gr","accfin@aueb.gr","secretary.marketing@aueb.gr","infotech@aueb.gr","prosec@aueb.gr" };
    string[] spoydes = new string[] { "0","0", "http://195.251.255.130/sites/default/files/dmst/OdSpoud_2015-2016_GR.pdf", "https://www.dept.aueb.gr/odigoi/PROGRAM_2014-15.pdf", "AuebUnofficial.Assets.LoxriOdhgosSp.pdf", "https://www.dept.aueb.gr/sites/default/files/mbc/Odigos_Spoudon_2016_17_IAN2017.pdf", "https://www.dept.aueb.gr/sites/default/files/cs/CS_Manuals/CS_StudyGuide2016-17.pdf", "AuebUnofficial.Assets.StatOdhgSpoudwn.pdf" };

    private ObservableCollection<PivotdItem> items;
    public ObservableCollection<PivotdItem> TabControlItems
    {
        get { return items; }
        set { items = value; }
    }

    public AddingInfos()
    {
        TabControlItems = new ObservableCollection<PivotdItem>();
        setP();

        
    }    
    private void setP()
    {
        PivotdItem pivot;
        for(int i=0; i<8; i++)
        {
            pivot = new PivotdItem(heders[i]);
            pivot.Mail = mails[i];
            pivot.P1 = numbers[i, 0]; pivot.P2 = numbers[i, 1]; pivot.P3 = numbers[i, 2];
            pivot.Spudes = spoydes[i];
            TabControlItems.Add(pivot);            
        }
    }
}

 public class PivotdItem
{
    public int p1;
    public int P1 { get; set; }
    public int p2;
    public int P2 { get; set; }
    public int p3;
    public int P3 { get; set; }
    public string spudes;

    public string Spudes { get; set; }
    public string Mail { get; set; }
    public string Header { get; set; }  
    public PivotdItem(string h)
    {
        Header = h;        
    }
    public PivotdItem()
    {
    }
}


