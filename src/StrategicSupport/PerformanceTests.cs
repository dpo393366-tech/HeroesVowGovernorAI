using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using UnityEngine;
public static class PerformanceTests
{
 static int tests;
 static void Check(bool b,string why){tests++;if(!b)throw new Exception("performance: "+why);}
 static object Call(string n,params object[] a)=>typeof(StrategicSupport).GetMethod(n,BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,a);
 static void Set(string n,object v)=>typeof(StrategicSupport).GetField(n,BindingFlags.NonPublic|BindingFlags.Static).SetValue(null,v);
 static T Get<T>(Type t,string n)=>(T)t.GetField(n,BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
 static IEnumerator<string> Steps(Action a,int count){for(int i=0;i<count;i++){a();yield return "stage";}}
 static IEnumerator<string> Fault(){yield return "start";throw new Exception("injected");}
 static void Setup(){typeof(CityIntegrationTests).GetMethod("Setup",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,null);Set("schedulerOnline",true);Time.timeScale=1;}
 public static void Run(){
  var q=new WorkQueue();var trace=new List<int>();int errors=0;
  for(int i=0;i<70;i++){int id=i;Check(q.Add("c"+i,()=>Steps(()=>trace.Add(id),9)),"burst accepted");}
  Check(trace.Count==0&&q.Count==70,"enqueue never executes game actions");
  Check(!q.Add("c0",()=>Steps(()=>throw new Exception(),1)),"duplicate coalesced");
  Check(q.Pump(0,8,(_,_)=>{},_=>errors++)==1&&trace.Count==1,"budget stops between indivisible calls");
  while(q.Count>0)q.Pump(100,8,(_,_)=>{},_=>errors++);
  Check(errors==0&&trace.Count==630,"all burst jobs finish without loss");
  Check(trace.Take(70).SequenceEqual(Enumerable.Range(0,70)),"cities receive fair round robin turns");
  q.Add("bad",Fault);q.Add("good",()=>Steps(()=>trace.Add(99),2));
  while(q.Count>0)q.Pump(100,8,(_,_)=>{},_=>errors++);
  Check(errors==1&&trace.Count(x=>x==99)==2,"fault does not stop unrelated cities");
  int old=0,fresh=0;
  q.Add("same",()=>Steps(()=>{old++;q.Clear();q.Add("same",()=>Steps(()=>fresh++,1));},2));
  while(q.Count>0)q.Pump(100,8,(_,_)=>{},_=>errors++);
  Check(old==1&&fresh==1,"world reset prevents stale continuation and same-key deletion");
  Setup();var c=Get<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");var d=Get<JNHOOCJJPJO>(typeof(CityIntegrationTests),"d");
  AEHAGJAKNEK.ACIGFKMDOKE["market"]=new(){OJPECMNINNA="market",HFBAJIGHOAF=MHCEFEODENB.Commerce,NPCHPDNFAME=600};
  Call("AfterRecruitInvestment",c,false);var scheduled=Get<WorkQueue>(typeof(StrategicSupport),"work");
  Check(d.AIMLPNBOIIF.Count==0&&scheduled.Count==1,"native callback only queues investment");
  Time.timeScale=0;StrategicSupport.PumpEconomy();Check(d.AIMLPNBOIIF.Count==0&&scheduled.Count==1,"pause preserves pending work without spending");
  Time.timeScale=1;int turns=0;while(scheduled.Count>0&&turns++<1000)StrategicSupport.PumpEconomy();
  Check(scheduled.Count==0&&d.AIMLPNBOIIF.Count>0,"queued native construction completes");
  Setup();c=Get<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");d=Get<JNHOOCJJPJO>(typeof(CityIntegrationTests),"d");
  Call("AfterRecruitInvestment",c,false);int money=c.LMPMNLLGOEC.DNOGPFHMOMO;c.LMPMNLLGOEC.LMMNKIOAGLC="captured";
  turns=0;while(scheduled.Count>0&&turns++<1000)StrategicSupport.PumpEconomy();
  Check(c.LMPMNLLGOEC.DNOGPFHMOMO==money&&d.AIMLPNBOIIF.Count==0,"captured city cannot spend old faction funds");
  // Change ownership after an incremental scan has already started.
  Setup();c=Get<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");d=Get<JNHOOCJJPJO>(typeof(CityIntegrationTests),"d");
  var f=Get<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");
  for(int i=0;i<100;i++)AEHAGJAKNEK.ACIGFKMDOKE["m"+i]=new(){OJPECMNINNA="m"+i,HFBAJIGHOAF=MHCEFEODENB.Commerce,NPCHPDNFAME=600};
  using(var scan=((IEnumerable<bool>)Call("BuildSteps",c,f)).GetEnumerator()){
   Check(scan.MoveNext()&&!scan.Current,"large search yields before purchase");
   int treasury=f.DNOGPFHMOMO;c.LMPMNLLGOEC.IPJHOOJBCKI="new-governor";
   while(scan.MoveNext())Check(!scan.Current,"no stale scan commit");
   Check(f.DNOGPFHMOMO==treasury&&d.AIMLPNBOIIF.Count==0,"mid-scan governance change aborts safely");
  }
  Setup();c=Get<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");f=Get<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");var governor=f.ANMMMOIKLEG["gov"];f.DNOGPFHMOMO=415349;
  governor.OnResearch=()=>f.DNOGPFHMOMO-=10000;
  using(var tasks=(IEnumerator<string>)Call("CityResearchWork",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI)){while(tasks.MoveNext()){} }
  Check(governor.ResearchCalls==4&&f.DNOGPFHMOMO==375349,"wealthy research cycle launches four paid native projects with yields");
  Setup();c=Get<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");f=Get<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");governor=f.ANMMMOIKLEG["gov"];f.DNOGPFHMOMO=40000;governor.OnResearch=()=>f.DNOGPFHMOMO-=10000;
  using(var tasks=(IEnumerator<string>)Call("CityResearchWork",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI)){while(tasks.MoveNext()){} }
  Check(governor.ResearchCalls==1&&f.DNOGPFHMOMO==30000,"multi-project cycle stops at treasury reserve");
  Setup();c=Get<AFAGFHJONFJ>(typeof(CityIntegrationTests),"c");f=Get<MKMOJNAJKJE>(typeof(CityIntegrationTests),"f");governor=f.ANMMMOIKLEG["gov"];
  using(var tasks=(IEnumerator<string>)Call("CityResearchWork",c,f,c.LMPMNLLGOEC.IPJHOOJBCKI)){while(tasks.MoveNext()){} }
  Check(governor.ResearchCalls==1,"no-progress research cycle stops without spinning");
  Set("schedulerOnline",false);Call("ResetWork");
  // Production logger receives only text; concurrent producers are preserved.
  for(int i=0;i<10;i++)AsyncJournal.Flush(true);
  string token="journal-test-"+Guid.NewGuid();
  System.Threading.Tasks.Parallel.For(0,100,i=>AsyncJournal.Write(token+" "+i));
  for(int i=0;i<10;i++)AsyncJournal.Flush(true);
  string file=Path.Combine(BepInEx.Paths.BepInExRootPath,"StrategicSupport-investment.log");
  Check(File.ReadLines(file).Count(l=>l.Contains(token))==100,"batched writer preserves concurrent records");
  Console.WriteLine("PASS performance queue/native-call-stub checks="+tests);
 }
}
