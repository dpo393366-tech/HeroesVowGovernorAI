using System;

// All rates use normal-speed simulation seconds, never wall-clock time.
public sealed class SupplyObservation
{
    public double Time, Burn, Loss, MoraleLoss;
    public int Food, Soldiers, Morale, Samples;
    public void Sample(double now,int food,int soldiers,int morale,double nativeBurn)
    {
        if(!double.IsFinite(now)||!double.IsFinite(nativeBurn)||nativeBurn<0)return;
        double dt=now-Time;
        if(Samples>0&&dt>0){
            // A daily discrete food deduction must not be extrapolated as a per-frame burst.
            double observed=Math.Max(0,Food-food)/dt;
            Burn=Samples==1?observed:Burn*.7+observed*.3;
            Loss=Loss*.5+Math.Max(0,Soldiers-soldiers)/dt*.5;
            MoraleLoss=MoraleLoss*.5+Math.Max(0,Morale-morale)/dt*.5;
        }
        Time=now;Food=Math.Max(0,food);Soldiers=Math.Max(0,soldiers);Morale=Math.Max(0,morale);Samples++;
        Burn=Math.Max(nativeBurn,Burn);
    }
    public void Credit(int food){Food=(int)Math.Min(int.MaxValue,(long)Food+Math.Max(0,food));}
}

public static class SupplyPolicy
{
    public static int Available(int stock,long reserve,long reserved)
        =>(int)Math.Clamp((long)stock-Math.Max(0,reserve)-Math.Max(0,reserved),0,int.MaxValue);
    public static int Amount(SupplyObservation o,double eta,double cover,double buffer,int spare)
    {
        if(o==null||o.Soldiers<=0||o.Burn<=0||!double.IsFinite(o.Burn)||!double.IsFinite(eta)||eta<0
            ||!double.IsFinite(cover)||cover<=0||!double.IsFinite(buffer)||buffer<0||spare<=0)return 0;
        // Avoid sending food to an army projected to be destroyed before delivery.
        if(o.Loss>0&&o.Soldiers/o.Loss<=eta*1.25)return 0;
        // Morale decline advances the request deadline; food is never claimed to cure combat losses.
        double moraleDeadline=o.MoraleLoss>0?Math.Max(0,o.Morale-15)/o.MoraleLoss:double.PositiveInfinity;
        double safeFoodTime=o.Food/o.Burn;
        if(safeFoodTime>eta+buffer&&moraleDeadline>eta+buffer)return 0;
        double survivors=Math.Clamp(1-o.Loss*eta/Math.Max(1,o.Soldiers),.25,1);
        double atArrival=Math.Max(0,o.Food-o.Burn*eta);
        double desired=o.Burn*survivors*cover-atArrival;
        return (int)Math.Clamp(Math.Ceiling(desired),0,spare);
    }
    public static double Priority(SupplyObservation o)
        =>Math.Min(o.Food/Math.Max(.001,o.Burn),o.MoraleLoss>0?Math.Max(0,o.Morale-15)/o.MoraleLoss:double.PositiveInfinity);
}
