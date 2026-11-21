using System;
using System.Collections.Generic;

// Pure policy: compare each ability separately, so a high aggregate cannot hide
// an entire city's missing military or civil capability.
public static class OfficerBalancePolicy
{
    public static double ExchangeGain(List<Officer> a,List<Officer> b,Officer give,Officer take){
        if(a.Count<4||b.Count<4||!a.Contains(give)||!b.Contains(take)||give.Ability.Length!=take.Ability.Length)return double.NegativeInfinity;
        return ExchangeGain(new Profile(a,give.Ability.Length),new Profile(b,take.Ability.Length),give,take);
    }
    public static double ExchangeGain(Profile p,Profile q,Officer give,Officer take){
        if(p.Count<4||q.Count<4||!p.Members.Contains(give)||!q.Members.Contains(take)||give.Ability.Length!=take.Ability.Length)return double.NegativeInfinity;
        double gain=0;
        for(int d=0;d<give.Ability.Length;d++){
            // Neither departure may leave a unique specialist without a substitute.
            double ap=give.Ability[d]==p.Peak[d]?p.Second[d]:p.Peak[d],bp=take.Ability[d]==q.Peak[d]?q.Second[d]:q.Peak[d];
            if(ap<p.Peak[d]*.8||bp<q.Peak[d]*.8||p.Sum[d]-give.Ability[d]<p.Sum[d]*.6||q.Sum[d]-take.Ability[d]<q.Sum[d]*.6)return double.NegativeInfinity;
            double before=p.Sum[d]/p.Count-q.Sum[d]/q.Count,after=(p.Sum[d]-give.Ability[d]+take.Ability[d])/p.Count-(q.Sum[d]-take.Ability[d]+give.Ability[d])/q.Count;
            gain+=(before*before-after*after)/(2500*give.Ability.Length);
        }return gain;
    }
    public sealed class Officer
    {
        public string Id;
        public double[] Ability;
        public Officer(string id, params double[] values){Id=id;Ability=values;}
    }
    public sealed class Profile
    {
        public readonly int Count;
        public readonly double[] Sum,Peak,Second;
        internal readonly HashSet<Officer> Members;
        public Profile(List<Officer> roster,int dimensions){Count=roster.Count;Members=new HashSet<Officer>(roster);
            Sum=new double[dimensions];Peak=new double[dimensions];Second=new double[dimensions];
            foreach(var h in roster)for(int d=0;d<dimensions;d++){double value=h.Ability[d];Sum[d]+=value;
                if(value>Peak[d]){Second[d]=Peak[d];Peak[d]=value;}else if(value>Second[d])Second[d]=value;}
        }
    }
    public static double Gain(List<Officer> source,List<Officer> target,Officer candidate,double[] baseline=null)
        =>Gain(new Profile(source,candidate.Ability.Length),new Profile(target,candidate.Ability.Length),candidate,baseline);
    public static double Gain(Profile source,Profile target,Officer candidate,double[] baseline=null)
    {
        if(source.Count<2||!source.Members.Contains(candidate))return double.NegativeInfinity;
        int dims=candidate.Ability.Length;
        if(dims==0)return double.NegativeInfinity;
        double before= .0025*Math.Pow(source.Count-target.Count,2);
        double after= .0025*Math.Pow(source.Count-target.Count-2,2);
        for(int d=0;d<dims;d++){
            double a=source.Sum[d],b=target.Sum[d],peak=source.Peak[d];
            double remainingPeak=candidate.Ability[d]==peak?source.Second[d]:peak;
            // Retain both a capable replacement for each specialist and at least
            // 60% of home capability. Incoming officers cannot satisfy this guard.
            if(remainingPeak+1e-6<peak*.8||a-candidate.Ability[d]+1e-6<a*.6)return double.NegativeInfinity;
            double mean=baseline==null?(a+b)/(source.Count+target.Count):baseline[d];
            double scale=Math.Max(50,mean);
            before+=(Math.Pow(a/source.Count-mean,2)+Math.Pow((target.Count==0?0:b/target.Count)-mean,2))/(scale*scale*dims);
            after+=(Math.Pow((a-candidate.Ability[d])/(source.Count-1)-mean,2)+Math.Pow((b+candidate.Ability[d])/(target.Count+1)-mean,2))/(scale*scale*dims);
        }
        return before-after;
    }
}
