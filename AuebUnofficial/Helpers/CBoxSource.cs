using System;

public class CBoxSource
{
    string[] cbox1 = new string[8];
    string[] cbox2 = new string[4];
    int[] go = {1,2,3,4,6,7,8,9,11,12,13,14,16,17,18,22,26,27,28,30,33,34,35,36,39,40,
        41,42,44,45,46,47};
    
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
    public int getTable(int i)
    {
        DateTime now = DateTime.Now;
        string a = now.Year.ToString();
        DateTime endSpringSemester = new DateTime(int.Parse(a),08,20);
        DateTime springSemester = new DateTime(int.Parse(a),02,24);
        if (now >= springSemester && now <= endSpringSemester&&i>=12)
        {
            return go[i]-1;
        }else
        {
            return go[i];
        }
    }
    public CBoxSource()
    {
        addCBox1();
        addCBox2();
        
    }

}