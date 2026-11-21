using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using O=OfficerBalancePolicy.Officer;
public static class OfficerBalanceTests
{
    static int checks,serial;
    static object Call(string n,params object[] args)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,args);
    static void Check(bool b,string message){checks++;if(!b)throw new Exception(message);}
    static O Officer(double v)=>new O("p"+(++serial),Enumerable.Repeat(v,14).ToArray());
    static List<O> Roster(int n,double v)=>Enumerable.Range(0,n).Select(_=>Officer(v)).ToList();
    static double[] Mean(IEnumerable<List<O>> rosters){var all=rosters.SelectMany(x=>x).ToArray();return Enumerable.Range(0,14).Select(d=>all.Average(h=>h.Ability[d])).ToArray();}
    static double Loss(List<List<O>> rosters,double[] mean)=>rosters.Sum(r=>.005*r.Count*r.Count+Enumerable.Range(0,14).Sum(d=>Math.Pow((r.Count==0?0:r.Average(h=>h.Ability[d]))-mean[d],2)/Math.Pow(Math.Max(50,mean[d]),2)/14));
    static int Simulate(List<List<O>> rosters,int rounds){var mean=Mean(rosters);int moves=0;int total=rosters.Sum(x=>x.Count);var ids=rosters.SelectMany(x=>x).Select(h=>h.Id).OrderBy(x=>x).ToArray();
        for(int round=0;round<rounds;round++){
            double best=.015;List<O> src=null,dst=null;O chosen=null;
            foreach(var a in rosters)foreach(var b in rosters){if(a==b||a.Count<=3||b.Count>=Math.Ceiling(total/(double)rosters.Count)+1)continue;
                foreach(var h in a){double gain=OfficerBalancePolicy.Gain(a,b,h,mean);if(gain>best){best=gain;src=a;dst=b;chosen=h;}}}
            if(chosen==null)break;double before=Loss(rosters,mean);src.Remove(chosen);dst.Add(chosen);moves++;
            Check(Loss(rosters,mean)<before-.014999,"every committed simulated move lowers fixed-cohort count/ability loss");
            Check(ids.SequenceEqual(rosters.SelectMany(x=>x).Select(h=>h.Id).OrderBy(x=>x)),"officer identities and abilities conserved");
        }return moves;
    }
    static AFAGFHJONFJ City(string id,int count,double ability){var c=new AFAGFHJONFJ{OJPECMNINNA=id,Roster=new()};c.LMPMNLLGOEC.LMMNKIOAGLC="F";c.LMPMNLLGOEC.IPJHOOJBCKI="gov";
        for(int n=0;n<count;n++){c.Roster.Add(new(){OJPECMNINNA=id+n,LMPMNLLGOEC=new(){CNOMNBCPJDI=id},Ability=Enumerable.Repeat(ability,14).ToArray()});c.LMPMNLLGOEC.KJPPGIOCDID.Add(new());}
        MICGPAMNDKM.IJPKEMPIFIH[id]=c;return c;
    }
    static void Setup(){new StrategicSupport().Load();MICGPAMNDKM.IJPKEMPIFIH=new();ICGAEMLACJG.HFJIECIBFKB.Clear();NEINKOOBJIH.COCMABJFOFH.Clear();NEINKOOBJIH.JMIADAFPGMC.Clear();NEINKOOBJIH.AOPJBNBGPLP.Clear();
        var f=new MKMOJNAJKJE{IBGCLCAIBOL="F"};f.ANMMMOIKLEG["gov"]=new(){LMMNKIOAGLC="F"};NEINKOOBJIH.COCMABJFOFH["F"]=f;Call("Ready");}
    static void ClearTimers()=>((IDictionary)typeof(StrategicSupport).GetField("next",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)).Clear();
    static void Arrive(AFAGFHJONFJ source,AFAGFHJONFJ target){var team=source.Last;foreach(var h in team.m_listHero){source.Roster.Remove(h);target.Roster.Add(h);h.LMPMNLLGOEC.CNOMNBCPJDI=target.OJPECMNINNA;}
        foreach(var a in team.m_dicDeployArmy.Values.SelectMany(x=>x)){source.LMPMNLLGOEC.KJPPGIOCDID.Remove(a);target.LMPMNLLGOEC.KJPPGIOCDID.Add(a);}ICGAEMLACJG.HFJIECIBFKB.Clear();}
    public static void Run(){
        var a=Roster(8,90);var b=Roster(8,20);Check(OfficerBalancePolicy.Gain(a,b,a[0])>.015,"equal headcounts can still trigger ability correction");
        var cities=new List<List<O>>{a,b};int moves=Simulate(cities,100);
        Check(Math.Abs(a.Average(h=>h.Ability[0])-b.Average(h=>h.Ability[0]))<12&&moves<30,"strong-only/weak-only cities mix and converge");
        Console.WriteLine($"Ability fixture [8x90,8x20] -> counts [{a.Count},{b.Count}], means [{a.Average(h=>h.Ability[0]):F2},{b.Average(h=>h.Ability[0]):F2}], moves={moves}");
        a=Roster(10,30);a[0].Ability=Enumerable.Repeat(300d,14).ToArray();b=Roster(3,20);
        Check(double.IsNegativeInfinity(OfficerBalancePolicy.Gain(a,b,a[0])),"sole outstanding specialist cannot be exported");
        a=Roster(10,70);a[0].Ability[3]=300;Check(double.IsNegativeInfinity(OfficerBalancePolicy.Gain(a,b,a[0])),"single critical ability protected despite high overall roster average");
        a=Roster(10,90);b=Roster(4,20);var weak=Officer(10);a.Add(weak);
        Check(OfficerBalancePolicy.Gain(a,b,a[0])>OfficerBalancePolicy.Gain(a,b,weak),"deficient city gets meaningful talent instead of weakest dump");
        a=Roster(8,50);b=Roster(8,50);Check(OfficerBalancePolicy.Gain(a,b,a[0])<.015,"already balanced equal rosters stay still");
        a=Roster(8,20);b=Roster(8,90);foreach(var h in a)for(int d=0;d<7;d++)h.Ability[d]=90;foreach(var h in b)for(int d=0;d<7;d++)h.Ability[d]=20;
        cities=new(){a,b};moves=Simulate(cities,100);Check(moves>0&&Enumerable.Range(0,14).All(d=>Math.Abs(a.Average(h=>h.Ability[d])-b.Average(h=>h.Ability[d]))<12),"equal overall average but opposite specialties also balance");
        var rng=new Random(5282);for(int n=0;n<500;n++){
            cities=new();for(int k=0;k<4;k++){var list=Roster(8,50);int level=n%2==0?20+k*35:50;foreach(var h in list)for(int d=0;d<14;d++)h.Ability[d]=Math.Max(0,level+rng.Next(-10,11));cities.Add(list);}
            moves=Simulate(cities,200);Check(moves<200,"random multi-city redistribution terminates");
        }
        Setup();var src=City("strong",8,90);var dest=City("weak",8,20);src.FKMKGNPFLFP.Add(dest);dest.FKMKGNPFLFP.Add(src);
        Check((bool)Call("BalanceGarrison",src)&&src.Last.m_isEnterCity&&src.Last.gameObject.activeSelf,"native route dispatches selected capable officer with equal counts");
        Check(src.Roster.Count==8&&dest.Roster.Count==8,"dispatch does not teleport or replace authoritative rosters");
        var mapped=new CLAGKDECFID{Ability=Enumerable.Range(1,14).Select(x=>(double)x).ToArray()};
        Check(((O)Call("ReadOfficer",mapped)).Ability.SequenceEqual(mapped.Ability),"all six attributes and eight role abilities read independently");
        ClearTimers();Check(!(bool)Call("BalanceGarrison",dest),"incoming transfer blocks premature reversal");
        Arrive(src,dest);int actualMoves=1;
        for(int step=0;step<50;step++){ClearTimers();foreach(var city in new[]{src,dest})if((bool)Call("BalanceGarrison",city)){Arrive(city,city==src?dest:src);actualMoves++;}}
        Check(Math.Abs(src.Roster.Average(h=>h.Ability[0])-dest.Roster.Average(h=>h.Ability[0]))<15&&actualMoves<30,"native-call fixture converges after simulated travel");
        Console.WriteLine($"Native-call fixture counts [{src.Roster.Count},{dest.Roster.Count}], means [{src.Roster.Average(h=>h.Ability[0]):F2},{dest.Roster.Average(h=>h.Ability[0]):F2}], moves={actualMoves}; arrivals simulated.");
        Setup();src=City("donor",10,90);dest=City("recipient",3,20);src.FKMKGNPFLFP.Add(dest);src.Roster[0].Ability=Enumerable.Repeat(10d,14).ToArray();
        Check((bool)Call("BalanceGarrison",src)&&src.Last.m_listHero[0]!=src.Roster[0],"native candidate filter selects useful talent rather than first/weakest officer");
        Check(typeof(StrategicSupport).GetField("selectedOfficer",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)==null,"selection scope cleared after success");
        Setup();src=City("fault",10,90);dest=City("empty",1,20);src.FKMKGNPFLFP.Add(dest);src.ThrowGeneration=true;
        try{Call("BalanceGarrison",src);throw new Exception("Expected injected failure");}catch(TargetInvocationException){}
        Check(typeof(StrategicSupport).GetField("selectedOfficer",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)==null,"selection scope cleared after native exception");
        Check(src.Roster.Count==10&&src.LMPMNLLGOEC.KJPPGIOCDID.Count==10,"native exception restores troop roster and preserves heroes");
        Setup();src=City("ineligible",10,90);dest=City("empty",1,20);src.FKMKGNPFLFP.Add(dest);foreach(var h in src.Roster)h.NativeEligible=false;
        Check(!(bool)Call("BalanceGarrison",src)&&src.Calls==0,"native officer eligibility cannot be bypassed");
        Setup();src=City("civilians",10,90);dest=City("empty",1,20);src.FKMKGNPFLFP.Add(dest);foreach(var h in src.Roster)h.HFBAJIGHOAF=3;
        Check(!(bool)Call("BalanceGarrison",src),"ordinary residents cannot count as officer reserves");
        Setup();src=City("protected",10,30);dest=City("thin",3,20);src.FKMKGNPFLFP.Add(dest);src.Roster[0].Ability=Enumerable.Repeat(300d,14).ToArray();
        Check((bool)Call("BalanceGarrison",src)&&src.Last.m_listHero[0]!=src.Roster[0],"real dispatch preserves sole outstanding local specialist");
        Console.WriteLine($"PASS {checks} ability balancing assertions (pure policy and native-call stubs; not live campaign)");
    }
}

