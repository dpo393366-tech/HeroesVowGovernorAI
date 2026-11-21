using System;

public static class InvestmentPolicy
{
    // All transfers conserve currency; reserves are never treated as available money.
    public static int TransferForPurchase(int treasury,int city,int price,int treasuryReserve,int cityReserve)
    {
        long need=Math.Max(0L,(long)price+cityReserve-city);
        long spare=Math.Max(0L,(long)treasury-treasuryReserve);
        return (int)Math.Min(Math.Min(need,spare),Math.Max(0L,(long)int.MaxValue-city));
    }
    public static bool CanPurchase(int treasury,int city,int price,int treasuryReserve,int cityReserve)
        =>price>=0 && (long)city+TransferForPurchase(treasury,city,price,treasuryReserve,cityReserve)-price>=cityReserve;
    public static bool ZoneAllows(int zone,int kind)=>kind==0 || zone==0 || zone==kind;
    public static double Score(int priority,double benefit,int cost,int area)
        =>priority*1e9+Math.Min(1e8,Math.Max(0,benefit)*10000/Math.Max(1,cost)+100.0/Math.Max(1,area));
}
