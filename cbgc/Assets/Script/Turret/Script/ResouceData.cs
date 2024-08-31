public class ResouceData
{
    static void Init()
    {
        logAmount = 0;
        fleshAmount = 0;
    }
    static int logAmount = 0;
    public static int LogAmount
    {
        get { return logAmount; }
        set {
            logAmount = value;
            UIManager.instance.UpdateLogAmountUI(value);
            }
    }
    static int fleshAmount = 0;
    public static int FleshAmount
    {
        get { return fleshAmount; }
        set
        {
            fleshAmount = value;
            UIManager.instance.UpdateFleshAmountUI(value);
        }
    }
}
