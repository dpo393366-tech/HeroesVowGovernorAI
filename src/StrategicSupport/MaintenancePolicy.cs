using System;

public static class MaintenancePolicy
{
    public static int RepairAmount(int hp,int maximum)
    {
        if(maximum<=0||hp<0||hp>=maximum)return 0;
        return (int)Math.Min((long)maximum-hp,Math.Max(1L,((long)maximum+3)/4));
    }
    // Paid acceleration beyond the game's passive repair: a complete rebuild's
    // quarter-price, proportional to restored durability, rounded up.
    public static int RepairCost(int price,int amount,int maximum)
    {
        if(price<0||amount<=0||maximum<=0||amount>maximum)return -1;
        return (int)Math.Max(1,((long)price*amount+4L*maximum-1)/(4L*maximum));
    }
    public static bool WorthMoving(double oldValue,double newValue,int cost)
        =>double.IsFinite(oldValue)&&double.IsFinite(newValue)&&cost>=0
        &&newValue-oldValue>=Math.Max(1,Math.Abs(oldValue)*.1)
        &&(newValue-oldValue)*120>=cost;
}
