using System;
using System.Collections.Generic;

public static class StrategyPolicy
{
    public static int Transfer(int treasury, int cityCash, long desired, int reserve, double share, int limit)
    {
        if (treasury <= reserve || cityCash < 0 || desired <= cityCash) return 0;
        long spare = (long)treasury - Math.Max(0, reserve);
        return (int)Math.Max(0, Math.Min(Math.Min(desired-cityCash, int.MaxValue-(long)cityCash),
            Math.Min(Math.Max(0,limit), (long)(spare*Math.Clamp(share,0,1)))));
    }
    // Bound the worst case: even if native selection sends the largest units first,
    // the requested garrison fraction and minimum number of units remain at home.
    public static int ReserveUnits(IReadOnlyList<int> soldiers, double fraction, int minimum, int alreadySent)
    {
        var sorted=new List<int>(); long total=0;
        foreach(int n in soldiers){ sorted.Add(Math.Max(0,n));total+=Math.Max(0,n); }
        sorted.Sort((a,b)=>b.CompareTo(a));
        long keep=(long)Math.Ceiling((total+Math.Max(0,alreadySent))*Math.Clamp(fraction,0,1));
        long remaining=total; int send=0;
        while(send<sorted.Count-Math.Max(0,minimum) && remaining-sorted[send]>=keep)
            remaining-=sorted[send++];
        return sorted.Count-send;
    }
    public static int Deficit(int enemy, int garrison, int incoming, double coverage)
        => (int)Math.Clamp(Math.Ceiling(Math.Max(0,enemy)*Math.Clamp(coverage,1,2))
            -Math.Max(0,garrison)-Math.Max(0,incoming),0,int.MaxValue);
}
