using System;

internal static class AwardRules {
    internal sealed class Gear {internal int kind;internal double[] values;internal string[] souls;}
    internal static int RecipientCount(int count,int percent)=>(int)(((long)Math.Max(0,count)*Math.Clamp(percent,1,100)+99)/100);
    internal static int GrowthRole(int force,int wise)=>force>0&&force>wise?1:wise>0&&wise>force?2:0;
    internal static bool PreferredGrowth(int role,int force,int wise)=>role!=0&&role==(force>wise?1:wise>force?2:0);
    // Conservative upgrade: never trade away a stat or an existing granted soul.
    // Incomparable trade-offs remain in the treasury for manual choice/another officer.
    internal static bool Upgrade(Gear next,Gear current,bool weapon){
        if(weapon&&next.kind!=current.kind)return false;
        if(next.values.Length!=current.values.Length)return false;
        bool better=false;
        for(int i=0;i<next.values.Length;i++){
            double a=next.values[i],b=current.values[i];
            if(!double.IsFinite(a)||!double.IsFinite(b)||a<b-1e-6)return false;
            if(a>b+1e-6)better=true;
        }
        for(int i=0;i<current.souls.Length;i++){
            string a=next.souls[i]??"",b=current.souls[i]??"";
            if(b.Length>0&&a!=b)return false;
            if(b.Length==0&&a.Length>0)better=true;
        }
        return better;
    }
}
