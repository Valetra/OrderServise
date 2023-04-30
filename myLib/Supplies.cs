namespace myLib;

//Размер массива съедобного
public class Supply
{
    string supplyName;
    int supplyCost;

    public Supply(string name, int cost)
    {
        this.supplyName = name;
        this.supplyCost = cost;
    }

    public string Name
    {
        get { return supplyName; }
        set { supplyName = value; }
    }

    public int Coast
    {
        get { return supplyCost; }
        set { supplyCost = value; }
    }
}