using System;
using System.Collections.Generic;

public sealed partial class StrategicSupport
{
    // A construction target, not a recruitment target: even an empty city must
    // build its unlocked, finite barracks allowance. Count registered work too.
    static Dictionary<string,int> BarracksMissing(AFAGFHJONFJ c,MKMOJNAJKJE f)
    {
        var result=new Dictionary<string,int>();
        if(c?.FGBLEMGGOPO==null||f==null||AEHAGJAKNEK.ACIGFKMDOKE==null)return result;
        int scale=(int)c.MPGFDBPEHBO(true);
        foreach(var kv in AEHAGJAKNEK.ACIGFKMDOKE){var b=kv.Value;
            if(b==null||b.IKELHGAHBID<=0||b.POLCGKHKLHI||b.OCJNCECECMH==FFDDNPOHLHN.Private
                ||!string.IsNullOrEmpty(b.MDOIJIAONJM)||b.LJCICFHPMLM==null||scale<0||scale>=b.LJCICFHPMLM.Length
                ||b.LJCICFHPMLM[scale]<=0||!b.PLEHAHBBLGL(f.KCKGONGIMDA)||!b.MCGEOICKKEE(c,false))continue;
            // true includes unfinished construction, and the second true counts
            // native upgrades as their base building. Never duplicate a worksite.
            int count=c.ELKFAJKBPKJ(b.OJPECMNINNA,true,true)?.Count??0;
            int missing=Math.Max(0,b.LJCICFHPMLM[scale]-count);
            if(missing>0)result[b.OJPECMNINNA]=missing;
        }
        return result;
    }
    static void AddBarracksGoals(AFAGFHJONFJ c,MKMOJNAJKJE f,GoalPlan goals)
    {
        foreach(var p in BarracksMissing(c,f)){
            goals.Buildings.TryGetValue(p.Key,out int prior);goals.Buildings[p.Key]=Math.Max(prior,p.Value);
            AddNeed(goals,"building:"+p.Key,p.Value,0);
        }
    }
}
