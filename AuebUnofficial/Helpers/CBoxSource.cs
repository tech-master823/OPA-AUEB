using System;
using System.Collections.Generic;

public class CBoxSource
{
    string[] cbox1 = new string[8];
    string[] cbox2 = new string[4];
    public List<int> go = new List<int> {1,2,3,4,6,7,8,9,11,12,13,14,16,17,18,22,26,27,28,30,33,34,35,36,39,40,
        41,42,44,45,46,47};
    public List<int> goWithAnnouncement = new List<int>();
    public int PageToGo { get; set; }
    public void addCBox1()
    {        
        cbox1[0] = "ΔΕΟΣ";
        cbox1[1] = "ΟΙΚ";
        cbox1[2] = "ΔΕΤ";
        cbox1[3] = "ΟΔΕ";
        cbox1[4] = "ΛΟΧΡΗ";
        cbox1[5] = "Μ&Ε";
        cbox1[6] = "ΠΛΗΡ";
        cbox1[7] = "ΣΤΑΤ";
    }
    public string getCBox1(int i)
    {
        return cbox1[i];
    }
    public void addCBox2()
    {       
        cbox2[0] = "1ο";
        cbox2[1] = "2ο";
        cbox2[2] = "3ο";
        cbox2[3] = "4ο";        
    }
    public string getCBox2(int i)
    {
        return cbox2[i];
    }
    
    public CBoxSource()
    {
        addCBox1();
        addCBox2();
        go.ForEach(page => goWithAnnouncement.Add(page + 1));
        
    }

}