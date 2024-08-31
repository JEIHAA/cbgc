using System.Xml.Linq;

public class ResouceData
{
    private static int firewood = 0;
    private static int fleshAmount = 0;

    private static void Init()
    {
        firewood = 0;
        fleshAmount = 0;
    }

    public static int FireWood
    {
        get => firewood;
        set => firewood = value;  // setter
    }

    public static int LogAmount
    {
        get  => firewood;
        set 
        {
            firewood = value;
            //UIManager.instance.UpdateLogAmountUI(value);
        }
    }
    public static int FleshAmount
    {
        get { return fleshAmount; }
        set
        {
            fleshAmount = value;
            //UIManager.instance.UpdateFleshAmountUI(value);
        }
    }
}
