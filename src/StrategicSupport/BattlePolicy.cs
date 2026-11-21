using System;

public sealed class AttritionWindow
{
    readonly double[] at=new double[6];
    readonly int[] friendly=new int[6],enemy=new int[6];
    int size,head;
    public double FriendlyLoss {get;private set;}
    public double EnemyLoss {get;private set;}
    public int Samples=>size;
    public void Sample(double time,int own,int foe)
    {
        if(size>0){int last=(head+5)%6;
            // Reinforcements, healing or a change of clock must not hide casualties
            // or masquerade as casualties. Collect a fresh observation window.
            if(time<=at[last]){size=0;head=0;}
        }
        at[head]=time;friendly[head]=own;enemy[head]=foe;head=(head+1)%6;size=Math.Min(6,size+1);
        FriendlyLoss=Rate(friendly);EnemyLoss=Rate(enemy);
    }
    double Rate(int[] counts){
        // Median interval losses reject an isolated burst. Positive arrivals do
        // not erase the whole history; they contribute a zero-loss interval.
        if(size<3)return 0;
        var rates=new double[size-1];int first=(head-size+6)%6;
        for(int i=1;i<size;i++){int a=(first+i-1)%6,b=(first+i)%6;rates[i-1]=Math.Max(0,counts[a]-counts[b])/Math.Max(.001,at[b]-at[a]);}
        Array.Sort(rates);return rates[(rates.Length-1)/2];
    }
}
public static class BattlePolicy
{
    public static int Need(int own,int foe,double ownLoss,double foeLoss,double eta,int arriving,double ratio)
        =>AssessNeed(own,foe,ownLoss,foeLoss,eta,arriving,ratio,out _);
    public static int AssessNeed(int own,int foe,double ownLoss,double foeLoss,double eta,int arriving,double ratio,out string reason)
    {
        reason="invalid-observation";
        if(own<=0||foe<=0||!double.IsFinite(eta)||eta<0||!double.IsFinite(ownLoss)||!double.IsFinite(foeLoss))return 0;
        ownLoss=Math.Max(0,ownLoss);foeLoss=Math.Max(0,foeLoss);
        // Do not send a rescue that cannot arrive before the position collapses.
        if(ownLoss>0&&eta>=own/ownLoss*.8){reason="collapse-before-arrival";return 0;}
        double futureOwn=Math.Max(0,own-ownLoss*eta);
        double futureEnemy=Math.Max(0,foe-foeLoss*eta);
        if(futureEnemy<=0){reason="enemy-projected-defeated";return 0;}
        // A neutral/even engagement is not a rescue request. Require a real
        // projected deficit before filling to the configured safety margin.
        if(futureOwn+Math.Max(0,arriving)>=futureEnemy*.95){reason="covered-at-arrival";return 0;}
        reason="reinforcement-needed";
        return (int)Math.Clamp(Math.Ceiling(futureEnemy*Math.Clamp(ratio,1,1.5)-futureOwn-Math.Max(0,arriving)),0,int.MaxValue);
    }
    public static double Arrival(double distance,double speed,double mobilization)
        =>distance>=0&&double.IsFinite(distance)&&speed>0&&double.IsFinite(speed)
            ?Math.Max(0,mobilization)+distance/speed:double.PositiveInfinity;
}
