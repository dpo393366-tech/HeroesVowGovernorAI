using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public static class City89Tests
{
    const BindingFlags F=BindingFlags.NonPublic|BindingFlags.Static;static int checks;
    static AFAGFHJONFJ c;static MKMOJNAJKJE f;static JNHOOCJJPJO d;
    static object Call(string name,params object[] args)=>typeof(StrategicSupport).GetMethod(name,F).Invoke(null,args);
    static void Check(bool ok,string why){checks++;if(!ok)throw new Exception("R89 city: "+why);}
    static PABIPDOFBFG Setup(){
        typeof(CityIntegrationTests).GetMethod("Setup",F).Invoke(null,null);
        c=(AFAGFHJONFJ)typeof(CityIntegrationTests).GetField("c",F).GetValue(null);
        f=NEINKOOBJIH.COCMABJFOFH["F"];d=c.FGBLEMGGOPO[0];c.LMPMNLLGOEC.KJPPGIOCDID.Clear();
        var b=new PABIPDOFBFG{OJPECMNINNA="barracks",HFBAJIGHOAF=MHCEFEODENB.Military,IKELHGAHBID=1,CNCPIILGDEF=0,LJCICFHPMLM=new[]{2,4,6,8}};
        AEHAGJAKNEK.ACIGFKMDOKE[b.OJPECMNINNA]=b;
        AEHAGJAKNEK.ACIGFKMDOKE["market"]=new(){OJPECMNINNA="market",CNCPIILGDEF=100000};return b;
    }
    static Dictionary<string,int> Missing()=>(Dictionary<string,int>)Call("BarracksMissing",c,f);
    static bool Build()=>(bool)Call("BuildBest",c,f);
    static object Revision()=>Call("ReadLayoutRevision",c,f);
    static string Changed(object before)=>(string)before.GetType().GetMethod("Change").Invoke(before,new[]{Revision()});
    public static void Run(){
        Setup();c.LMPMNLLGOEC.OEGNGFBCHFL=0;
        AEHAGJAKNEK.ACIGFKMDOKE["farm"]=new(){OJPECMNINNA="farm",HFBAJIGHOAF=MHCEFEODENB.Agriculture,AJJJJNDNNME=100000};
        Check(Build()&&d.AIMLPNBOIIF.Last().GEKKLMPLDHJ.OJPECMNINNA=="barracks","empty army and food shortage still build barracks first");
        Check(Missing()["barracks"]==1,"unfinished work consumes an allowance");
        Check(Build()&&d.AIMLPNBOIIF.Last().GEKKLMPLDHJ.OJPECMNINNA=="barracks"&&Missing().Count==0,"all legal barracks slots registered before other buildings");
        Check(Build()&&d.AIMLPNBOIIF.Last().GEKKLMPLDHJ.OJPECMNINNA!="barracks","economic work resumes after allowance committed");
        c.Scale=1;Check(Missing()["barracks"]==2&&Build()&&d.AIMLPNBOIIF.Last().GEKKLMPLDHJ.OJPECMNINNA=="barracks","city expansion immediately raises target and restores barracks priority");
        var b=Setup();b.NPCHPDNFAME=2000000;int cash=f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO;
        Check(!Build()&&d.AIMLPNBOIIF.Count==0&&f.DNOGPFHMOMO+c.LMPMNLLGOEC.DNOGPFHMOMO==cash,"unaffordable required barracks preserves budget instead of buying a cheap market");
        b.TechAllowed=false;Check(Missing().Count==0&&Build(),"locked barracks do not freeze the city");
        b.TechAllowed=true;Check(Missing().Count==1&&Missing()["barracks"]==2,"research unlock makes barracks mandatory");
        b=Setup();b.LJCICFHPMLM[0]=-1;Check(Missing().Count==0,"unlimited mod building is not an infinite mandatory quota");
        b=Setup();b.OCJNCECECMH=FFDDNPOHLHN.Private;Check(Missing().Count==0,"private estate never becomes a public barracks target");
        b=Setup();d.AIMLPNBOIIF.Add(new(){GEKKLMPLDHJ=new(){OJPECMNINNA="barracks2",MDOIJIAONJM="barracks"}});Check(Missing()["barracks"]==1,"upgraded barracks count toward base allowance");
        Setup();var revision=Revision();Check(Changed(revision)==null,"unchanged city preserves plan");
        c.Scale++;Check(Changed(revision)=="city-expansion-or-plots","scale change invalidates plan");
        revision=Revision();d.HGNFEMJOPPB.Add(new(){GMIKIMACDBC=d});Check(Changed(revision)=="city-expansion-or-plots","new plots invalidate plan even at identical scale");
        revision=Revision();d.HGNFEMJOPPB.Reverse();Check(Changed(revision)==null,"enumeration order alone does not trigger replanning");
        revision=Revision();AEHAGJAKNEK.ACIGFKMDOKE["barracks"].TechAllowed=false;Check(Changed(revision)=="research-or-building-caps","available catalogue changes invalidate plan");
        revision=Revision();c.HFLAGGEFBOO.Add(new(){CKIJPJFFFOA=new(){new(){HFBAJIGHOAF=DDBHJKOIIPI.BuildingNum,IJNEIHPHMEE="relief",KLCDGPDHENB=2}}});Check(Changed(revision)=="city-goals","new goal invalidates plan");
        revision=Revision();d.AIMLPNBOIIF.Add(new(){GEKKLMPLDHJ=AEHAGJAKNEK.ACIGFKMDOKE["market"]});Check(Changed(revision)=="buildings-changed","external construction invalidates plan");
        Setup();var scan=((IEnumerable<bool>)Call("BuildSteps",c,f)).GetEnumerator();Check(scan.MoveNext(),"building scan yields");c.Scale++;
        bool purchased=false;while(scan.MoveNext())purchased|=scan.Current;Check(!purchased&&d.AIMLPNBOIIF.Count==0,"expansion during scan cancels stale purchase");
        Console.WriteLine($"PASS R89 {checks} barracks-priority, expansion, unlock and stale-plan checks; native calls mocked.");
    }
}
